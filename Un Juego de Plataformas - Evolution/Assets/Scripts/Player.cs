using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; //con esta variable podemos controlar la velocidad del player y los goombas
    public float jumpForce; //controlamos la fuerza con la que salta 
    public GameController gameController; //para acceder a componentes del gamecontroller
    public bool grounded; //usamos esta variable para habilitar la posibilidad de saltar cuando el player este tocando el suelo
    public GameObject soundJump, soundGetCoin, soundShoot; //hemos asociado sonidos a estos dos gameobjects asi luego los activaremos instanciandolos
    public GameObject superShoot;
    public Transform lSpawnShot;
    public Transform rSpawnShot;
    public bool extraLife; //ponemos a true esta variable cuando el player coge una seta
    private Rigidbody2D rigidBody; //para acceder a componentes del rigidBody
    private Animator animator; //para acceder a componentes del animator
    private SpriteRenderer spriteRenderer; 
    public static Player instancePlayer;
    public bool savedRespawnPoint = false;
    public Vector3 respawnPosition; 

    //inicializamos las variables con los componentes correspondientes asociados al gameobject al que asocuamos este script
    void Start() {
        Player.instancePlayer = this;
        respawnPosition = gameObject.transform.position;
        rigidBody = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        PlayerDirectionAndAnimation();
        PlayerShoot ();
        Playerjump();
    }

    private void PlayerDirectionAndAnimation(){
        float direction = Input.GetAxis("Horizontal"); //controlamos la dirección del personaje segun las entradas del sistema de inputs que nos ofrece unity 
        //direction sera 0 si no hay ninguna input, personaje parado, si va a la izquierda sera -1, derecha +1
        if (direction != 0){
            animator.SetBool("Run", true);//como se mueve activamos la animacion en movimiento
            rigidBody.AddForce(new Vector2(direction * speed * Time.deltaTime,0)); //aplicamos una fuerza en la direccion de la input recibida
            if (direction < 0)
                spriteRenderer.flipX = true;//si va a la izquierda giramos la visualización del personaje
            else
                spriteRenderer.flipX = false;
        }else
            animator.SetBool("Run", false); // si no se mueve el player, activamos la animación del personaje parado
        
        if (rigidBody.velocity.y < 0){
            animator.SetBool("Falling", true);
        }
    }

    private void Playerjump(){
        //si pulsamos la tecla espacio y ademas estamos pisando el suelo podemos saltar
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            Instantiate(soundJump); //instanciamos el gameobjet que contiene el sonido de salto
            animator.SetBool("Jump", true); //activamos la animación de salto
            rigidBody.AddForce(new Vector2(0, jumpForce)); //aplicamos fuerza al salto
            grounded = false; //indicamos que ya no esta en el salto, asi evitamos que siga saltando cuando esta en el aire
        }
    }

    private void PlayerShoot (){
        //Shoot
        GameObject shoot = null;
        //if the player has extralife means he is in modo super
        if (extraLife && Input.GetKeyDown(KeyCode.E)){
            Instantiate(soundShoot);
            shoot = Instantiate(superShoot);
        }
       //Shoot in the direction the player is looking at        
       if (spriteRenderer.flipX){
            if (shoot != null){
                shoot.transform.position = lSpawnShot.position;
                shoot.GetComponent<SpriteRenderer>().flipY = true;
                shoot.GetComponent<ShootModeSuper>().direction=-1;
            }
        }else{
            if (shoot != null)
                shoot.transform.position = rSpawnShot.position;
        }
    }

    //subrutina para entrar en modo super
    private IEnumerator SuperSonic() {
        Vector2 g = Physics.gravity; 
        Vector2 v = rigidBody.velocity;
        float s = speed;
        speed = 0;
        Physics2D.gravity = Vector3.zero; //deshabilitamos la gravedad para parar a sonic en el mometo que coge la seta
        rigidBody.velocity = Vector2.zero; //detenemos las inercias para parar a sonic
        yield return new WaitForSeconds(1); //esperamos un segundo
        //recuperamos fuerzas, velocidad y movimiento del player antes de iniciar la subrutina
        Physics2D.gravity = g; 
        rigidBody.velocity = v;
        speed = s;
    }

        //Body Colisions entramos en contacto con un elemento (sin trigger)
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyFlying")){
            //si es de tipo enemigo y estamos en modo super, revotamos un poco
            if (extraLife) {
                extraLife = false;
                animator.SetBool("SuperSonic", false);
                rigidBody.AddForce(new Vector2(-jumpForce/2, jumpForce / 2));
                animator.SetBool("Falling", false);
            }else{
                if (savedRespawnPoint == true)
                    gameObject.transform.position = respawnPosition + new Vector3(0,10,0);
                else
                    gameController.GameOver(); //si no estamos en modo super y es de tipo enemigo, llamamos a la función de fin de juego
            }
        }
    }

        //Foot Colisions, cuando el player entre en contacto con un collider con trigger
    private void OnTriggerEnter2D(Collider2D collision){
        String collisionElement = collision.tag;
        switch (collisionElement){
            case "Coin":
                Instantiate(soundGetCoin); //activamos el gameobject que contiene el sonido de coger moneda
                Destroy(collision.gameObject); //destruimos la moneda despues de cogerla
                gameController.AddCoints(); //llamamos a la funcion de contar monedas
                break;
            case "Mushroom":
                Destroy(collision.gameObject); //destruimos la moneda después de cogerla
                gameController.AddPoints(100); //llamamos a la función de sumar puntos
                animator.SetBool("Jump", false); 
                animator.SetTrigger("Mushroom");
                if (extraLife == false)
                    StartCoroutine(SuperSonic()); //si no estábamos ya en modo super, llamamos a la corutina que activa la animación de supersonic
                extraLife = true; //indicamos que estamos en modo supersonic           
                animator.SetBool("SuperSonic", true); //esta variable del animator nos indica que las animaciones van a ser en modo supersonic
                break;
            case "Ground":
                grounded = true; //indicamos que estamos en contacto con el suelo
                animator.SetBool("Jump", false); //desactivamos la animación saltando
                animator.SetBool("Falling", false);
                break;         
            case "Enemy":
                collision.gameObject.transform.localScale =  new Vector3(collision.gameObject.transform.localScale.x, 
                collision.gameObject.transform.localScale.y/3, collision.gameObject.transform.localScale.z); //reducimos el tamáño del enemigo pisado
                collision.gameObject.tag = "Untagged"; //le quitamos el tag de enemigo para que no nos pueda matar 
                collision.gameObject.GetComponent<EnemyGoombasMovement>().speed = 0; //paramos el movimientos del enemigo
                Destroy(collision.gameObject,1); //destruimos el enemigo al cabo de un segundo
                //salto tras pisar enemigo
                rigidBody.velocity = new Vector2(0, 0); //eliminamos las inercias del player
                rigidBody.AddForce(new Vector2(0, jumpForce/4)); //hacemos que revote sobre el enemigo con una cuarta parte de la fuerza de referencia que usamos para el salto
                grounded = true; //nos permite volver a saltar
                gameController.AddPoints(100); //llamamos a la función que suma puntos
                animator.SetBool("Falling", false);            
                break;                           
            case "EnemyFlying":
                collision.gameObject.transform.localScale =  new Vector3(collision.gameObject.transform.localScale.x, 
                collision.gameObject.transform.localScale.y/3, collision.gameObject.transform.localScale.z); //reducimos el tamáño del enemigo pisado
                collision.gameObject.tag = "Untagged"; //le quitamos el tag de enemigo para que no nos pueda matar 
                collision.gameObject.GetComponent<EnemyFlyingMovement>().speed = 0; //paramos el movimientos del enemigo
                Destroy(collision.gameObject,1); //destruimos el enemigo al cvabo de un segundo
                //salto tras pisar enemigo
                rigidBody.velocity = new Vector2(0, 0); //eliminamos las inercias del player
                rigidBody.AddForce(new Vector2(0, jumpForce/4)); //hacemos que revote sobre el enemigo con una cuarta parte de la fuerza de referencia que usamos para el salto
                grounded = true; //nos permite volver a saltar
                gameController.AddPoints(200); //llamamos a la función que suma puntos
                animator.SetBool("Falling", false);            
                break;                 
            case "Flag":      
                savedRespawnPoint = true;
                respawnPosition = collision.transform.position;  
                break;                          
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingMovement : MonoBehaviour
{
    public float speed;
    public float xRange = 5.0f; //Horizontal range for patrol movement
    public float yRange = 1.0f; //Vertical range for patrol movement
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    public bool enemyChasing = false;
    public Player player;
    public float distanceToChase = 5;
    private int direction=1;

    Vector3 posIni;

    void Start(){
        posIni = transform.position;
        rigidBody = GetComponent<Rigidbody2D>(); 
    }

    //Body Colisions
    private void OnCollisionEnter2D(Collision2D collision){
         //si entramos en contacto con un componente colisión que no sea ground
        if (!collision.gameObject.CompareTag("Ground")){
            direction = -direction; // cambiamos  
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    // Update is called once per frame
    void FixedUpdate(){
        if(enemyChasing == false)
            Patrol();
        else
            Chase();
    }

    void Patrol(){
        //Here we define sinusoide movement for patrol status
        float x = Mathf.Sin(Time.time) * xRange* speed; 
        float y = Mathf.Cos(Time.time*4) * yRange * speed;
        Vector3 pos = new Vector3(x, y, 0) + posIni;
        //here we make the enemy look to the direction he is moving
        if (pos.x > transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;  
        rigidBody.MovePosition(pos); 
        //Find distance to player
         distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if(distanceToPlayer < distanceToChase)
            enemyChasing = true;
        
    }

     void Chase(){
        Vector3 pos = Vector3.MoveTowards(transform.position, player.transform.position, 4* speed * Time.deltaTime);
        if (pos.x > transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
        transform.position = pos;
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if(distanceToPlayer > distanceToChase)
            enemyChasing = false;
     }

     //We create this gizmos to make it easer to make a visual of the area where th enemy starts chasing the player
     void OnDrawGizmos()
        {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToChase);
        }

public float distanceToPlayer;
}


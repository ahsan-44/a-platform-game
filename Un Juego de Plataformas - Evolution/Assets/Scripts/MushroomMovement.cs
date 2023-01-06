using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMovement : MonoBehaviour{
    // Start is called before the first frame update
    public float speed = 1000;
    public float xRange = 5.0f; //Rango horizontal
    public float yRange = 1.0f; //Rango vertical
    private Rigidbody2D rigidBody;
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
            direction = -direction;
        }
    }
    // Update is called once per frame
    void FixedUpdate(){
        rigidBody.AddForce(new Vector2 (direction * speed * Time.deltaTime, 0), ForceMode2D.Force);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;

    private int direction=1;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>(); 
    }

    //Body Colisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) //si entramos en contacto con un componente colisión que no sea ground
        {
            direction = -direction; // cambiamos  
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.AddForce(new Vector2 (direction * speed * Time.deltaTime, 0), ForceMode2D.Force);
    }
}

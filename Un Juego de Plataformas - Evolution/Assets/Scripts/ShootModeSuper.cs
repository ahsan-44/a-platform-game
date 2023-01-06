using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootModeSuper : MonoBehaviour
{
    public float speed = 1;
    public float direction = 1;
    public GameObject destroyEffect; 

    void Update()
    {
        transform.Translate(0, -Time.deltaTime*speed*direction,0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
        Instantiate(destroyEffect, transform.position, destroyEffect.transform.rotation);
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyFlying"))
            Destroy(other.gameObject);
    }


    private void OnCollisionEnter2D(Collision2D other){
        Destroy(gameObject);
        Instantiate(destroyEffect, transform.position, destroyEffect.transform.rotation);
    }
}

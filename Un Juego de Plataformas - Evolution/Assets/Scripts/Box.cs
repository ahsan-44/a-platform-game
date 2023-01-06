using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour //usamos la misma clase para todas las cajas
{
    public bool destroyable; // indicamos si es de tipo destruible
    public bool hasCoin; //si contien una moneda al golpearla
    public bool hasMushroom; //si contiene una seta al golpearla
    public GameObject destroyEffect; 
    public GameObject mushroom;
    public GameObject coin;
    public GameObject boxAfter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //si el trigger de la caja que estara en la parte inferior entra en contacto con el player
        {
            if (!destroyable){
                if (UnityEngine.Random.Range(1, 100) < 70) //elegimos al azar si tendra una seta o moneda (esto lo asociaremos a las cajas con interrogante)
                {
                    hasCoin = true;
                    hasMushroom = false;
                }
                else
                {
                    hasMushroom = true;
                    hasCoin = false;
                }
            }

            if (hasCoin) //si contiene una moneda
            {
                coin.transform.parent = null; //sacamos a la moneda del padre
                coin.SetActive(true); //hacemos que se visualice y active
            }

            if (hasMushroom && mushroom != null) // //idem con la seta
            {
                mushroom.transform.parent = null;
                mushroom.SetActive(true);
            }

            if (boxAfter != null) //si la caja no contiene nada
            {
                Destroy(transform.parent.gameObject, 1);
                boxAfter.SetActive(true);
                boxAfter.transform.parent = null;
            }

            if (destroyable && Player.instancePlayer.extraLife) //si es destruible
            {
                Destroy(transform.parent.gameObject);
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                //instanciamos el efecto tras destruir la caja
                Instantiate(destroyEffect, transform.position, destroyEffect.transform.rotation);
            }

        }
    }
}

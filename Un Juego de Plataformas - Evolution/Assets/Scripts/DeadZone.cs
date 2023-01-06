using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public GameController gameController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //GameOver o restart level
            gameController.GameOver();
        }
    }
}

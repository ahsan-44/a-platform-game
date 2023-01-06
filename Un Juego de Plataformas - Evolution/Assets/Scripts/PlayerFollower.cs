using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Transform player;

    private Vector3 offset;

    private void Start()
    {
        offset = player.transform.position - transform.position; // para que siga la camara el movimiento del player
    }

    private void Update()
    {
        //transform.position = player.position - offset;
        transform.position = new Vector3(player.position.x - offset.x, transform.position.y, transform.position.z);
    }
}

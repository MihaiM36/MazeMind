using UnityEngine;

public class EndPoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameTimer timer = FindObjectOfType<GameTimer>();
            if (timer != null)
            {
                timer.PlayerReachedEndPoint();
            }
        }
    }
}
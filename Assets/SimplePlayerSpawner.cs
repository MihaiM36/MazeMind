using UnityEngine;

public class SimplePlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject playerInstance;

    void Start()
    {
        if (playerInstance == null)
        {
            Vector3 startPosition = new Vector3(0, 0.5f, 0); // Adjust y to ensure it's just above the ground
            playerInstance = Instantiate(playerPrefab, startPosition, Quaternion.identity);
            Debug.Log("Player spawn position: " + playerInstance.transform.position);
        }
    }
}
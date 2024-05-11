using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private string playerTag = "Player";

    private Vector3 offset;
    private GameObject player;
    private Transform playerTransform;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found with tag: " + playerTag);
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 targetPosition = playerTransform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
        }
    }
}

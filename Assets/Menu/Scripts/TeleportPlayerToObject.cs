using UnityEngine;
using BNG;

public class TeleportPlayerToObject : MonoBehaviour
{
    public GameObject targetObject;
    public float teleportOffset = 1.0f;
    [Tooltip("Height offset to prevent falling through ground")]
    public float heightOffset = 10.0f;  // Add this new variable

    private PlayerTeleport playerTeleporter;

    void Start()
    {
        // Find the BNG PlayerTeleport component
        playerTeleporter = FindFirstObjectByType<PlayerTeleport>();
        if (playerTeleporter == null)
        {
            Debug.LogError("No PlayerTeleport component found in scene!");
        }
    }

    public void TeleportPlayer()
    {
        if (playerTeleporter != null && targetObject != null)
        {
            // Calculate the target position
            Vector3 teleportPosition = targetObject.transform.position +
                                      targetObject.transform.forward * teleportOffset;

            // Add height offset to prevent falling through ground
            teleportPosition.y += heightOffset;  // Add this line

            // Use BNG's teleport
            playerTeleporter.TeleportPlayer(teleportPosition, targetObject.transform.rotation);
        }
    }
}
using UnityEngine;
using BNG;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("GameObject prefab to spawn")]
    public GameObject objectToSpawn;

    [Tooltip("Transform where objects will spawn. If null, uses this transform")]
    public Transform spawnLocation;

    [Tooltip("Add random offset to spawn position")]
    public bool addRandomOffset = false;

    [Tooltip("Range of random offset")]
    public float randomRange = 0.2f;

    [Header("Velocity Settings")]
    [Tooltip("Apply velocity to spawned objects")]
    public bool addVelocity = false;

    [Tooltip("Direction of velocity (will be normalized)")]
    public Vector3 velocityDirection = Vector3.forward;

    [Tooltip("Speed of the spawned object")]
    public float velocitySpeed = 5f;

    [Tooltip("Use spawn location's forward direction instead of fixed direction")]
    public bool useSpawnDirectionForVelocity = true;

    [Tooltip("Add random variation to velocity")]
    public bool randomizeVelocity = false;

    [Tooltip("Random velocity variation range")]
    public float velocityVariation = 1f;

    [Header("Spawn Limits")]
    [Tooltip("Maximum number of objects that can be spawned. 0 = unlimited")]
    public int maxObjects = 0;

    [Tooltip("Destroy oldest object when limit is reached")]
    public bool destroyOldestWhenLimitReached = true;

    // Track spawned objects
    private System.Collections.Generic.List<GameObject> spawnedObjects = new System.Collections.Generic.List<GameObject>();

    /// <summary>
    /// Spawn an object. Can be called from UI button events.
    /// </summary>
    public void SpawnObject()
    {
        // Check if we've reached the limit
        if (maxObjects > 0 && spawnedObjects.Count >= maxObjects)
        {
            if (destroyOldestWhenLimitReached && spawnedObjects.Count > 0)
            {
                // Remove oldest object
                GameObject oldestObject = spawnedObjects[0];
                spawnedObjects.RemoveAt(0);
                Destroy(oldestObject);
            }
            else
            {
                // Don't spawn if at limit
                Debug.Log("Object limit reached. Not spawning new object.");
                return;
            }
        }

        // Check if we have a prefab to spawn
        if (objectToSpawn == null)
        {
            Debug.LogWarning("No object assigned to spawn!");
            return;
        }

        // Determine spawn position
        Vector3 position;
        Quaternion rotation;

        if (spawnLocation != null)
        {
            position = spawnLocation.position;
            rotation = spawnLocation.rotation;
        }
        else
        {
            position = transform.position;
            rotation = transform.rotation;
        }

        // Add random offset if enabled
        if (addRandomOffset)
        {
            position += new Vector3(
                Random.Range(-randomRange, randomRange),
                Random.Range(-randomRange, randomRange),
                Random.Range(-randomRange, randomRange)
            );
        }

        // Instantiate the object
        GameObject newObject = Instantiate(objectToSpawn, position, rotation);

        // Apply velocity if enabled
        if (addVelocity)
        {
            Rigidbody rb = newObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Get the velocity direction
                Vector3 direction;

                if (useSpawnDirectionForVelocity)
                {
                    // Use the spawn location's forward direction
                    direction = (spawnLocation != null) ? spawnLocation.forward : transform.forward;
                }
                else
                {
                    // Use the configured direction
                    direction = velocityDirection.normalized;
                }

                // Calculate final velocity
                float speed = velocitySpeed;

                // Add randomization if enabled
                if (randomizeVelocity)
                {
                    speed += Random.Range(-velocityVariation, velocityVariation);

                    // Add slight random direction variation
                    direction += new Vector3(
                        Random.Range(-0.1f, 0.1f),
                        Random.Range(-0.1f, 0.1f),
                        Random.Range(-0.1f, 0.1f)
                    );
                    direction.Normalize();
                }

                // Apply velocity
                rb.linearVelocity = direction * speed;

                Debug.Log($"Applied velocity: {rb.linearVelocity} to spawned object");
            }
            else
            {
                Debug.LogWarning("Object has no Rigidbody, cannot apply velocity");
            }
        }

        // Add to our list of spawned objects
        spawnedObjects.Add(newObject);

        Debug.Log($"Spawned {objectToSpawn.name} at {position}");
    }

    /// <summary>
    /// Destroys all spawned objects
    /// </summary>
    public void ClearAllSpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        spawnedObjects.Clear();
        Debug.Log("Cleared all spawned objects");
    }
}
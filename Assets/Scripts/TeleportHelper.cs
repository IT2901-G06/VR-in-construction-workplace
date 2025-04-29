using UnityEngine;

public class TeleportHelper : MonoBehaviour
{

    public static TeleportHelper Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
    }

    public void Teleport(Vector3 targetPosition)
    {
    }
}

using UnityEngine;
using UnityEngine.Events;

public class ProximityTrigger : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Values")]
    public float triggerRadius = 5f;
    public bool canTriggerOnlyOnce = true;

    [Header("Events")]
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private bool _hasTriggered = false;
    private bool _hasExited = false;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (canTriggerOnlyOnce && _hasTriggered && _hasExited)
        {
            return;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= triggerRadius && !_hasTriggered)
        {
            _hasTriggered = true;
            OnEnter?.Invoke();
            return;
        }

        if (distance > triggerRadius && _hasTriggered && !_hasExited)
        {
            if (!canTriggerOnlyOnce)
            {
                _hasTriggered = false;
            }

            _hasExited = true;
            OnExit?.Invoke();
            return;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}

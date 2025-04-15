using UnityEngine;
using UnityEngine.Events;

public class ProximityTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _player;

    [Header("Values")]
    [SerializeField] private float _triggerRadius = 5f;
    [SerializeField] private bool _canTriggerOnlyOnce = true;

    [Header("Events")]
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private bool _hasTriggered = false;
    private bool _hasExited = false;

    void Start()
    {
        if (_player == null)
            _player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (_canTriggerOnlyOnce && _hasTriggered && _hasExited)
        {
            return;
        }

        float distance = Vector3.Distance(_player.position, transform.position);
        if (distance <= _triggerRadius && !_hasTriggered)
        {
            _hasTriggered = true;
            OnEnter?.Invoke();
            return;
        }

        if (distance > _triggerRadius && _hasTriggered && !_hasExited)
        {
            if (!_canTriggerOnlyOnce)
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
        Gizmos.DrawWireSphere(transform.position, _triggerRadius);
    }
}

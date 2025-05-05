using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class triggers events when the player enters or exits a specified radius.
/// </summary>
public class ProximityTrigger : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The player transform. If not set, an attempt will be made to find it by its tag.")]
    [SerializeField] private Transform _player;

    [Header("Values")]
    [Tooltip("The radius within which the trigger will be activated.")]
    [SerializeField] private float _triggerRadius = 5f;

    [Tooltip("If true, the trigger will only be activated once. If false, it can be triggered multiple times.")]
    [SerializeField] private bool _canTriggerOnlyOnce = true;

    [Header("Events")]
    [Tooltip("Event triggered when the player enters the trigger radius.")]
    public UnityEvent OnEnter;

    [Tooltip("Event triggered when the player exits the trigger radius.")]
    public UnityEvent OnExit;

    private bool _hasTriggered = false;
    private bool _hasExited = false;
    private float _startTime = 0;

    void Start()
    {
        if (_player == null)
            _player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        // Return if the player is inside the proximity during the first second of the
        // scene. This is to avoid triggering the event in the split second that the player
        // may be teleported away or to a point within the proximity.
        _startTime += Time.deltaTime;
        if (_startTime < 1)
        {
            return;
        }

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

        // Reset the trigger if the player is outside the radius and the trigger has
        // previously been triggered.
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
        // Draw a wire sphere to visualize the trigger radius in the editor.
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _triggerRadius);
    }
}

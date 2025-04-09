using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointWalker : MonoBehaviour
{
    private List<Vector3> Waypoints;
    public float Threshold = 0.5f;
    public UnityEngine.Events.UnityEvent OnFinalDestinationReached;
    

    private NavMeshAgent _agent;
    private Animator _animator;
    private int _velocityYHash;
    private int _currentIndex = 0;
    private int _direction = 1;
    private bool _initialized = false;
    private bool _shouldWalkWaypointsInCircle = true;
    private bool _enabled = true;


    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _velocityYHash = Animator.StringToHash("VelocityY");
    }

    void Update()
    {
        if (!_initialized || _agent == null || Waypoints == null || Waypoints.Count == 0 || !_enabled) return;

        if (_animator != null)
        {
            _animator.SetFloat(_velocityYHash, _agent.velocity.magnitude);
        }

        if (_agent.remainingDistance <= Threshold && !_agent.pathPending)
        {
            _currentIndex += _direction;

            if (_currentIndex >= Waypoints.Count || _currentIndex < 0)
            {
                if (_shouldWalkWaypointsInCircle)
                {
                    _direction *= -1;
                    _currentIndex += _direction * 2;
                }
                else
                {
                    _enabled = false;
                    OnFinalDestinationReached?.Invoke();
                    _animator?.SetFloat(_velocityYHash, 0f);
                }
            }

            if (_enabled)
            {
                _agent.SetDestination(Waypoints[_currentIndex]);
            }

            AnimationConstraintsController visualFix = GetComponentInChildren<AnimationConstraintsController>();
            if (visualFix != null)
            {
                visualFix.SnapToLocalOrigin();
            }
        }
    }

    public void SetWaypoints(List<Vector3> waypointPositions)
    {
        Waypoints = waypointPositions;
    }

    public void SetShouldWalkWaypointsInCircle(bool shouldWalkWaypointsInCircle)
    {
        _shouldWalkWaypointsInCircle = shouldWalkWaypointsInCircle;
    }

    public void UpdateAnimator(Animator animator)
    {
        _animator = animator;
        _initialized = true;

        if (Waypoints != null && Waypoints.Count > 0 && _agent != null)
        {
            _agent.SetDestination(Waypoints[_currentIndex]);
        }
    }

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointWalker : MonoBehaviour
{
    private List<Vector3> Waypoints;
    public float Threshold = 0.5f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private int _velocityYHash;
    private int _currentIndex = 0;
    private int _direction = 1;
    private bool _initialized = false;


    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _velocityYHash = Animator.StringToHash("VelocityY");
    }

    void Update()
    {
        if (!_initialized || _agent == null || Waypoints == null || Waypoints.Count == 0) return;

        if (_animator != null)
        {
            _animator.SetFloat(_velocityYHash, _agent.velocity.magnitude);
        }

        if (_agent.remainingDistance <= Threshold && !_agent.pathPending)
        {
            _currentIndex += _direction;

            if (_currentIndex >= Waypoints.Count || _currentIndex < 0)
            {
                _direction *= -1;
                _currentIndex += _direction * 2;
            }

            _agent.SetDestination(Waypoints[_currentIndex]);
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ElectricityManager : MonoBehaviour
{
    [Header("Electricity Settings")]
    [SerializeField] private bool _requiresBothHands = true;
    [SerializeField] private float _secondsBetweenElectricitySteps = 0.1f;
    [Range(1, 100)]
    [SerializeField]
    private int _motorStrength = 100;

    [Header("Events")]
    [SerializeField] private UnityEvent _onElectricityStarting;
    [SerializeField] private UnityEvent _onElectricityStarted;
    [SerializeField] private UnityEvent _onElectricityStopping;
    [SerializeField] private UnityEvent _onElectricityStopped;

    private bool _electricityIsOn = false;

    private readonly List<int> _bhapticsRequestIds = new();
    private Coroutine _stopElectricityCoroutine;

    private Transform _leftHandGrabbed;
    private Transform _rightHandGrabbed;

    private bool IsLeftHandSelected()
    {
        return _leftHandGrabbed != null;
    }
    private bool IsRightHandSelected()
    {
        return _rightHandGrabbed != null;
    }
    private bool HasGrabbedTwoOfSameDirection()
    {
        return _leftHandGrabbed != null && _leftHandGrabbed.tag.Length > 0 && _leftHandGrabbed.CompareTag(_rightHandGrabbed.tag);
    }
    private bool IsRightToLeft()
    {
        return IsElectricityFromSource(_rightHandGrabbed) && !IsElectricityFromSource(_leftHandGrabbed);
    }

    public void OnGrabFromChild(Transform leverGrabbed, bool isLeftHand)
    {
        if (isLeftHand)
        {
            _leftHandGrabbed = leverGrabbed;
            Debug.Log("Left hand grabbed");
        }
        else
        {
            _rightHandGrabbed = leverGrabbed;
            Debug.Log("Right hand grabbed");
        }

        if (_electricityIsOn) return;

        if (!_requiresBothHands)
        {
            StartCoroutine(StartElectricitySequence(isLeftHand));
        }
        else
        {
            if (IsLeftHandSelected() && IsRightHandSelected() && !HasGrabbedTwoOfSameDirection())
            {
                StartCoroutine(StartElectricitySequence(!IsRightToLeft()));
            }
        }
    }

    public void OnReleaseFromChild(Transform leverReleased)
    {
        if (_leftHandGrabbed == leverReleased)
        {
            _leftHandGrabbed = null;
            Debug.Log("Left hand released");
        }
        else if (_rightHandGrabbed == leverReleased)
        {
            _rightHandGrabbed = null;
            Debug.Log("Right hand released");
        }

        if (_electricityIsOn)
        {
            _stopElectricityCoroutine = StartCoroutine(StopElectricitySequence());
        }
    }

    private bool IsElectricityFromSource(Transform source)
    {
        return source.CompareTag("ElectricitySourceFrom");
    }

    private IEnumerator KillAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        DeathManager.Instance.Kill();
        Debug.Log("Player killed after delay");
    }

    private IEnumerator StartElectricitySequence(bool reverse = false)
    {
        HapticController hapticController = HapticController.Instance;

        Debug.Log("Electricity starting!");
        _onElectricityStarting?.Invoke();
        _electricityIsOn = true;

        MotorEvent[] events = reverse ? ElectricityEventSequence.EventSteps.Reverse().ToArray() : ElectricityEventSequence.EventSteps;
        Debug.Log("walla " + events.Count() + "");

        foreach (MotorEvent motorEvent in events)
        {
            // If the stop has already started, we need to break out of this loop as the stop loop will
            // not be able to stop requests that hasn't already started.
            if (_stopElectricityCoroutine != null) break;

            int requestId = hapticController.RunMotors(motorEvent, _motorStrength, 99999999);
            _bhapticsRequestIds.Add(requestId);
            yield return new WaitForSeconds(_secondsBetweenElectricitySteps);
        }

        Debug.Log("Electricity on!");
        _onElectricityStarted?.Invoke();

        StartCoroutine(KillAfterDelay(0.5f));
    }

    private IEnumerator StopElectricitySequence()
    {
        HapticController hapticController = HapticController.Instance;

        Debug.Log("Electricity off is starting!");
        _onElectricityStopping?.Invoke();

        var copiedRequestIds = new int[_bhapticsRequestIds.Count];
        _bhapticsRequestIds.CopyTo(copiedRequestIds);
        _bhapticsRequestIds.Clear();
        Debug.Log("wallah " + copiedRequestIds.Count() + "");
        _electricityIsOn = false;

        foreach (int requestId in copiedRequestIds)
        {
            hapticController.StopByRequestId(requestId);
            yield return new WaitForSeconds(_secondsBetweenElectricitySteps);
        }

        _electricityIsOn = false;
        _stopElectricityCoroutine = null;
        Debug.Log("Electricity is off!");
        _onElectricityStopped?.Invoke();
    }
}

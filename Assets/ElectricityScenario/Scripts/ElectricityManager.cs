using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ElectricityManager : MonoBehaviour
{
    [Header("Electricity Settings")]
    [SerializeField] private bool requiresBothHands = true;
    [SerializeField] private float secondsBetweenElectricitySteps = 0.1f;
    [Range(1, 100)]
    [SerializeField]
    private int motorStrength = 100;

    [Header("Events")]
    [SerializeField] private UnityEvent onElectricityStarting;
    [SerializeField] private UnityEvent onElectricityStarted;
    [SerializeField] private UnityEvent onElectricityStopping;
    [SerializeField] private UnityEvent onElectricityStopped;

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

        if (!requiresBothHands)
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
        onElectricityStarting?.Invoke();
        _electricityIsOn = true;

        MotorEvent[] events = reverse ? ElectricityEventSequence.EventSteps.Reverse().ToArray() : ElectricityEventSequence.EventSteps;

        foreach (MotorEvent motorEvent in events)
        {
            // If the stop has already started, we need to break out of this loop as the stop loop will
            // not be able to stop requests that hasn't already started.
            if (_stopElectricityCoroutine != null) break;

            int requestId = hapticController.RunMotors(motorEvent, motorStrength, 99999999);
            _bhapticsRequestIds.Add(requestId);
            yield return new WaitForSeconds(secondsBetweenElectricitySteps);
        }

        Debug.Log("Electricity on!");
        onElectricityStarted?.Invoke();

        StartCoroutine(KillAfterDelay(0.5f));
    }

    private IEnumerator StopElectricitySequence()
    {
        HapticController hapticController = HapticController.Instance;

        Debug.Log("Electricity off is starting!");
        onElectricityStopping?.Invoke();

        var copiedRequestIds = new int[_bhapticsRequestIds.Count];
        _bhapticsRequestIds.CopyTo(copiedRequestIds);
        _bhapticsRequestIds.Clear();

        foreach (int requestId in copiedRequestIds)
        {
            hapticController.StopByRequestId(requestId);
            yield return new WaitForSeconds(secondsBetweenElectricitySteps);
        }

        _electricityIsOn = false;
        _stopElectricityCoroutine = null;
        Debug.Log("Electricity is off!");
        onElectricityStopped?.Invoke();
    }
}

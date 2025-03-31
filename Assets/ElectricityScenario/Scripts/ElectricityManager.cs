using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BNG;
using UnityEngine;

public class ElectricityManager : MonoBehaviour
{
    [SerializeField] private bool requiresBothHands = true;
    [SerializeField] private float secondsBetweenElectricitySteps = 0.1f;
    [Range(1, 100)]
    [SerializeField]
    private int motorStrength = 100;

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

    internal void OnGrabFromChild(Transform child, Grabber grabber)
    {
        if (grabber.CompareTag("LeftHandGrabber"))
        {
            _leftHandGrabbed = child;
            Debug.Log("Left hand grabbed");
        }
        else if (grabber.CompareTag("RightHandGrabber"))
        {
            _rightHandGrabbed = child;
            Debug.Log("Right hand grabbed");
        }

        if (_electricityIsOn) return;

        bool isLeftHand = grabber.CompareTag("LeftHandGrabber");

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

    internal void OnReleaseFromChild(Transform leverReleased)
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

    private IEnumerator StartElectricitySequence(bool reverse = false)
    {
        HapticController hapticController = HapticController.Instance;

        Debug.Log("Electricity starting!");
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
    }

    private IEnumerator StopElectricitySequence()
    {
        HapticController hapticController = HapticController.Instance;

        Debug.Log("Electricity off is starting!");

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
    }
}

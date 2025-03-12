using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private Transform _leftHandCollidingChild;
    private Transform _rightHandCollidingChild;

    private bool IsLeftHandSelected()
    {
        return _leftHandCollidingChild != null;
    }
    private bool IsRightHandSelected()
    {
        return _rightHandCollidingChild != null;
    }
    private bool HasGrabbedTwoOfSameDirection()
    {
        return _leftHandCollidingChild != null && _leftHandCollidingChild.tag.Length > 0 && _leftHandCollidingChild.CompareTag(_rightHandCollidingChild.tag);
    }
    private bool IsRightToLeft()
    {
        return IsElectricityFromSource(_rightHandCollidingChild) && !IsElectricityFromSource(_leftHandCollidingChild);
    }

    internal void OnTriggerEnterFromChild(Transform child, Collider collider)
    {
        // Find out which hand is poking
        if (collider.CompareTag("LeftHandIndexFingerTip"))
        {
            _leftHandCollidingChild = child;
            Debug.Log("Left hand poking");
        }
        else if (collider.CompareTag("RightHandIndexFingerTip"))
        {
            _rightHandCollidingChild = child;
            Debug.Log("Right hand poking");
        }
        else
        {
            return;
        }

        if (_electricityIsOn) return;

        bool isLeftHand = collider.CompareTag("LeftHandIndexFingerTip");

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

    internal void OnTriggerExitFromChild(Transform _, Collider collider)
    {
        // Find out which hand is poking
        if (collider.CompareTag("LeftHandIndexFingerTip"))
        {
            _leftHandCollidingChild = null;
            Debug.Log("Left hand stopped poking");
        }
        else if (collider.CompareTag("RightHandIndexFingerTip"))
        {
            _rightHandCollidingChild = null;
            Debug.Log("Right hand stopped poking");
        }
        else
        {
            return;
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

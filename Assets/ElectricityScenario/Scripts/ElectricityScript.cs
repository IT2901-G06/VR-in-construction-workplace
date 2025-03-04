using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bhaptics.SDK2;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ElectricityScript : MonoBehaviour
{
    [SerializeField] private bool requiresBothHands = true;
    [SerializeField] private float secondsBetweenElectricitySteps = 0.1f;
    [Range(1, 100)]
    [SerializeField]
    private int motorStrength = 100;

    private bool _electricityIsOn = false;

    private readonly List<int> _bhapticsRequestIds = new();
    private Coroutine _startElectricityCoroutine;
    private Coroutine _stopElectricityCoroutine;

    private Transform _leftHandCollidingChild;
    private Transform _rightHandCollidingChild;

    private bool _leftHandSelected => _leftHandCollidingChild != null;
    private bool _rightHandSelected => _rightHandCollidingChild != null;
    private bool _hasGrabbedTwoOfSameDirection => _leftHandCollidingChild?.tag.Length > 0 && _leftHandCollidingChild?.tag == _rightHandCollidingChild?.tag;
    private bool _rightToLeft => IsElectricityFromSource(_rightHandCollidingChild) && !IsElectricityFromSource(_leftHandCollidingChild);

    // Start is called once before the first execution of Update after the MonoBehaviour is created


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
        else {
            return;
        }

        if (_electricityIsOn) return;

        bool isLeftHand = collider.CompareTag("LeftHandIndexFingerTip");

        if (!requiresBothHands)
        {
            _startElectricityCoroutine = StartCoroutine(StartElectricity(isLeftHand));
        }
        else {
            if (_leftHandSelected && _rightHandSelected && !_hasGrabbedTwoOfSameDirection)
            {
                _startElectricityCoroutine = StartCoroutine(StartElectricity(!_rightToLeft));
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
        else {
            return;
        }

        bool isLeftHand = collider.CompareTag("LeftHandIndexFingerTip");

        if (_electricityIsOn)
        {
            _stopElectricityCoroutine = StartCoroutine(StopElectricity());
        }
    }

    private bool IsElectricityFromSource(Transform source)
    {
        return source.CompareTag("ElectricitySourceFrom");
    }

    private int[] MagnifyMotorStrengths(int[] motorValues, int magnificationFactor)
    {
        int[] newMotorValues = new int[motorValues.Length];
        for (int i = 0; i < motorValues.Length; i++)
        {
            newMotorValues[i] = motorValues[i] * magnificationFactor;
        }
        return newMotorValues;
    }

    private IEnumerator StartElectricity(bool reverse = false)
    {
        Debug.Log("Electricity starting!");      
        _electricityIsOn = true;

        MotorEvent[] events = reverse ? ElectricityEvent.EventSteps.Reverse().ToArray() : ElectricityEvent.EventSteps;

        foreach (MotorEvent motorEvent in events)
        {
            // If the stop has already started, we need to break out of this loop as the stop loop will
            // not be able to stop requests that hasn't already started.
            if (_stopElectricityCoroutine != null) break;

            int requestId = BhapticsLibrary.PlayMotors((int)motorEvent.PositionType, MagnifyMotorStrengths(motorEvent.MotorValues, motorStrength), 99999999);
            _bhapticsRequestIds.Add(requestId);
            yield return new WaitForSeconds(secondsBetweenElectricitySteps);
        }

        _startElectricityCoroutine = null;
        Debug.Log("Electricity on!");      
    }

    private IEnumerator StopElectricity()
    {
        Debug.Log("Electricity off is starting!");

        var copiedRequestIds = new int[_bhapticsRequestIds.Count];
        _bhapticsRequestIds.CopyTo(copiedRequestIds);
        _bhapticsRequestIds.Clear();

        foreach (int requestId in copiedRequestIds)
        {
            BhapticsLibrary.StopInt(requestId);
            yield return new WaitForSeconds(secondsBetweenElectricitySteps);
        }

        _electricityIsOn = false;
        _stopElectricityCoroutine = null;
        Debug.Log("Electricity is off!");
    }

    // private void OnSelectedEnter(SelectEnterEventArgs args)
    // {
    //     Debug.Log("ElectricityScript.OnSelectedEnter");
    //     bool isLeftHand = args.interactorObject.handedness == InteractorHandedness.Left;

    //     if (isLeftHand)
    //     {
    //         _leftHandSelectInteractable = args.interactableObject;
    //     }
    //     else
    //     {
    //         _rightHandSelectInteractable = args.interactableObject;
    //     }

    //     if (_electricityIsOn) return;

    //     if (!requiresBothHands)
    //     {
    //         _startElectricityCoroutine = StartCoroutine(StartElectricity(isLeftHand));
    //     }
    //     else {
    //         if (_leftHandSelected && _rightHandSelected && !_hasGrabbedTwoOfSameDirection)
    //         {
    //             _startElectricityCoroutine = StartCoroutine(StartElectricity(!_rightToLeft));
    //         }
    //     }
    // }

    // private void OnSelectedExit(SelectExitEventArgs args)
    // {
    //     Debug.Log("ElectricityScript.OnSelectedExit");

    //     bool isLeftHand = args.interactorObject.handedness == InteractorHandedness.Left;

    //     if (isLeftHand)
    //     {
    //         _leftHandSelectInteractable = null;
    //     }
    //     else
    //     {
    //         _rightHandSelectInteractable = null;
    //     }

    //     if (_electricityIsOn)
    //     {
    //         _stopElectricityCoroutine = StartCoroutine(StopElectricity());
    //     }
    // }
}

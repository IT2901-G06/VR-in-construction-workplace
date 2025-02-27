using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bhaptics.SDK2;
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
    private IXRHoverInteractable _leftHandHoverInteractable;
    private IXRHoverInteractable _rightHandHoverInteractable;
    private readonly List<int> _bhapticsRequestIds = new();
    private Coroutine _startElectricityCoroutine;
    private Coroutine _stopElectricityCoroutine;

    private bool _leftHandHovered => _leftHandHoverInteractable != null;
    private bool _rightHandHovered => _rightHandHoverInteractable != null;
    private bool _hasGrabbedTwoOfSameDirection => _leftHandHoverInteractable?.transform.tag.Length > 0 && _leftHandHoverInteractable?.transform.tag == _rightHandHoverInteractable?.transform.tag;
    private bool _rightToLeft => isElectricityFromSource(_rightHandHoverInteractable) && !isElectricityFromSource(_leftHandHoverInteractable);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Register the event handlers
        for (int i = 0; i < transform.childCount; i++)
        {
            XRGrabInteractable childGrabInteractable = transform.GetChild(i).GetComponent<XRGrabInteractable>();
            childGrabInteractable.hoverEntered.AddListener(OnHoveredEnter);
            childGrabInteractable.hoverExited.AddListener(OnHoveredExit);
        }
    }

    private bool isElectricityFromSource(IXRHoverInteractable interactable)
    {
        return interactable.transform.tag == "ElectricitySourceFrom";
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

    private void OnHoveredEnter(HoverEnterEventArgs args)
    {
        Debug.Log("ElectricityScript.OnHoveredEnter");
        bool isLeftHand = args.interactorObject.handedness == InteractorHandedness.Left;

        if (isLeftHand)
        {
            _leftHandHoverInteractable = args.interactableObject;
        }
        else
        {
            _rightHandHoverInteractable = args.interactableObject;
        }

        if (_electricityIsOn) return;

        if (!requiresBothHands)
        {
            _startElectricityCoroutine = StartCoroutine(StartElectricity(isLeftHand));
        }
        else {
            if (_leftHandHovered && _rightHandHovered && !_hasGrabbedTwoOfSameDirection)
            {
                _startElectricityCoroutine = StartCoroutine(StartElectricity(!_rightToLeft));
            }
        }
    }

    private void OnHoveredExit(HoverExitEventArgs args)
    {
        Debug.Log("ElectricityScript.OnHoveredExit");

        bool isLeftHand = args.interactorObject.handedness == InteractorHandedness.Left;

        if (isLeftHand)
        {
            _leftHandHoverInteractable = null;
        }
        else
        {
            _rightHandHoverInteractable = null;
        }

        if (_electricityIsOn)
        {
            _stopElectricityCoroutine = StartCoroutine(StopElectricity());
        }
    }
}

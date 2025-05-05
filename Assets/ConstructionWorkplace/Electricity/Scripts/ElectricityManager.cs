using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ElectricityManager : MonoBehaviour
{
    [Header("Electricity Settings")]

    [SerializeField]
    [Tooltip("Whether or not both hands are required to start the electricity.")]
    private bool _requiresBothHands = true;

    [SerializeField]
    [Tooltip("The delay between each electricity step in seconds.")]
    private float _secondsBetweenElectricitySteps = 0.2f;

    [Range(1, 100)]
    [SerializeField]
    [Tooltip("The strength of the motors. 1-100.")]
    private int _motorStrength = 100;

    [Header("Events")]
    [SerializeField]
    [Tooltip("Event triggered when the electricity is starting.")]
    private UnityEvent _onElectricityStarting;

    [SerializeField]
    [Tooltip("Event triggered when the electricity is started.")]
    private UnityEvent _onElectricityStarted;

    [SerializeField]
    [Tooltip("Event triggered when the electricity is stopping.")]
    private UnityEvent _onElectricityStopping;

    [SerializeField]
    [Tooltip("Event triggered when the electricity is stopped.")]
    private UnityEvent _onElectricityStopped;

    // Protected so that test mocks can access it.
    protected bool _electricityIsOn = false;

    private readonly List<int> _bhapticsRequestIds = new();
    private Coroutine _stopElectricityCoroutine;

    private Transform _leftHandPressed;
    private Transform _rightHandPressed;

    /// <summary>
    /// Sets whether or not both hands are required to start the electricity.
    /// </summary>
    /// <param name="requiresBothHands">Whether or not both hands are required to start electricity.</param>
    public void SetRequiresBothHands(bool requiresBothHands)
    {
        _requiresBothHands = requiresBothHands;
    }

    /// <summary>
    /// Whether the electricity is on or off.
    /// </summary>
    /// <returns>True if the electricity is on, false otherwise.</returns>
    public bool IsElectricityOn()
    {
        return _electricityIsOn;
    }

    /// <summary>
    /// Whether the left hand is pressed.
    /// </summary>
    /// <returns>True if the left hand is pressed, false otherwise.</returns>
    public bool IsLeftHandSelected()
    {
        return _leftHandPressed != null;
    }

    /// <summary>
    /// Whether the right hand is pressed.
    /// </summary>
    /// <returns>True if the right hand is pressed, false otherwise.</returns>
    public bool IsRightHandSelected()
    {
        return _rightHandPressed != null;
    }

    /// <summary>
    /// Whether two elements are pressed in the same electricity direction.
    /// </summary>
    /// <returns>True if two elements are pressed in the same electricity direction, false otherwise.</returns>
    public bool HasPressedTwoOfSameDirection()
    {
        return _leftHandPressed != null && _leftHandPressed.tag.Length > 0 && _leftHandPressed.CompareTag(_rightHandPressed.tag);
    }

    /// <summary>
    /// Whether the electricity is coming from the right hand and travels to the left hand.
    /// </summary>
    /// <returns>True if the electricity is coming from the right hand and travels to the left hand, false otherwise.</returns>
    public bool IsRightToLeft()
    {
        return IsElectricityFromSource(_rightHandPressed) && !IsElectricityFromSource(_leftHandPressed);
    }

    /// <summary>
    /// Called when the electricity is pressed from a child object.
    /// </summary>
    /// <param name="buttonPressed">The button that was pressed.</param>
    /// <param name="isLeftHand">True if the left hand was used, false if the right hand was used.</param>
    public void OnPressFromChild(Transform buttonPressed, bool isLeftHand)
    {
        if (isLeftHand)
        {
            _leftHandPressed = buttonPressed;
            Debug.Log("Left hand pressed");
        }
        else
        {
            _rightHandPressed = buttonPressed;
            Debug.Log("Right hand pressed");
        }

        // No need to start the electricity if it's already on.
        if (_electricityIsOn) return;

        if (!_requiresBothHands)
        {
            // Always start the electricity from the left hand if both hands are not required.
            StartCoroutine(StartElectricitySequence(isLeftHand));
        }
        else
        {
            if (IsLeftHandSelected() && IsRightHandSelected() && !HasPressedTwoOfSameDirection())
            {
                StartCoroutine(StartElectricitySequence(!IsRightToLeft()));
            }
        }
    }

    /// <summary>
    /// Called when the electricity is released from a child object.
    /// </summary>
    /// <param name="buttonReleased">The button that was released.</param>
    public void OnReleaseFromChild(Transform buttonReleased)
    {
        if (_leftHandPressed == buttonReleased)
        {
            _leftHandPressed = null;
            Debug.Log("Left hand released");
        }
        else if (_rightHandPressed == buttonReleased)
        {
            _rightHandPressed = null;
            Debug.Log("Right hand released");
        }

        if (_electricityIsOn)
        {
            _stopElectricityCoroutine = StartCoroutine(StopElectricitySequence());
        }
    }

    /// <summary>
    /// Checks if a source is a "from" electricity source.
    /// </summary>
    /// <param name="source">The source to check.</param>
    /// <returns>True if the source is a "from" electricity source, false otherwise.</returns>
    private bool IsElectricityFromSource(Transform source)
    {
        // One can assume that the source is a "to" electricity source if it is not a "from"
        // electricity source.
        return source.CompareTag("ElectricitySourceFrom");
    }

    /// <summary>
    /// Kills the player after a delay.
    /// </summary>
    /// <param name="delaySeconds">The delay in seconds before killing the player.</param>
    /// <returns>An IEnumerator for coroutine.</returns>
    private IEnumerator KillAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        DeathManager.Instance.Kill();
        Debug.Log("Player killed after delay");
    }

    /// <summary>
    /// Starts the electricity sequence.
    /// </summary>
    /// <param name="reverse">Whether to reverse the electricity sequence.</param>
    /// <returns>An IEnumerator for coroutine.</returns>
    public virtual IEnumerator StartElectricitySequence(bool reverse = false)
    {
        HapticManager hapticManager = HapticManager.Instance;

        Debug.Log("Electricity starting!");
        _onElectricityStarting?.Invoke();
        _electricityIsOn = true;

        MotorEvent[] events = reverse ? ElectricityEventSequence.EventSteps.Reverse().ToArray() : ElectricityEventSequence.EventSteps;

        foreach (MotorEvent motorEvent in events)
        {
            // If the stop has already started, we need to break out of this loop as the stop loop will
            // not be able to stop requests that hasn't already started.
            if (_stopElectricityCoroutine != null) break;

            // Just set the duration to a very high number so that the electricity will not stop. We will
            // stop it manually later.
            int requestId = hapticManager.RunMotors(motorEvent, _motorStrength, 99999999);

            // Save the request ID so that we can stop it later in the correct order.
            _bhapticsRequestIds.Add(requestId);
            yield return new WaitForSeconds(_secondsBetweenElectricitySteps);
        }

        Debug.Log("Electricity on!");
        _onElectricityStarted?.Invoke();

        StartCoroutine(KillAfterDelay(0.5f));
    }

    /// <summary>
    /// Stops the electricity sequence.
    /// </summary>
    /// <returns>An IEnumerator for coroutine.</returns>
    public virtual IEnumerator StopElectricitySequence()
    {
        // Wait for a bit before stopping the electricity. This is to make sure that the player
        // gets to experience the electricity a bit before it stops.
        yield return new WaitForSeconds(4f);

        HapticManager hapticManager = HapticManager.Instance;

        Debug.Log("Electricity off is starting!");
        _onElectricityStopping?.Invoke();

        var copiedRequestIds = new int[_bhapticsRequestIds.Count];
        _bhapticsRequestIds.CopyTo(copiedRequestIds);
        _bhapticsRequestIds.Clear();

        _electricityIsOn = false;

        // Stop the electricity in the order that it was started.
        foreach (int requestId in copiedRequestIds)
        {
            hapticManager.StopByRequestId(requestId);
            yield return new WaitForSeconds(_secondsBetweenElectricitySteps);
        }

        _electricityIsOn = false;
        _stopElectricityCoroutine = null;
        
        Debug.Log("Electricity is off!");
        _onElectricityStopped?.Invoke();
    }
}

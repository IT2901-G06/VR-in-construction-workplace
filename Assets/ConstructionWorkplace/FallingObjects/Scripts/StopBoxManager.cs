using UnityEngine;

/// <summary>
/// This class manages the stop box for the crane and player interactions.
/// It handles the state of the crane, the rope attached, and the player's proximity.
/// It also manages the snap zones and the spools floor.
/// </summary>
public class StopBoxManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the BoxCollider for the spools floor")]
    private BoxCollider _spoolsFloor;

    [SerializeField]
    [Tooltip("Reference to the initial snap zones for stacked items")]
    private GameObject _initialSnapZones;

    [SerializeField]
    [Tooltip("Reference to the secondary snap zones for stacked items")]
    private GameObject _secondarySnapZones;

    private bool _craneAtTop = false;
    private string _ropeAttached = "";
    private bool _withinRange = false;

    /// <summary>
    /// Sets the _ropeAttached state to "Bad" or "Good" based on the input.
    /// </summary>
    /// <param name="badRope">True if the rope is bad, false if it's good.</param>
    public void SetRopeAttached(bool badRope)
    {
        _ropeAttached = badRope ? "Bad" : "Good";
        Debug.Log(_ropeAttached + " rope attached!");
    }

    /// <summary>
    /// Gets the current state of the rope attached.
    /// </summary>
    /// <returns>The state of the rope attached as a string.</returns>
    public string GetRopeAttached()
    {
        return _ropeAttached;
    }

    /// <summary>
    /// Sets the crane state to either at the top or not.
    /// </summary>
    /// <param name="craneAtTop">True if the crane is at the top, false otherwise.</param>
    public void SetCraneAtTop(bool craneAtTop)
    {
        if (craneAtTop) Debug.Log("Crane at top!");
        _craneAtTop = craneAtTop;
    }

    /// <summary>
    /// Sets the player's proximity state.
    /// </summary>
    /// <param name="withinRange">True if the player is within range, false otherwise.</param>
    public void SetWithinRange(bool withinRange)
    {
        _withinRange = withinRange;
    }

    /// <summary>
    /// Handler for when the player looks up.
    /// This method checks the conditions for releasing crates and triggers the release if all conditions are met.
    /// </summary>
    public virtual void LookedUp()
    {
        Debug.Log("Looked up! | Crane at top: " + _craneAtTop + " | Bad rope attached: " + (_ropeAttached == "Bad") + " | Player within range: " + _withinRange);

        // Release crates if all conditions met
        if (_craneAtTop && _ropeAttached == "Bad" && _withinRange)
        {
            _initialSnapZones.SetActive(false);
            _secondarySnapZones.SetActive(false);
            _spoolsFloor.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}

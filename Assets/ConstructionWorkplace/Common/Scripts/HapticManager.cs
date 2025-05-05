using Bhaptics.SDK2;
using UnityEngine;

/// <summary>
/// This class manages haptic feedback using the Bhaptics SDK.
/// It provides methods to run motors with specified strength and duration.
/// It also allows for the magnification of motor strengths.
/// </summary>
public class HapticManager : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    [Tooltip("The strength of the motors in the falling objects scene.")]
    private int _fallingObjectsMotorStrength = 75;

    [SerializeField]
    [Tooltip("The time in milliseconds that the motors will run for a single event.")]
    private int _singleEventMotorRunTimeMs = 500;

    /// <summary>
    /// The singleton instance of the HapticManager.
    /// </summary>
    public static HapticManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    /// <summary>
    /// Gets the single event motor run time in milliseconds.
    /// </summary>
    /// <returns>The single event motor run time in milliseconds.</returns>
    public virtual int GetSingleEventMotorRunTimeMs()
    {
        return _singleEventMotorRunTimeMs;
    }

    /// <summary>
    /// Gets the strength of the motors in the falling objects scene.
    /// </summary>
    /// <returns>The strength of the motors in the falling objects scene.</returns>
    public virtual int GetFallingObjectsMotorStrength()
    {
        return _fallingObjectsMotorStrength;
    }

    /// <summary>
    /// Magnifies the motor strengths by a specified factor. The maximum value of a motor for the end
    /// result is 100. If higher, bHaptics will ignore the value and set it to 100.
    /// </summary>
    /// <param name="motorValues">The original motor values.</param>
    /// <param name="magnificationFactor">The factor by which to magnify the motor values.</param>
    /// <returns>The magnified motor values.</returns>
    private int[] MagnifyMotorStrengths(int[] motorValues, int magnificationFactor)
    {
        int[] newMotorValues = new int[motorValues.Length];
        for (int i = 0; i < motorValues.Length; i++)
        {
            newMotorValues[i] = motorValues[i] * magnificationFactor;
        }
        return newMotorValues;
    }

    /// <summary>
    /// Runs the motors for a specified event with a specified strength and duration.
    /// </summary>
    /// <param name="bhapticsEvent">The motor event to run.</param>
    /// <param name="motorStrength">The strength of the motors.</param>
    /// <param name="durationMs">The duration in milliseconds for which the motors will run.</param>
    /// <returns>The request ID for the motor event.</returns>
    public virtual int RunMotors(MotorEvent bhapticsEvent, int motorStrength, int durationMs)
    {
        return BhapticsLibrary.PlayMotors((int)bhapticsEvent.PositionType, MagnifyMotorStrengths(bhapticsEvent.MotorValues, motorStrength), durationMs);
    }

    /// <summary>
    /// Stops the motors for a specified event using the request ID.
    /// </summary>
    /// <param name="requestId">The request ID for the motor event.</param>
    /// <returns>True if the motors were stopped successfully, false otherwise.</returns>
    public bool StopByRequestId(int requestId)
    {
        return BhapticsLibrary.StopInt(requestId);
    }
}

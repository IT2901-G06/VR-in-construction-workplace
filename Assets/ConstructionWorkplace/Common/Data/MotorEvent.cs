using Bhaptics.SDK2;

/// <summary>
/// Represents a motor event that specifies the position and motor values for haptic feedback.
/// </summary>
public struct MotorEvent
{
    /// <summary>
    /// The type of position for the motor event (e.g., Vest, GloveL, GloveR etc.).
    /// </summary>
    public PositionType PositionType;

    /// <summary>
    /// The values for each motor in the event. Each value corresponds to a specific motor.
    /// The amount of motors must be exactly the same amount as the motors count of the
    /// position type. You can read more about this here
    /// https://docs.bhaptics.com/sdk/unity/references/library#playmotors.
    /// </summary>
    public int[] MotorValues;

    /// <summary>
    /// Initializes a new instance of the <see cref="MotorEvent"/> struct with the specified position type and motor values.
    /// </summary>
    /// <param name="positionType">The type of position for the motor event.</param>
    /// <param name="motorValues">The values for each motor in the event.</param>
    public MotorEvent(PositionType positionType, int[] motorValues)
    {
        PositionType = positionType;
        MotorValues = motorValues;
    }
}
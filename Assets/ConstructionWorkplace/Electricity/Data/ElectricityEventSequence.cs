/// <summary>
/// This class contains a sequence of haptic events to be run one after another.
/// </summary>
public class ElectricityEventSequence
{
    /// <summary>
    /// The sequence of haptic events to be run one after another.
    /// </summary>
    public static MotorEvent[] EventSteps = {
        BhapticsEventCollection.GloveFingersRight,
        BhapticsEventCollection.GlovePalmRight,
        BhapticsEventCollection.VestFarRight,
        BhapticsEventCollection.VestMidRight,
        BhapticsEventCollection.VestMidLeft,
        BhapticsEventCollection.VestFarLeft,
        BhapticsEventCollection.GlovePalmLeft,
        BhapticsEventCollection.GloveFingersLeft,
    };
}
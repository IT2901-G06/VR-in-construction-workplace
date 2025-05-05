using Bhaptics.SDK2;

// This class exists solely to 

/// <summary>
/// Holds information about common haptic events. For more information about the Bhaptics SDK and what
/// each motor value represents, visit https://docs.bhaptics.com/sdk/further/motor.
/// </summary>
public class BhapticsEventCollection
{
    /// <summary>
    /// Enables all motors on the vest.
    /// </summary>
    public static MotorEvent VestAll = new(PositionType.Vest, new int[] {
        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,

        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
    });

    /// <summary>
    /// Enables all motors at the top row of the vest, both front and back.
    /// </summary>
    public static MotorEvent VestTop = new(PositionType.Vest, new int[] {
        1, 1, 1, 1,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,

        1, 1, 1, 1,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
    });

    /// <summary>
    /// Enables all motors at the bottom row of the vest, both front and back.
    /// </summary>
    public static MotorEvent VestBottom = new(PositionType.Vest, new int[] {
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        1, 1, 1, 1,

        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        1, 1, 1, 1,
    });

    /// <summary>
    /// Enables all front motors of the vest.
    /// </summary>
    public static MotorEvent VestFront = new(PositionType.Vest, new int[] {
        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,

        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
    });

    /// <summary>
    /// Enables all back motors of the vest.
    /// </summary>
    public static MotorEvent VestBack = new(PositionType.Vest, new int[] {
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,

        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
    });

    /// <summary>
    /// Enables all motors in the far left column of the vest, both front and back.
    /// </summary>
    public static MotorEvent VestFarLeft = new(PositionType.Vest, new int[] {
        1, 0, 0, 0,
        1, 0, 0, 0,
        1, 0, 0, 0,
        1, 0, 0, 0,

        1, 0, 0, 0,
        1, 0, 0, 0,
        1, 0, 0, 0,
        1, 0, 0, 0,
    });

    /// <summary>
    /// Enables all motors in the middle left column of the vest, both front and back.
    /// </summary>
    public static MotorEvent VestMidLeft = new(PositionType.Vest, new int[] {
        0, 1, 0, 0,
        0, 1, 0, 0,
        0, 1, 0, 0,
        0, 1, 0, 0,

        0, 1, 0, 0,
        0, 1, 0, 0,
        0, 1, 0, 0,
        0, 1, 0, 0,
    });

    /// <summary>
    /// Enables all motors in the middle right column of the vest, both front and back.
    /// </summary>
    public static MotorEvent VestMidRight = new(PositionType.Vest, new int[] {
        0, 0, 1, 0,
        0, 0, 1, 0,
        0, 0, 1, 0,
        0, 0, 1, 0,

        0, 0, 1, 0,
        0, 0, 1, 0,
        0, 0, 1, 0,
        0, 0, 1, 0,
    });

    /// <summary>
    /// Enables all motors in the far right column of the vest, both front and back.
    /// </summary>
    public static MotorEvent VestFarRight = new(PositionType.Vest, new int[] {
        0, 0, 0, 1,
        0, 0, 0, 1,
        0, 0, 0, 1,
        0, 0, 0, 1,

        0, 0, 0, 1,
        0, 0, 0, 1,
        0, 0, 0, 1,
        0, 0, 0, 1,
    });

    /// <summary>
    /// Enables all motors on the left glove.
    /// </summary>
    public static MotorEvent AllLeft = new(PositionType.GloveL, new int[] {
        1, 1, 1, 1, 1, 1,
    });

    /// <summary>
    /// Enables all motors on the right glove.
    /// </summary>
    public static MotorEvent AllRight = new(PositionType.GloveR, new int[] {
        1, 1, 1, 1, 1, 1,
    });

    /// <summary>
    /// Enables all finger motors on the left glove, but leaves the palm motor disabled.
    /// </summary>
    public static MotorEvent GloveFingersLeft = new(PositionType.GloveL, new int[] {
        1, 1, 1, 1, 1, 0,
    });

    /// <summary>
    /// Enables the palm motor on the left glove, but leaves all finger motors disabled.
    /// </summary>
    public static MotorEvent GlovePalmLeft = new(PositionType.GloveL, new int[] {
        0, 0, 0, 0, 0, 1,
    });

    /// <summary>
    /// Enables all finger motors on the right glove, but leaves the palm motor disabled.
    /// </summary>
    public static MotorEvent GloveFingersRight = new(PositionType.GloveR, new int[] {
        1, 1, 1, 1, 1, 0,
    });

    /// <summary>
    /// Enables the palm motor on the right glove, but leaves all finger motors disabled.
    /// </summary>
    public static MotorEvent GlovePalmRight = new(PositionType.GloveR, new int[] {
        0, 0, 0, 0, 0, 1,
    });

    /// <summary>
    /// Enables the index finger motor on the left glove, but leaves all other motors disabled.
    /// </summary>
    public static MotorEvent IndexFingerLeft = new(PositionType.GloveL, new int[] {
        0, 1, 0, 0, 0, 0,
    });

    /// <summary>
    /// Enables the index finger motor on the right glove, but leaves all other motors disabled.
    /// </summary>
    public static MotorEvent IndexFingerRight = new(PositionType.GloveR, new int[] {
        0, 1, 0, 0, 0, 0,
    });

    /// <summary>
    /// Enables the middle finger motor on the left glove, but leaves all other motors disabled.
    /// </summary>
    public static MotorEvent MiddleFingerLeft = new(PositionType.GloveL, new int[] {
        0, 0, 1, 0, 0, 0,
    });

    /// <summary>
    /// Enables the middle finger motor on the right glove, but leaves all other motors disabled.
    /// </summary>
    public static MotorEvent MiddleFingerRight = new(PositionType.GloveR, new int[] {
        0, 0, 1, 0, 0, 0,
    });

    /// <summary>
    /// Enables the ring finger motor on the left glove, but leaves all other motors disabled.
    /// </summary>
    public static MotorEvent RingFingerLeft = new(PositionType.GloveL, new int[] {
        0, 0, 0, 1, 0, 0,
    });

    /// <summary>
    /// Enables the ring finger motor on the right glove, but leaves all other motors disabled.
    /// </summary>
    public static MotorEvent RingFingerRight = new(PositionType.GloveR, new int[] {
        0, 0, 0, 1, 0, 0,
    });

    /// <summary>
    /// Enables the pinky finger motor on the left glove, but leaves all other motors disabled.
    /// </summary>
    public static MotorEvent PinkyFingerLeft = new(PositionType.GloveL, new int[] {
        0, 0, 0, 0, 1, 0,
    });

    /// <summary>
    /// Enables the pinky finger motor on the right glove, but leaves all other motors disabled.
    /// </summary>
    public static MotorEvent PinkyFingerRight = new(PositionType.GloveR, new int[] {
        0, 0, 0, 0, 1, 0,
    });

    /// <summary>
    /// Enables the thumb motor on the left glove, but leaves all other motors disabled.
    /// </summary>
    public static MotorEvent ThumbLeft = new(PositionType.GloveL, new int[] {
        1, 0, 0, 0, 0, 0,
    });

    /// <summary>
    /// Enables the thumb motor on the right glove, but leaves all other motors disabled.
    /// </summary>
    public static MotorEvent ThumbRight = new(PositionType.GloveR, new int[] {
        1, 0, 0, 0, 0, 0,
    });
}

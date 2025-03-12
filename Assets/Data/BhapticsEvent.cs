using Bhaptics.SDK2;

// This class exists solely to hold information about common haptic events. More information
// about the Bhaptics SDK and what each motor value represents, visit https://docs.bhaptics.com/sdk/further/motor
public class BhapticsEvent
{
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
    public static MotorEvent GloveFingersLeft = new(PositionType.GloveL, new int[] {
        1, 1, 1, 1, 1, 0,
    });
    public static MotorEvent GlovePalmLeft = new(PositionType.GloveL, new int[] {
        0, 0, 0, 0, 0, 1,
    });
    public static MotorEvent GloveFingersRight = new(PositionType.GloveR, new int[] {
        1, 1, 1, 1, 1, 0,
    });
    public static MotorEvent GlovePalmRight = new(PositionType.GloveR, new int[] {
        0, 0, 0, 0, 0, 1,
    });
}

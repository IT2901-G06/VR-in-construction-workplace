using Bhaptics.SDK2;

public class ElectricityEvent
{
    public static MotorEvent[] EventSteps = {
        new(PositionType.GloveR, new int[] {
            1, 1, 1, 1, 1, 0,
        }),
        new(PositionType.GloveR, new int[] {
            0, 0, 0, 0, 0, 1,
        }),
        new(PositionType.Vest, new int[] {
            0, 0, 0, 1,
            0, 0, 0, 1,
            0, 0, 0, 1,
            0, 0, 0, 1,
            0, 0, 0, 1,
            0, 0, 0, 1,
            0, 0, 0, 1,
            0, 0, 0, 1,
        }),
        new(PositionType.Vest, new int[] {
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
            0, 0, 1, 0,
        }),
        new(PositionType.Vest, new int[] {
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,
        }),
        new(PositionType.Vest, new int[] {
            1, 0, 0, 0,
            1, 0, 0, 0,
            1, 0, 0, 0,
            1, 0, 0, 0,
            1, 0, 0, 0,
            1, 0, 0, 0,
            1, 0, 0, 0,
            1, 0, 0, 0,
        }),
        new(PositionType.GloveL, new int[] {
            0, 0, 0, 0, 0, 1,
        }),
        new(PositionType.GloveL, new int[] {
            1, 1, 1, 1, 1, 0,
        }),
    };
}
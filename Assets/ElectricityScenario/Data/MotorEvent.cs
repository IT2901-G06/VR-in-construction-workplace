using Bhaptics.SDK2;

public struct MotorEvent {
    public PositionType PositionType;
    public int[] MotorValues;

    public MotorEvent(PositionType positionType, int[] motorValues) {
        PositionType = positionType;
        MotorValues = motorValues;
    }
}
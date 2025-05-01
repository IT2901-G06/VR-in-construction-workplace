using Bhaptics.SDK2;
using UnityEngine;

public class HapticController : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    private int _fallingObjectsMotorStrength = 75;

    [SerializeField]
    private int _singleEventMotorRunTimeMs = 500;

    public static HapticController Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public virtual int GetSingleEventMotorRunTimeMs()
    {
        return _singleEventMotorRunTimeMs;
    }

    public virtual int GetFallingObjectsMotorStrength()
    {
        return _fallingObjectsMotorStrength;
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

    public virtual int RunMotors(MotorEvent bhapticsEvent, int motorStrength, int durationMs)
    {
        return BhapticsLibrary.PlayMotors((int)bhapticsEvent.PositionType, MagnifyMotorStrengths(bhapticsEvent.MotorValues, motorStrength), durationMs);
    }

    public bool StopByRequestId(int requestId)
    {
        return BhapticsLibrary.StopInt(requestId);
    }
}

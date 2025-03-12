using Bhaptics.SDK2;
using UnityEngine;
using static BodyTriggerController;

public class HapticController : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    private int _minimumMotorStrength = 10;

    [SerializeField]
    private int _singleEventMotorRunTimeMs = 500;

    public static HapticController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }
    }

    public int GetSingleEventMotorRunTimeMs()
    {
        return _singleEventMotorRunTimeMs;
    }

    public int GetMaxMotorStrength()
    {
        return 100;
    }

    public int GetMinMotorStrength()
    {
        return _minimumMotorStrength;
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

    public int RunMotors(MotorEvent bhapticsEvent, int motorStrength, int durationMillis)
    {
        return BhapticsLibrary.PlayMotors((int)bhapticsEvent.PositionType, MagnifyMotorStrengths(bhapticsEvent.MotorValues, motorStrength), durationMillis);
    }

    public bool StopByRequestId(int requestId)
    {
        return BhapticsLibrary.StopInt(requestId);
    }
}

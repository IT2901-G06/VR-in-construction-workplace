using Bhaptics.SDK2;
using UnityEngine;
using static BodyTriggerController;

public class HapticController : MonoBehaviour
{

    [SerializeField]
    [Range(0, 100)]
    private int _minimumMotorStrength = 10;

    [SerializeField]
    private int _motorRunTimeMs = 500;

    public static HapticController Instance;

    private int[] VestTop;
    private int[] VestFront;
    private int[] VestBack;
    private int[] VestLeft;
    private int[] VestRight;

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

        VestTop = new int[]
        {
            1, 1, 1, 1,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,

            1, 1, 1, 1,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };

        VestFront = new int[]
        {
            1, 1, 1, 1,
            1, 1, 1, 1,
            1, 1, 1, 1,
            1, 1, 1, 1,

            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
        };

        VestBack = new int[]
        {
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,

            1, 1, 1, 1,
            1, 1, 1, 1,
            1, 1, 1, 1,
            1, 1, 1, 1,
        };

        VestLeft = new int[]
        {
            1, 0, 0, 0,
            1, 0, 0, 0,
            1, 0, 0, 0,
            1, 0, 0, 0,

            1, 0, 0, 0,
            1, 0, 0, 0,
            1, 0, 0, 0,
            1, 0, 0, 0,
        };

        VestRight = new int[]
        {
            0, 0, 0, 1,
            0, 0, 0, 1,
            0, 0, 0, 1,
            0, 0, 0, 1,

            0, 0, 0, 1,
            0, 0, 0, 1,
            0, 0, 0, 1,
            0, 0, 0, 1,
        };
    }

    public int GetMinimumMotorStrength()
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

    public void RunMotors(TriggerPositionType position, int motorStrength)
    {
        Debug.Log(position + " " + motorStrength);
        switch (position)
        {
            case TriggerPositionType.Front:
                BhapticsLibrary.PlayMotors((int)PositionType.Vest, MagnifyMotorStrengths(VestFront, motorStrength), _motorRunTimeMs);
                break;
            case TriggerPositionType.Back:
                BhapticsLibrary.PlayMotors((int)PositionType.Vest, MagnifyMotorStrengths(VestBack, motorStrength), _motorRunTimeMs);
                break;
            case TriggerPositionType.Left:
                BhapticsLibrary.PlayMotors((int)PositionType.Vest, MagnifyMotorStrengths(VestLeft, motorStrength), _motorRunTimeMs);
                break;
            case TriggerPositionType.Right:
                BhapticsLibrary.PlayMotors((int)PositionType.Vest, MagnifyMotorStrengths(VestRight, motorStrength), _motorRunTimeMs);
                break;
            case TriggerPositionType.Head:
                BhapticsLibrary.PlayMotors((int)PositionType.Vest, MagnifyMotorStrengths(VestTop, motorStrength), _motorRunTimeMs);
                break;
        }
    }
}

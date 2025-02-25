using Bhaptics.SDK2;
using UnityEngine;
using static BodyTriggerController;

public class HapticController : MonoBehaviour
{

    [SerializeField]
    private int motorRunTimeMs = 500;

    public static HapticController instance;

    private int[] VestTop;
    private int[] VestFront;
    private int[] VestBack;
    private int[] VestLeft;
    private int[] VestRight;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
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
                BhapticsLibrary.PlayMotors((int)PositionType.Vest, MagnifyMotorStrengths(VestFront, motorStrength), motorRunTimeMs);
                break;
            case TriggerPositionType.Back:
                BhapticsLibrary.PlayMotors((int)PositionType.Vest, MagnifyMotorStrengths(VestBack, motorStrength), motorRunTimeMs);
                break;
            case TriggerPositionType.Left:
                BhapticsLibrary.PlayMotors((int)PositionType.Vest, MagnifyMotorStrengths(VestLeft, motorStrength), motorRunTimeMs);
                break;
            case TriggerPositionType.Right:
                BhapticsLibrary.PlayMotors((int)PositionType.Vest, MagnifyMotorStrengths(VestRight, motorStrength), motorRunTimeMs);
                break;
            case TriggerPositionType.Head:
                BhapticsLibrary.PlayMotors((int)PositionType.Vest, MagnifyMotorStrengths(VestTop, motorStrength), motorRunTimeMs);
                break;
        }
    }
}

using Bhaptics.SDK2;
using UnityEngine;
using static BodyTriggerController;

public class HapticController : MonoBehaviour
{

    [SerializeField]
    [Range(0, 100)]
    private int motorStrength;

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
        } else
        {
            Destroy(this);
        }

    VestTop = new int[] {
        motorStrength, motorStrength, motorStrength, motorStrength,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,

        motorStrength, motorStrength, motorStrength, motorStrength,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
    };

    VestFront = new int[] {
        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,

        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
    };

    VestBack = new int[] {
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,

        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,
    };

    VestLeft = new int[] {
        motorStrength, 0, 0, 0,
        motorStrength, 0, 0, 0,
        motorStrength, 0, 0, 0,
        motorStrength, 0, 0, 0,

        motorStrength, 0, 0, 0,
        motorStrength, 0, 0, 0,
        motorStrength, 0, 0, 0,
        motorStrength, 0, 0, 0,
    };

    VestRight = new int[] {
        0, 0, 0, motorStrength,
        0, 0, 0, motorStrength,
        0, 0, 0, motorStrength,
        0, 0, 0, motorStrength,

        0, 0, 0, motorStrength,
        0, 0, 0, motorStrength,
        0, 0, 0, motorStrength,
        0, 0, 0, motorStrength,
    };
    }

    public void RunMotors (TriggerPositionType position)
    {
        Debug.Log(position);
        switch(position)
        {
            case TriggerPositionType.Front:
                BhapticsLibrary.PlayMotors((int) PositionType.Vest, VestFront, motorRunTimeMs);
                break;
            case TriggerPositionType.Back:
                BhapticsLibrary.PlayMotors((int) PositionType.Vest, VestBack, motorRunTimeMs);
                break;
            case TriggerPositionType.Left:
                BhapticsLibrary.PlayMotors((int) PositionType.Vest, VestLeft, motorRunTimeMs);
                break;
            case TriggerPositionType.Right:
                BhapticsLibrary.PlayMotors((int) PositionType.Vest, VestRight, motorRunTimeMs);
                break;
            case TriggerPositionType.Head:
                BhapticsLibrary.PlayMotors((int) PositionType.Vest, VestTop, motorRunTimeMs);
                break;
        }
    }
}

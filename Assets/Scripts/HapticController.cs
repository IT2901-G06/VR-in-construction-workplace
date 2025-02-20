using Bhaptics.SDK2;
using UnityEngine;

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



    public void RunHead ()
    {
        Debug.Log("head");
        BhapticsLibrary.PlayMotors((int) PositionType.Vest, VestTop, motorRunTimeMs);
    }

    public void RunFront ()
    {
        Debug.Log("front");
        BhapticsLibrary.PlayMotors((int) PositionType.Vest, VestFront, motorRunTimeMs);
    }

    public void RunBack ()
    {
        Debug.Log("back");
        BhapticsLibrary.PlayMotors((int) PositionType.Vest, VestBack, motorRunTimeMs);
    }

    public void RunLeft ()
    {
        Debug.Log("left");
        BhapticsLibrary.PlayMotors((int) PositionType.Vest, VestLeft, motorRunTimeMs);
    }

    public void RunRight ()
    {
        Debug.Log("right");
        BhapticsLibrary.PlayMotors((int) PositionType.Vest, VestRight, motorRunTimeMs);

    }
}

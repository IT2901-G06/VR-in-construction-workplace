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

    private int[] AllMaxTopVest;
    private int[] AllMaxFrontVest;
    private int[] AllMaxBackVest;

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

    AllMaxTopVest = new int[] {
        motorStrength, motorStrength, motorStrength, motorStrength,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,

        motorStrength, motorStrength, motorStrength, motorStrength,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
    };

    AllMaxFrontVest = new int[] {
        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,

        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
    };

    AllMaxBackVest = new int[] {
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,

        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,
        motorStrength, motorStrength, motorStrength, motorStrength,
    };
    }



    public void RunHead ()
    {
        Debug.Log("head");
        BhapticsLibrary.PlayMotors((int) PositionType.Vest, AllMaxTopVest, motorRunTimeMs);
    }

    public void RunFront ()
    {
        Debug.Log("front");
        BhapticsLibrary.PlayMotors((int) PositionType.Vest, AllMaxFrontVest, motorRunTimeMs);
    }

    public void RunBack ()
    {
        Debug.Log("back");
        BhapticsLibrary.PlayMotors((int) PositionType.Vest, AllMaxBackVest, motorRunTimeMs);
    }
}

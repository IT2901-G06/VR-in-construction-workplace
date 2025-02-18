using Bhaptics.SDK2;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{

    private int[] AllMaxTopVest = new int[] {
        100, 100, 100, 100,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,

        100, 100, 100, 100,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FallingObject"))
        {
            BhapticsLibrary.PlayMotors((int) PositionType.Vest, AllMaxTopVest, 500);
        }
    }
}

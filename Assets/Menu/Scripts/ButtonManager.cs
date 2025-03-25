using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public void HandleGloveButtonPress()
    {
        Debug.Log("Glove Button Pressed");

        HapticController hapticController = HapticController.Instance;

        if (hapticController != null)
        {
            MotorEvent motorEvent = BhapticsEventCollection.GloveFingersLeft;

            int motorStrength = 75;

            hapticController.RunMotors(motorEvent, motorStrength, hapticController.GetSingleEventMotorRunTimeMs());
        }
        else
        {
            Debug.Log("Haptic Controller is null");
        }
    }

    public void HandleSuitButtonPress()
    {
        Debug.Log("Suit Button Pressed");

        HapticController hapticController = HapticController.Instance;

        if (hapticController != null)
        {
            MotorEvent motorEvent = BhapticsEventCollection.VestFront;

            int motorStrength = 75;

            hapticController.RunMotors(motorEvent, motorStrength, hapticController.GetSingleEventMotorRunTimeMs());
        }
        else
        {
            Debug.Log("Haptic Controller is null");
        }
    }
}

using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public void HandleGloveButtonPress()
    {
        Debug.Log("Glove Button Pressed");

        HapticController hapticController = HapticController.Instance;

        if (hapticController != null)
        {
            Debug.Log("Haptic Controller is not null");

            int motorStrength = 75;
            int duration = hapticController.GetSingleEventMotorRunTimeMs();

            // Left glove
            hapticController.RunMotors(BhapticsEventCollection.GloveFingersLeft, motorStrength, duration);
            hapticController.RunMotors(BhapticsEventCollection.GlovePalmLeft, motorStrength, duration);

            // Right glove
            hapticController.RunMotors(BhapticsEventCollection.GloveFingersRight, motorStrength, duration);
            hapticController.RunMotors(BhapticsEventCollection.GlovePalmRight, motorStrength, duration);
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
            Debug.Log("Haptic Controller is not null");

            int motorStrength = 75;
            int duration = hapticController.GetSingleEventMotorRunTimeMs();

            // Trigger both front and back simultaneously
            hapticController.RunMotors(BhapticsEventCollection.VestFront, motorStrength, duration);
            hapticController.RunMotors(BhapticsEventCollection.VestBack, motorStrength, duration);

        }
        else
        {
            Debug.Log("Haptic Controller is null");
        }
    }
}

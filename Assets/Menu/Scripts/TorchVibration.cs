using UnityEngine;

public class TorchVibration : MonoBehaviour
{

    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject torchFlameArea;

    void Start()
    {
        // Add colliders if not already present
        if (leftHand != null && leftHand.GetComponent<Collider>() == null)
        {
            SphereCollider leftCollider = leftHand.AddComponent<SphereCollider>();
            leftCollider.radius = 0.05f;
            leftCollider.isTrigger = true;  // This makes it a trigger collider
        }

        if (rightHand != null && rightHand.GetComponent<Collider>() == null)
        {
            SphereCollider rightCollider = rightHand.AddComponent<SphereCollider>();
            rightCollider.radius = 0.05f;
            rightCollider.isTrigger = true;
        }

        // Make sure torch flame is also a trigger
        if (torchFlameArea != null)
        {
            Collider flameCollider = torchFlameArea.GetComponent<Collider>();
            if (flameCollider != null)
                flameCollider.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for hand collision with the torch flame area
        CheckHandCollision();

    }

    public void TriggerLeftHandHaptic()
    {
        Debug.Log("Left Hand Haptic Triggered");

        HapticController hapticController = HapticController.Instance;

        if (hapticController != null)
        {
            Debug.Log("Haptic Controller is not null");

            int motorStrength = 75;
            int duration = hapticController.GetSingleEventMotorRunTimeMs();

            // Left hand
            hapticController.RunMotors(BhapticsEventCollection.GloveFingersLeft, motorStrength, duration);
            hapticController.RunMotors(BhapticsEventCollection.GlovePalmLeft, motorStrength, duration);
        }
        else
        {
            Debug.Log("Haptic Controller is null");
        }
    }

    public void TriggerRightHandHaptic()
    {
        Debug.Log("Right Hand Haptic Triggered");

        HapticController hapticController = HapticController.Instance;

        if (hapticController != null)
        {
            Debug.Log("Haptic Controller is not null");

            int motorStrength = 75;
            int duration = hapticController.GetSingleEventMotorRunTimeMs();

            // Right hand
            hapticController.RunMotors(BhapticsEventCollection.GloveFingersRight, motorStrength, duration);
            hapticController.RunMotors(BhapticsEventCollection.GlovePalmRight, motorStrength, duration);
        }
        else
        {
            Debug.Log("Haptic Controller is null");
        }
    }

    public void CheckHandCollision()
    {
        if (leftHand.GetComponent<Collider>().bounds.Intersects(torchFlameArea.GetComponent<Collider>().bounds))
        {
            TriggerLeftHandHaptic();
        }

        if (rightHand.GetComponent<Collider>().bounds.Intersects(torchFlameArea.GetComponent<Collider>().bounds))
        {
            TriggerRightHandHaptic();
        }
    }
}

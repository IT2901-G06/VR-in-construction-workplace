using UnityEngine;

public class BodyPartTrigger : MonoBehaviour
{

    [Tooltip("Select the position for the haptic feedback")]
    [SerializeField]
    private TriggerPositionType _position;

    public enum TriggerPositionType
    {
        Front,
        Back,
        Left,
        Right,
        Head
    }

    void Update()
    {
        Quaternion cameraRotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
        transform.rotation = new Quaternion(0, cameraRotation.y, 0, cameraRotation.w);
    }

    private MotorEvent TranslateTriggerPositionTypeToMotorEvent(TriggerPositionType position)
    {
        return position switch
        {
            TriggerPositionType.Front => BhapticsEvent.VestFront,
            TriggerPositionType.Back => BhapticsEvent.VestBack,
            TriggerPositionType.Left => BhapticsEvent.VestFarLeft,
            TriggerPositionType.Right => BhapticsEvent.VestFarLeft,
            TriggerPositionType.Head => BhapticsEvent.VestTop,
            _ => BhapticsEvent.VestFront,
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallingObject"))
        {
            HapticController hapticController = HapticController.Instance;
            int motorStrength = Mathf.Clamp((int)(other.attachedRigidbody.linearVelocity.magnitude * 10), hapticController.GetMinMotorStrength(), hapticController.GetMaxMotorStrength());

            MotorEvent motorEvent = TranslateTriggerPositionTypeToMotorEvent(_position);
            hapticController.RunMotors(motorEvent, motorStrength, hapticController.GetSingleEventMotorRunTimeMs());
        }
    }
}

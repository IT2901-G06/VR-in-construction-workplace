using UnityEngine;

/// <summary>
/// This class handles the haptic feedback for different body parts when a falling object collides with the trigger.
/// </summary>
public class BodyPartTrigger : MonoBehaviour
{
    [Tooltip("Select the position for the haptic feedback")]
    [SerializeField]
    private TriggerPositionType _position;

    private static readonly float _percentOfPlayerHeightForBody = 0.6f;
    private float _percentOfHeightToFill = _percentOfPlayerHeightForBody;

    /// <summary>
    /// Enum representing the position of the trigger for haptic feedback.
    /// </summary>
    public enum TriggerPositionType
    {
        Front,
        Back,
        Left,
        Right,
        Head,
        LeftHand,
        RightHand
    }

    private Vector3 _initialCenter;
    private float _initialRigY;
    private Oculus.Interaction.Locomotion.CharacterController _cameraCharController;

    void Start()
    {
        // Set the trigger size based on the position
        if (_position == TriggerPositionType.Head)
        {
            _percentOfHeightToFill = 1 - _percentOfPlayerHeightForBody;
        }

        _initialCenter = GetComponent<BoxCollider>().center;
        _initialRigY = GameObject.Find("PlayerController").transform.position.y / 2;
        _cameraCharController = GameObject.Find("PlayerController").GetComponent<Oculus.Interaction.Locomotion.CharacterController>();
    }

    void Update()
    {
        // Hands shouldnt change size when the player changes size
        if (_position == TriggerPositionType.LeftHand || _position == TriggerPositionType.RightHand) return;

        // Update the trigger size based on the player's height
        float newHeight = (_cameraCharController.transform.position.y + _initialRigY) * _percentOfHeightToFill;
        Vector3 currentSize = GetComponent<BoxCollider>().size;
        GetComponent<BoxCollider>().size = new(currentSize.x, newHeight, currentSize.z);

        float newCenter = newHeight / 2 - _initialRigY;
        if (_position == TriggerPositionType.Head)
        {
            newCenter += _percentOfPlayerHeightForBody * (_cameraCharController.transform.position.y + _initialRigY);
        }
        GetComponent<BoxCollider>().center = new(_initialCenter.x, newCenter, _initialCenter.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is a falling object
        if (other.CompareTag("FallingObject"))
        {
            HapticManager hapticManager = HapticManager.Instance;

            MotorEvent motorEvent = BhapticsEventCollection.VestAll;
            switch (_position)
            {
                case TriggerPositionType.LeftHand:
                    motorEvent = BhapticsEventCollection.AllLeft;
                    break;
                case TriggerPositionType.RightHand:
                    motorEvent = BhapticsEventCollection.AllRight;
                    break;
            }

            hapticManager.RunMotors(motorEvent, hapticManager.GetFallingObjectsMotorStrength(), hapticManager.GetSingleEventMotorRunTimeMs());
            Debug.Log(_position + ": " + hapticManager.GetFallingObjectsMotorStrength());

            // Kill if hit by any falling object, no matter the velocity
            DeathManager.Instance.Kill();
        }
    }
}


using UnityEngine;

public class BodyPartTrigger : MonoBehaviour
{

    [Tooltip("Select the position for the haptic feedback")]
    [SerializeField]
    private TriggerPositionType _position;

    private static readonly float _percentOfPlayerHeightForBody = 0.6f;
    private float _percentOfHeightToFill = _percentOfPlayerHeightForBody;

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
        if (_position == TriggerPositionType.LeftHand || _position == TriggerPositionType.RightHand) return;

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
            DeathManager.Instance.Kill();
        }
    }
}


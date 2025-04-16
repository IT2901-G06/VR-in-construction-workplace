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
        Head
    }

    private Vector3 _initialCenter;
    private float _initialRigY;
    private Oculus.Interaction.Locomotion.CharacterController _cameraCharController;

    void Awake()
    {
        if (_position == TriggerPositionType.Head)
        {
            _percentOfHeightToFill = 1 - _percentOfPlayerHeightForBody;
        }
    }

    void Start()
    {
        _initialCenter = GetComponent<BoxCollider>().center;
        _initialRigY = GameObject.FindGameObjectWithTag("Player").transform.position.y / 2;
        _cameraCharController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Oculus.Interaction.Locomotion.CharacterController>();
    }

    void Update()
    {
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

    private MotorEvent TranslateTriggerPositionTypeToMotorEvent(TriggerPositionType position)
    {
        return position switch
        {
            TriggerPositionType.Front => BhapticsEventCollection.VestFront,
            TriggerPositionType.Back => BhapticsEventCollection.VestBack,
            TriggerPositionType.Left => BhapticsEventCollection.VestFarLeft,
            TriggerPositionType.Right => BhapticsEventCollection.VestFarLeft,
            TriggerPositionType.Head => BhapticsEventCollection.VestTop,
            _ => BhapticsEventCollection.VestFront,
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
            Debug.Log(_position + ": " + motorStrength);
            if (motorStrength >= 50)
            {
                DeathManager.Instance.Kill();
            }
        }
    }
}


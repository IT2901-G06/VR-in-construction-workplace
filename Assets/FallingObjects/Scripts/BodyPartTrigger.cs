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

    private float _initialHeightProportion;
    private float _initialCameraYCenter;
    private float _initialCenterProportion;
    private Vector3 _initialCenter;

    void Start()
    {
        _initialCenter = GetComponent<BoxCollider>().center;
        _initialHeightProportion = GetComponent<BoxCollider>().size.y / GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().height;
        _initialCameraYCenter = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().center.y;
        _initialCenterProportion = (_initialCameraYCenter + _initialCenter.y) / GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().height;
    }

    void Update()
    {
        Quaternion cameraRotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
        transform.rotation = new(0, cameraRotation.y, 0, cameraRotation.w);

        CharacterController cameraCharController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();

        float newHeight = _initialHeightProportion * cameraCharController.height;
        Vector3 currentSize = GetComponent<BoxCollider>().size;
        GetComponent<BoxCollider>().size = new(currentSize.x, newHeight, currentSize.z);

        float newCenter = _initialCenterProportion * cameraCharController.height - _initialCameraYCenter + cameraCharController.skinWidth;
        GetComponent<BoxCollider>().center = new(_initialCenter.x, newCenter, _initialCenter.z);

        Vector3 cameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        transform.position = new(cameraPos.x, transform.position.y, cameraPos.z);
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
        }
    }
}


using UnityEngine;

public class ExitButtonScript : MonoBehaviour
{
    [SerializeField]
    private Transform handTransform;
    
    private readonly float _middle = 180f;
    [SerializeField]
    [Range(0, 180)]
    private float _thresholdVariance = 40f;

    void Update()
    {
        float zRotation = handTransform.localEulerAngles.z;

        if (zRotation > _middle - _thresholdVariance && zRotation < _middle + _thresholdVariance)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}

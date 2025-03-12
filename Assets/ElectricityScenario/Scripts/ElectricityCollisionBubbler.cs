using UnityEngine;

public class ElectricityCollisionBubbler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider collider)
    {
        if (!transform.parent.TryGetComponent<ElectricityManager>(out var electricityScript))
        {
            Debug.LogError("ElectricityCollisionBubbler: ElectricityScript not found in parent");
            return;
        }

        electricityScript.OnTriggerEnterFromChild(transform, collider);      
    }

    void OnTriggerExit(Collider collider)
    {
        if (!transform.parent.TryGetComponent<ElectricityManager>(out var electricityScript))
        {
            Debug.LogError("ElectricityCollisionBubbler: ElectricityScript not found in parent");
            return;
        }

        electricityScript.OnTriggerExitFromChild(transform, collider);
    }
}

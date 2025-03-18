using UnityEngine;

public class ElectricityCollisionBubbler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider collider)
    {
        if (!transform.parent.TryGetComponent<ElectricityManager>(out var electricityManager))
        {
            Debug.LogError("ElectricityCollisionBubbler: ElectricityManager not found in parent");
            return;
        }

        electricityManager.OnTriggerEnterFromChild(transform, collider);      
    }

    void OnTriggerExit(Collider collider)
    {
        if (!transform.parent.TryGetComponent<ElectricityManager>(out var electricityManager))
        {
            Debug.LogError("ElectricityCollisionBubbler: ElectricityManager not found in parent");
            return;
        }

        electricityManager.OnTriggerExitFromChild(transform, collider);
    }
}

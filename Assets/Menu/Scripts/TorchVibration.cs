using System.Collections.Generic;
using UnityEngine;

public class TorchVibration : MonoBehaviour
{

    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject torchFlameArea;

    private List<Collider> createdColliders = new List<Collider>();


    [Header("Colliders")]
    public Vector3 handColliderSize = new Vector3(0.1f, 0.1f, 0.1f); // Size of the hand colliders
    public Vector3 handColliderOffset = Vector3.zero; // Offset for the hand colliders

    [Header("Debug")]
    public bool visualizeColliders = true;
    public Color colliderColor = Color.red;

    private GameObject leftHandVisualizer;
    private GameObject rightHandVisualizer;

    void Start()
    {
        AddColliders(); // Add colliders to hands and torch flame area
        VisualizeHandColliders(); // Visualize the colliders for debugging
    }

    // Update is called once per frame
    void Update()
    {
        // Check for hand collision with the torch flame area
        CheckHandCollision();

    }


    public void AddColliders()
    {
        // Add box colliders if not already present
        if (leftHand != null && leftHand.GetComponent<Collider>() == null)
        {
            BoxCollider leftCollider = leftHand.AddComponent<BoxCollider>();
            leftCollider.size = handColliderSize;       // Use Vector3 for custom dimensions
            leftCollider.center = handColliderOffset;   // Set position offset
            leftCollider.isTrigger = true;
            createdColliders.Add(leftCollider); // Add to the list of created colliders
        }

        if (rightHand != null && rightHand.GetComponent<Collider>() == null)
        {
            BoxCollider rightCollider = rightHand.AddComponent<BoxCollider>();
            rightCollider.size = handColliderSize;      // Use Vector3 for custom dimensions
            rightCollider.center = handColliderOffset;  // Set position offset
            rightCollider.isTrigger = true;
            createdColliders.Add(rightCollider); // Add to the list of created colliders
        }

        // Make sure torch flame is also a trigger
        if (torchFlameArea != null)
        {
            Collider flameCollider = torchFlameArea.GetComponent<Collider>();
            if (flameCollider != null)
                flameCollider.isTrigger = true;
        }
    }

    public void TriggerLeftHandHaptic()
    {
        Debug.Log("Left Hand Haptic Triggered");

        HapticController hapticController = HapticController.Instance;

        if (hapticController != null)
        {
            Debug.Log("Haptic Controller is not null");

            int motorStrength = 50;
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

            int motorStrength = 50;
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

    public void VisualizeHandColliders()
    {
        // Remove existing visualizers
        if (leftHandVisualizer != null)
            Destroy(leftHandVisualizer);
        if (rightHandVisualizer != null)
            Destroy(rightHandVisualizer);

        if (!visualizeColliders)
            return;

        // Create left hand visualizer
        if (leftHand != null)
        {
            Collider collider = leftHand.GetComponent<Collider>();
            if (collider != null && collider is BoxCollider)
            {
                BoxCollider boxCol = collider as BoxCollider;
                leftHandVisualizer = CreateBoxVisualizer("LeftHandVisual", leftHand, boxCol.size);
            }
        }

        // Create right hand visualizer
        if (rightHand != null)
        {
            Collider collider = rightHand.GetComponent<Collider>();
            if (collider != null && collider is BoxCollider)
            {
                BoxCollider boxCol = collider as BoxCollider;
                rightHandVisualizer = CreateBoxVisualizer("RightHandVisual", rightHand, boxCol.size);
            }
        }
    }

    private GameObject CreateBoxVisualizer(string name, GameObject parent, Vector3 size)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = name;

        // Remove the collider component (we just want visual)
        Destroy(cube.GetComponent<Collider>());

        // Make it a child of the hand
        cube.transform.SetParent(parent.transform);

        // Set position to match collider's offset
        cube.transform.localPosition = handColliderOffset;

        // Set size to match collider's dimensions
        cube.transform.localScale = size;

        // Set solid color for debugging
        Renderer renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = colliderColor;
            renderer.material = mat;
        }

        return cube;
    }

    public void OnDestroy()
    {
        // Clean up only the colliders we created
        foreach (var collider in createdColliders)
        {
            if (collider != null)
                Destroy(collider);
        }
        createdColliders.Clear();
    }

}

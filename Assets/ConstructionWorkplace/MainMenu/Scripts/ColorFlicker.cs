using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class handles the color flicker effect for a button press.
/// </summary>
public class ColorFlicker : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Renderer to apply the color effect to")]
    public Renderer targetRenderer;

    [Header("Color Settings")]
    [Tooltip("Color to show when button is pressed")]
    public Color pressedColor = Color.red;

    [Tooltip("Use material instead of direct color")]
    public bool useCustomMaterial = false;

    [Tooltip("Custom material to use if useCustomMaterial is true")]
    public Material customPressedMaterial;

    [Tooltip("Material slot index (for multi-material objects)")]
    public int materialIndex = 0;

    [Header("Events")]
    [Tooltip("Event triggered when button is pressed")]
    public UnityEvent OnButtonPressed;

    [Tooltip("Event triggered when button is released")]
    public UnityEvent OnButtonReleased;

    // Store the original material
    private Material originalMaterial;
    private Material generatedMaterial;
    private bool isPressed = false;

    private void Start()
    {
        // Auto-find renderer if not assigned
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }

        // Store the original material
        if (targetRenderer != null && targetRenderer.materials.Length > materialIndex)
        {
            originalMaterial = targetRenderer.materials[materialIndex];
        }

        // Create our color material if we're not using a custom one
        if (!useCustomMaterial)
        {
            CreateColorMaterial();
        }
    }

    /// <summary>
    /// Creates a material with the specified color
    /// </summary>
    private void CreateColorMaterial()
    {
        // If we already have a generated material, just update its color
        if (generatedMaterial != null)
        {
            generatedMaterial.color = pressedColor;
            return;
        }

        // Try to find an appropriate shader based on the render pipeline
        Shader shader = null;

        // Try Universal Render Pipeline first
        shader = Shader.Find("Universal Render Pipeline/Lit");

        // Try built-in Standard shader
        if (shader == null)
            shader = Shader.Find("Standard");

        // Try Legacy shader as fallback
        if (shader == null)
            shader = Shader.Find("Diffuse");

        // Final fallback - unlit color shader (should always be available)
        if (shader == null)
            shader = Shader.Find("Unlit/Color");

        if (shader == null)
        {
            Debug.LogError("Could not find any suitable shader for color material!");
            return;
        }

        // Create the material with the found shader
        generatedMaterial = new Material(shader);
        generatedMaterial.color = pressedColor;

        // Try to make it emissive if the shader supports it
        if (generatedMaterial.HasProperty("_EmissionColor"))
        {
            generatedMaterial.EnableKeyword("_EMISSION");
            generatedMaterial.SetColor("_EmissionColor", pressedColor);
        }
    }

    /// <summary>
    /// Call this when button is pressed
    /// </summary>
    public void OnButtonPress()
    {
        if (targetRenderer == null || isPressed)
        {
            return;
        }

        // Make sure we have the original material saved
        if (originalMaterial == null && targetRenderer.materials.Length > materialIndex)
        {
            originalMaterial = targetRenderer.materials[materialIndex];
        }

        // Make sure we have a material to use
        if (!useCustomMaterial && generatedMaterial == null)
        {
            CreateColorMaterial();
        }

        // Apply the pressed material/color
        ApplyPressedMaterial();

        isPressed = true;
        OnButtonPressed?.Invoke();
    }

    /// <summary>
    /// Call this when button is released
    /// </summary>
    public void OnButtonRelease()
    {
        if (targetRenderer == null || !isPressed)
        {
            return;
        }

        // Restore the original material
        RestoreOriginalMaterial();

        isPressed = false;
        OnButtonReleased?.Invoke();
    }

    /// <summary>
    /// Apply the pressed material/color
    /// </summary>
    private void ApplyPressedMaterial()
    {
        // Save materials array to modify
        Material[] materials = targetRenderer.materials;

        // Determine which material to use
        Material pressedMat = useCustomMaterial ? customPressedMaterial : generatedMaterial;

        if (pressedMat != null)
        {
            materials[materialIndex] = pressedMat;
            targetRenderer.materials = materials;
        }
    }

    /// <summary>
    /// Restore the original material
    /// </summary>
    private void RestoreOriginalMaterial()
    {
        if (originalMaterial != null)
        {
            // Save materials array to modify
            Material[] materials = targetRenderer.materials;
            materials[materialIndex] = originalMaterial;
            targetRenderer.materials = materials;
        }
    }

    /// <summary>
    /// Set a new pressed color at runtime
    /// </summary>
    public void SetPressedColor(Color newColor)
    {
        pressedColor = newColor;

        // Update or create the material with this color
        if (generatedMaterial != null)
        {
            generatedMaterial.color = newColor;
            generatedMaterial.SetColor("_EmissionColor", newColor);
        }
        else
        {
            CreateColorMaterial();
        }
    }
}
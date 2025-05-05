using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class updates a UI Text component with a formatted string.
/// </summary>
public class TextUpdate : MonoBehaviour
{
    [Tooltip("Format string for displaying values (e.g. '{0}%' or 'Value: {0:F1}')")]
    public string format = "{0}%";

    private Text textComponent;

    void Start()
    {
        // Get the UI Text component attached to this GameObject
        textComponent = GetComponent<Text>();

        if (textComponent == null)
        {
            Debug.LogError("No UI Text component found on " + gameObject.name);
        }
    }

    /// <summary>
    /// Updates the text value with the specified float value.
    /// </summary>
    /// <param name="value">The float value to display.</param>
    public void UpdateTextValue(float value)
    {
        if (textComponent != null)
        {
            // Format the value according to the format string
            textComponent.text = string.Format(format, value);
        }
    }
}
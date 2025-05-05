using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A script to manage the electricity scenario.
/// </summary>
public class ElectricityScenarioManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Whether or not the second part of the electricity scenario is active.")]
    private bool _partTwo;

    public static ElectricityScenarioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Sets the scene to the second part of the electricity scenario and runs it.
    /// </summary>
    public void GoToPartTwo()
    {
        _partTwo = true;
        Destroy(GameObject.Find("OVRCameraRig"));
        SceneManager.LoadScene("Electricity");
    }

    /// <summary>
    /// Whether or not the second part of the electricity scenario is active.
    /// </summary>
    /// <returns>True if the second part is active, false otherwise.</returns>
    public bool IsPartTwo()
    {
        return _partTwo;
    }
}

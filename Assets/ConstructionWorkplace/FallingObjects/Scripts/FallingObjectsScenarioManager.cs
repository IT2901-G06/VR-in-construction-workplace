using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the scenario for the Falling Objects game.
/// It handles the transition between different parts of the game,
/// manages the dialogue trees, and ensures that the game state is preserved.
/// </summary>
public class FallingObjectsScenarioManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Whether the game is in part two or not")]
    private bool _partTwo;

    [SerializeField]
    [Tooltip("List of dialogue trees for part 2")]
    private List<DialogueTree> _dialogueTrees;

    public static FallingObjectsScenarioManager Instance;

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
    /// This method is called to transition to part two of the scenario.
    /// It handles the destruction of the OVRCameraRig and reloads the FallingObjects scene.
    /// </summary>
    public void GoToPartTwo()
    {
        _partTwo = true;
        Destroy(GameObject.Find("OVRCameraRig"));
        SceneManager.LoadScene("FallingObjects");
    }

    /// <summary>
    /// Checks if the game is in part two.
    /// </summary>
    /// <returns>True if the game is in part two, false otherwise.</returns>
    public bool IsPartTwo()
    {
        return _partTwo;
    }

    /// <summary>
    /// Gets the list of dialogue trees for part two.
    /// </summary>
    /// <returns>A list of DialogueTree objects.</returns>
    public List<DialogueTree> GetDialogueTrees()
    {
        return _dialogueTrees;
    }
}

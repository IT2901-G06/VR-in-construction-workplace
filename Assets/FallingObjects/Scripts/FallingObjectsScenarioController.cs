using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingObjectsScenarioController : MonoBehaviour
{
    [SerializeField]
    private bool _partTwo;

    [SerializeField]
    private List<DialogueTree> _dialogueTrees;

    public static FallingObjectsScenarioController Instance;
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

    public void GoToPartTwo()
    {
        _partTwo = true;
        SceneManager.LoadScene("FallingObjectsScenario");
    }

    public bool GetPartTwo()
    {
        return _partTwo;
    }

    public List<DialogueTree> GetDialogueTrees()
    {
        return _dialogueTrees;
    }
}

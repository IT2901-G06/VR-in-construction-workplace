using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingObjectsScenarioManager : MonoBehaviour
{
    [SerializeField]
    private bool _partTwo;

    [SerializeField]
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

    public void GoToPartTwo()
    {
        _partTwo = true;
        Destroy(GameObject.Find("OVRCameraRig"));
        SceneManager.LoadScene("FallingObjectsScenario");
    }

    public bool IsPartTwo()
    {
        return _partTwo;
    }

    public List<DialogueTree> GetDialogueTrees()
    {
        return _dialogueTrees;
    }
}

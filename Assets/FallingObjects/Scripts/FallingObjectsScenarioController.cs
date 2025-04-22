using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingObjectsScenarioController : MonoBehaviour
{
    [SerializeField]
    private bool _partTwo;

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
        _partTwo = false;
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
}

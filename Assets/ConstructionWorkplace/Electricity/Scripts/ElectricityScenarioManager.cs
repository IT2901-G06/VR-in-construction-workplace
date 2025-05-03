using UnityEngine;
using UnityEngine.SceneManagement;

public class ElectricityScenarioManager : MonoBehaviour
{
    [SerializeField]
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

    public void GoToPartTwo()
    {
        _partTwo = true;
        Destroy(GameObject.Find("OVRCameraRig"));
        SceneManager.LoadScene("Electricity");
    }

    public bool IsPartTwo()
    {
        return _partTwo;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A misc script to load scenes and destroy the OVRCameraRig in the previous scene.
/// </summary>
public class SceneManagerScript : MonoBehaviour
{
    /// <summary>
    /// Loads a scene by name. Will also destroy the OVRCameraRig in the previous
    /// scene to prevent two OVRCameraRigs from being active at the same time.
    /// </summary>
    /// <param name="sceneName"> The name of the scene to load.</param>
    public void LoadScene(string sceneName)
    {
        // Destroy the OVRCameraRig in the previous scene to prevent two OVRCameraRigs
        // from being active at the same time.
        Destroy(GameObject.Find("OVRCameraRig"));

        Debug.Log("Loading Scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}

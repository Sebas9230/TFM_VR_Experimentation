using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string sceneName;
    public string objectToActivateInScene;    // Ej: "V1_Basic_Version"
    public string controllerToActivateInScene; // Ej: "Right Controller"

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            PlayerPrefs.SetString("ObjectToActivate", objectToActivateInScene);
            PlayerPrefs.SetString("ControllerToActivate", controllerToActivateInScene);
            PlayerPrefs.Save();

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}

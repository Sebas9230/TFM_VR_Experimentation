using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string sceneName;
    public string objectToActivateInScene;    // Ex: "V1_Basic_Version"
    public string controllerToActivateInScene; // Ex: "Right Controller"

    // Loads scene and sets activation preferences
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

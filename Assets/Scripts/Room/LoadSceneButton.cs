using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string sceneName;
    public string objectToActivateInScene; // Por ejemplo: "V1", "V2", "V3"

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName) && !string.IsNullOrEmpty(objectToActivateInScene))
        {
            // Guardar nombre del objeto que debe activarse
            PlayerPrefs.SetString("ObjectToActivate", objectToActivateInScene);
            PlayerPrefs.Save();

            SceneManager.LoadScene(sceneName);
        }
    }
}

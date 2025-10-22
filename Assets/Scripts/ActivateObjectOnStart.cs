using UnityEngine;

public class ActivateObjectOnStart : MonoBehaviour
{
    // Activates specific objects and controllers based on PlayerPrefs settings
    void Start()
    {
        string objectName = PlayerPrefs.GetString("ObjectToActivate", "");
        string controllerName = PlayerPrefs.GetString("ControllerToActivate", "");

        // Activate the general object (like V1_Basic_Version)
        if (!string.IsNullOrEmpty(objectName))
        {
            GameObject objToActivate = FindInScene(objectName);
            if (objToActivate != null)
            {
                objToActivate.SetActive(true);
                Debug.Log($"Activated object: {objectName}");
            }
            else
            {
                Debug.LogWarning($"Object not found: {objectName}");
            }
        }

        // Activate specifically the controller, like Right Controller
        if (!string.IsNullOrEmpty(controllerName))
        {
            GameObject controllerToActivate = FindInScene(controllerName);
            if (controllerToActivate != null)
            {
                controllerToActivate.SetActive(true);
                Debug.Log($"Activated controller: {controllerName}");
            }
            else
            {
                Debug.LogWarning($"Controller not found: {controllerName}");
            }
        }
    }

    // Searches for a GameObject by name in the current scene
    GameObject FindInScene(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name && obj.transform.hideFlags == HideFlags.None && obj.scene.isLoaded)
            {
                return obj;
            }
        }

        return null;
    }
}

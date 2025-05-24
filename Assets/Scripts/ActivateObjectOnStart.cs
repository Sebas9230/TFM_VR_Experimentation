using UnityEngine;

public class ActivateObjectOnStart : MonoBehaviour
{
    void Start()
    {
        string objectName = PlayerPrefs.GetString("ObjectToActivate", "");
        string controllerName = PlayerPrefs.GetString("ControllerToActivate", "");

        // Activar el objeto general (como V1_Basic_Version)
        if (!string.IsNullOrEmpty(objectName))
        {
            GameObject objToActivate = FindInScene(objectName);
            if (objToActivate != null)
            {
                objToActivate.SetActive(true);
                Debug.Log($"Activado objeto: {objectName}");
            }
            else
            {
                Debug.LogWarning($"No se encontró el objeto: {objectName}");
            }
        }

        // Activar específicamente el controlador, como Right Controller
        if (!string.IsNullOrEmpty(controllerName))
        {
            GameObject controllerToActivate = FindInScene(controllerName);
            if (controllerToActivate != null)
            {
                controllerToActivate.SetActive(true);
                Debug.Log($"Activado controlador: {controllerName}");
            }
            else
            {
                Debug.LogWarning($"No se encontró el controlador: {controllerName}");
            }
        }
    }

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

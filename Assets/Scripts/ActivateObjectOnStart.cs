using UnityEngine;

public class ActivateObjectOnStart : MonoBehaviour
{
    void Start()
    {
        string objectName = PlayerPrefs.GetString("ObjectToActivate", "");

        if (!string.IsNullOrEmpty(objectName))
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.name == objectName && obj.scene.isLoaded)
                {
                    obj.SetActive(true);
                    Debug.Log($"Activado: {objectName}");
                    return;
                }
            }

            Debug.LogWarning($"No se encontró ningún GameObject (aunque esté desactivado) con el nombre: {objectName}");
        }
    }
}

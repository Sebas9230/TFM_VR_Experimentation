using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class RoomTeleport : MonoBehaviour
{
    public string sceneToLoad = "";
    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        // Asegurarse de que el objeto que entra es el XR Rig o un Controlador VR
        if ((other.CompareTag("Player") || other.GetComponent<XRController>()) && !isTeleporting)
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                StartCoroutine(TeleportAfterDelay(1f)); // Espera 1 segundo antes de teletransportar
            }
        }
    }

    private IEnumerator TeleportAfterDelay(float delay)
    {
        isTeleporting = true;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
}

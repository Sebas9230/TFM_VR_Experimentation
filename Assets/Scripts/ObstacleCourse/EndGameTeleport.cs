using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class EndGameTeleport : MonoBehaviour
{
    public string sceneToLoad = "";
    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        // Asegurarse de que el objeto que entra es el XR Rig o un Controlador VR
        if ((other.CompareTag("Player") || other.GetComponent<XRController>()) && !isTeleporting)
        {
            // Detener cronómetro y registrar finalización
            Cronometer cronometer = FindObjectOfType<Cronometer>();
            if (cronometer != null)
            {
                cronometer.DetenerCronometro();
            }
            
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

    if (SceneTracker.Instance != null)
    {
        SceneTracker.Instance.PreviousScene = SceneManager.GetActiveScene().name;
    }

    SceneManager.LoadScene(sceneToLoad);
}

}

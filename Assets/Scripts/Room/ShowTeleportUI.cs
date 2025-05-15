using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowTeleportUI : MonoBehaviour
{
    public GameObject uiTeleportA;
    public GameObject uiTeleportB;
    public GameObject uiTeleportC;

    public enum TeleportType { A, B, C }
    public TeleportType teleportType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<XRController>())
        {
            // Oculta todos primero
            if (uiTeleportA) uiTeleportA.SetActive(false);
            if (uiTeleportB) uiTeleportB.SetActive(false);
            if (uiTeleportC) uiTeleportC.SetActive(false);

            // Muestra el canvas correspondiente
            switch (teleportType)
            {
                case TeleportType.A:
                    if (uiTeleportA) uiTeleportA.SetActive(true);
                    break;
                case TeleportType.B:
                    if (uiTeleportB) uiTeleportB.SetActive(true);
                    break;
                case TeleportType.C:
                    if (uiTeleportC) uiTeleportC.SetActive(true);
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<XRController>())
        {
            // Oculta solo el UI correspondiente al tipo de teleport
            switch (teleportType)
            {
                case TeleportType.A:
                    if (uiTeleportA) uiTeleportA.SetActive(false);
                    break;
                case TeleportType.B:
                    if (uiTeleportB) uiTeleportB.SetActive(false);
                    break;
                case TeleportType.C:
                    if (uiTeleportC) uiTeleportC.SetActive(false);
                    break;
            }
        }
    }
}

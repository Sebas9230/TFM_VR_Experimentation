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
            // Hide all first
            if (uiTeleportA) uiTeleportA.SetActive(false);
            if (uiTeleportB) uiTeleportB.SetActive(false);
            if (uiTeleportC) uiTeleportC.SetActive(false);

            // Show the corresponding canvas
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
            // Hide only the UI corresponding to the teleport type
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

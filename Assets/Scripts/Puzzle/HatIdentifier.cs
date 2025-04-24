using UnityEngine;


public enum HatType
{
    CowboyHat,
    Crown,
    MagicianHat,
    MinerHat,
    Mirror,
    Mustache,
    PajamaHat,
    PillboxHat,
    PoliceCap,
    ShowerCap,
    Sombrero,
    VikingHelmet
}

public class HatIdentifier : MonoBehaviour
{
    public HatType hatType;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        }

        // Configure XR Grab Interactable settings
        grabInteractable.throwOnDetach = false;
        grabInteractable.smoothPosition = true;
        grabInteractable.smoothRotation = true;
        grabInteractable.smoothPositionAmount = 5f;
        grabInteractable.smoothRotationAmount = 5f;
    }
} 
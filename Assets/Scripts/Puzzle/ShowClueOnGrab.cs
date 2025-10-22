using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowClueOnGrab : MonoBehaviour
{
    [Tooltip("The clue corresponding to this hat")]
    public GameObject clueToShow;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    // Initializes grab interaction listeners
    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    // Cleans up listeners on destruction
    private void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
    }

    // Shows clue when hat is grabbed
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        HideAllClues();

        if (clueToShow != null)
            clueToShow.SetActive(true);
    }

    // Hides clue when hat is released
    private void OnReleased(SelectExitEventArgs args)
    {
        if (clueToShow != null)
            clueToShow.SetActive(false);
    }

    // Hides all clue panels
    private void HideAllClues()
    {
        GameObject cluesParent = GameObject.Find("CluesPanels");
        if (cluesParent == null) return;

        foreach (Transform clue in cluesParent.transform)
            clue.gameObject.SetActive(false);
    }
}

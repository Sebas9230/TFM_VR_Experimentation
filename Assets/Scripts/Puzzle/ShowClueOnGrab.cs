using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowClueOnGrab : MonoBehaviour
{
    [Tooltip("El Clue correspondiente a este sombrero")]
    public GameObject clueToShow;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    private void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        HideAllClues();

        if (clueToShow != null)
            clueToShow.SetActive(true);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (clueToShow != null)
            clueToShow.SetActive(false);
    }

    private void HideAllClues()
    {
        GameObject cluesParent = GameObject.Find("CluesPanels");
        if (cluesParent == null) return;

        foreach (Transform clue in cluesParent.transform)
            clue.gameObject.SetActive(false);
    }
}

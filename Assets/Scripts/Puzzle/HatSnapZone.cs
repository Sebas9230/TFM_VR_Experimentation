using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HatSnapZone : UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor
{
    public string expectedCharacterName; // e.g., "diego", "alice", etc.
    public HatIdentifier CurrentHat { get; private set; }
    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    protected override void Awake()
    {
        base.Awake();
        
        // Configure socket settings
        socketActive = true;
        interactionLayers = InteractionLayerMask.GetMask("Default");
        
        // Save original transform data
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;

        // Get the expected character name from the GameObject name
        if (gameObject.name.StartsWith("stand_"))
        {
            expectedCharacterName = gameObject.name.Substring(6).ToLower(); // removes "stand_" prefix
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        
        if (args.interactableObject.transform.TryGetComponent(out HatIdentifier hat))
        {
            CurrentHat = hat;
            
            // Snap the hat to the correct position
            hat.transform.SetParent(transform);
            hat.transform.localPosition = Vector3.zero;
            hat.transform.localRotation = Quaternion.identity;

            // Notify the LogicPuzzleManager
            LogicPuzzleManager.Instance?.CheckSolution();
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        
        if (args.interactableObject.transform.TryGetComponent(out HatIdentifier hat))
        {
            if (hat == CurrentHat)
            {
                CurrentHat = null;
                // Notify the LogicPuzzleManager when a hat is removed
                LogicPuzzleManager.Instance?.CheckSolution();
            }
        }
    }

    public override bool CanSelect(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
    {
        // Only allow selecting hat objects
        return base.CanSelect(interactable) && interactable.transform.GetComponent<HatIdentifier>() != null;
    }

    private void Reset()
    {
        // Reset to original position if needed
        if (originalParent != null)
        {
            transform.SetParent(originalParent);
            transform.localPosition = originalLocalPosition;
            transform.localRotation = originalLocalRotation;
        }
    }
} 
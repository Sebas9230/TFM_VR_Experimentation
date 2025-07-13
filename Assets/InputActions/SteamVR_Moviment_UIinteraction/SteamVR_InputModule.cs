using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

[AddComponentMenu("SteamVR/SteamVR Input Module")]
public class SteamVR_InputModule : BaseInputModule
{
    public SteamVR_Behaviour_Pose leftPointer;
    public SteamVR_Behaviour_Pose rightPointer;
    public SteamVR_Action_Boolean clickAction = SteamVR_Input.GetBooleanAction("InteractUI");

    public float clickThreshold = 0.1f;
    public float clickTime = 0.1f;

    private PointerEventData leftPointerEvent;
    private PointerEventData rightPointerEvent;

    private GameObject leftCurrentObject = null;
    private GameObject rightCurrentObject = null;

    private void ProcessPointer(SteamVR_Behaviour_Pose pointer, ref PointerEventData pointerEvent, ref GameObject currentObject)
    {
        if (pointer == null || clickAction == null)
            return;

        if (pointerEvent == null)
            pointerEvent = new PointerEventData(eventSystem);

        pointerEvent.Reset();
        pointerEvent.position = new Vector2(Screen.width / 2, Screen.height / 2);

        eventSystem.RaycastAll(pointerEvent, m_RaycastResultCache);
        pointerEvent.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        currentObject = pointerEvent.pointerCurrentRaycast.gameObject;
        m_RaycastResultCache.Clear();

        HandlePointerExitAndEnter(pointerEvent, currentObject);

        if (clickAction.GetStateDown(pointer.inputSource))
        {
            pointerEvent.pressPosition = pointerEvent.position;
            pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

            GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(currentObject, pointerEvent, ExecuteEvents.pointerDownHandler);

            if (newPointerPress == null)
                newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

            pointerEvent.pointerPress = newPointerPress;
            pointerEvent.rawPointerPress = currentObject;
        }

        if (clickAction.GetStateUp(pointer.inputSource))
        {
            ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

            GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

            if (pointerEvent.pointerPress == pointerUpHandler)
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);

            eventSystem.SetSelectedGameObject(null);

            pointerEvent.pressPosition = Vector2.zero;
            pointerEvent.pointerPress = null;
            pointerEvent.rawPointerPress = null;
        }
    }

    public override void Process()
    {
        ProcessPointer(leftPointer, ref leftPointerEvent, ref leftCurrentObject);
        ProcessPointer(rightPointer, ref rightPointerEvent, ref rightCurrentObject);
    }
}

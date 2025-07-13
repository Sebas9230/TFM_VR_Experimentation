using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(CharacterController))]
public class SteamVR_CameraRigMovement : MonoBehaviour
{
    public SteamVR_Action_Vector2 inputMove = SteamVR_Actions.default_Touchpad;
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.RightHand;
    public float speed = 2.0f;

    public Transform cameraRig;   // El objeto "CameraRig"
    public Transform head;        // El objeto "Camera" dentro del rig

    private CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();

        if (cameraRig == null)
            Debug.LogError("CameraRig no asignado");

        if (head == null)
            Debug.LogError("Head (cámara) no asignado");
    }

    void Update()
    {
        Vector2 input = inputMove.GetAxis(inputSource);

        // Dirección en la que mira el usuario (solo en plano horizontal)
        Vector3 direction = new Vector3(input.x, 0, input.y);
        Vector3 headYaw = new Vector3(head.forward.x, 0, head.forward.z).normalized;
        Quaternion rotation = Quaternion.LookRotation(headYaw);
        Vector3 move = rotation * direction;

        Debug.Log("Input: " + inputMove.GetAxis(inputSource));

        character.Move(move * speed * Time.deltaTime);
    }

    void LateUpdate()
    {
        // Sincronizar el collider con la posición real de la cabeza
        Vector3 headPositionLocal = cameraRig.InverseTransformPoint(head.position);
        character.center = new Vector3(headPositionLocal.x, character.height / 2, headPositionLocal.z);
    }
}

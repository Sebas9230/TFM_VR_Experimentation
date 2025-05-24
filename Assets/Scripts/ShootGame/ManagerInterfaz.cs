using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfazManager : MonoBehaviour
{
    public Transform cabeza;
    public float distanciaInterfaz = 100;
    public GameObject panelInterfazT;
    public GameObject panelInterfazP;
    public int rotacionP = 175;
    public int rotacionT = 190;
    void Update() {
        panelInterfazT.transform.position = cabeza.position + new Vector3(cabeza.forward.x, cabeza.forward.y, cabeza.forward.z).normalized * distanciaInterfaz;
        panelInterfazT.transform.LookAt(new Vector3(-cabeza.position.x, cabeza.position.y, cabeza.position.z)); 
        panelInterfazT.transform.Rotate(0, rotacionT, 0);

        panelInterfazP.transform.position = cabeza.position + new Vector3(cabeza.forward.x, cabeza.forward.y, cabeza.forward.z).normalized * distanciaInterfaz;
        panelInterfazP.transform.LookAt(new Vector3(-cabeza.position.x, cabeza.position.y, cabeza.position.z));
        panelInterfazP.transform.Rotate(0, rotacionP, 0);
    }
}

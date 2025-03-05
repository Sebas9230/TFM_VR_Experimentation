using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncionamientoBalas : MonoBehaviour
{
    public float vida;
    public float nacimiento;
 
    void OnEnable() {
        nacimiento = Time.time; // guardo el tiempo de nacimiento de la bala
    }
    void Update() {
        if(Time.time > nacimiento + vida) { // si el tiempo actual es mayor al tiempo de nacimiento más el tiempo de vida
            gameObject.SetActive(false); // desactivo la bala
        }
    }
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Disparable")) {
            ObjetivosManager.Instance.fuera = true;
            ObjetivosManager.Instance.Despawn();
            Debug.Log("Objetivo destruido");
            ObjetivosManager.Instance.fuera = false;
            gameObject.SetActive(false); 
            Debug.Log("Bala destruida");
            ObjetivosManager.Instance.puntos ++;
            Debug.Log("Puntos: " + ObjetivosManager.Instance.puntos);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMirilla : MonoBehaviour
{
    public LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.positionCount = 2;
    }
    void Update()
    {
       if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 20f))
        {
            if(hit.collider.CompareTag("Disparable")) {
                // Debug.Log("Dsiparable");
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.magenta);
                // lineRenderer.startColor = Color.magenta;
                // lineRenderer.endColor = Color.magenta;
                lineRenderer.material.color = Color.magenta;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
            } else if(hit.collider) {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
                // lineRenderer.startColor = Color.yellow;
                // lineRenderer.endColor = Color.yellow;
                lineRenderer.material.color = Color.yellow;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
            }
        } else {
            Debug.DrawRay(transform.position, transform.forward * 20f, Color.green);
            // lineRenderer.startColor = Color.green;
            // lineRenderer.endColor = Color.green;
            lineRenderer.material.color = Color.green;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * 20f);
        }
    }
}

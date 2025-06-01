using UnityEngine;

[System.Serializable]
public class NoSlipPhysicMaterial : MonoBehaviour
{
    [Header("Material de Fricción")]
    public float staticFriction = 1.0f;
    public float dynamicFriction = 1.0f;
    public float bounciness = 0.0f;
    public PhysicMaterialCombine frictionCombine = PhysicMaterialCombine.Maximum;
    public PhysicMaterialCombine bounceCombine = PhysicMaterialCombine.Minimum;

    private PhysicMaterial noSlipMaterial;

    void Start()
    {
        CreateAndApplyPhysicMaterial();
    }

    void CreateAndApplyPhysicMaterial()
    {
        // Crear el material físico con alta fricción
        noSlipMaterial = new PhysicMaterial("NoSlipMaterial");
        noSlipMaterial.staticFriction = staticFriction;
        noSlipMaterial.dynamicFriction = dynamicFriction;
        noSlipMaterial.bounciness = bounciness;
        noSlipMaterial.frictionCombine = frictionCombine;
        noSlipMaterial.bounceCombine = bounceCombine;

        // Aplicar el material a todos los colliders del objeto
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.material = noSlipMaterial;
            Debug.Log($"[NoSlip] Material aplicado a collider: {col.name}");
        }

        // También aplicar a colliders hijos si es necesario
        Collider[] childColliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in childColliders)
        {
            if (col.gameObject != gameObject) // Evitar duplicar en el objeto padre
            {
                col.material = noSlipMaterial;
                Debug.Log($"[NoSlip] Material aplicado a collider hijo: {col.name}");
            }
        }
    }

    // Función para ajustar la fricción en tiempo real
    public void SetFriction(float staticFric, float dynamicFric)
    {
        if (noSlipMaterial != null)
        {
            staticFriction = staticFric;
            dynamicFriction = dynamicFric;
            noSlipMaterial.staticFriction = staticFriction;
            noSlipMaterial.dynamicFriction = dynamicFriction;
        }
    }
} 
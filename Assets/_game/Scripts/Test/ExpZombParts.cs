using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpZombParts : MonoBehaviour
{
    public float forceMagnitude = 10f; // Магнитуда силы
    public Vector3 forceDirection = Vector3.up; // Направление силы (по умолчанию вверх)
    public ForceMode forceMode = ForceMode.Force; // Режим применения силы

    private void OnEnable()
    {
        ApplyForceToAllChildren();
    }

    private void ApplyForceToAllChildren()
    {
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Применяем силу к дочернему объекту
                rb.AddForce(forceDirection * forceMagnitude, forceMode);
            }
        }
    }
}
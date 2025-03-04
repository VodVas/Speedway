using UnityEngine;

public class SetChildRigidbodiesKinematic : MonoBehaviour
{
    public void EnableKinematic() => SetKinematicState(true);
    public void DisableKinematic() => SetKinematicState(false);

    private void SetKinematicState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = state;
        }

        Debug.Log($"Set isKinematic to {state} on {rigidbodies.Length} Rigidbodies");
    }
}
using UnityEngine;

public class ZombieRagdoll : MonoBehaviour
{
    [SerializeField] private Collider _playerCollider;
    [SerializeField] private Rigidbody[] _ragdollRigidbodies;
    [SerializeField] private Collider[] _ragdollColliders;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _forceMultiplier = 25f;
    [SerializeField] private float _upwardForce = 0.5f;
    [SerializeField, Range(0.1f, 10f)] private float _ragdollMass = 1f; // Масса с диапазоном

    private bool _isRagdollActive = false;

    private void Awake()
    {
        _ragdollRigidbodies = _ragdollRigidbodies?.Length > 0 ? _ragdollRigidbodies : GetComponentsInChildren<Rigidbody>();
        _ragdollColliders = _ragdollColliders?.Length > 0 ? _ragdollColliders : GetComponentsInChildren<Collider>();
        _animator = _animator ? _animator : GetComponent<Animator>();

        SetupRagdollRigidbodies();
    }

    private void SetupRagdollRigidbodies()
    {
        for (int i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            Rigidbody rb = _ragdollRigidbodies[i];
            rb.mass = _ragdollMass;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("OnTriggerEnter");

        if (!_isRagdollActive && other == _playerCollider)
        {
            ActivateRagdoll(other);
            Debug.LogWarning("!_isRagdollActive && other == _playerCollider");
        }
    }

    private void ActivateRagdoll(Collider carCollider)
    {
        _animator.enabled = false;
        _isRagdollActive = true;

        Vector3 forceDirection = (transform.position - carCollider.transform.position).normalized;
        forceDirection.y += _upwardForce;

        SetRagdollKinematic(false);
        ApplyForceToRagdoll(forceDirection);
    }

    private void SetRagdollKinematic(bool isKinematic)
    {
        for (int i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            Rigidbody rb = _ragdollRigidbodies[i];
            rb.isKinematic = isKinematic;
            rb.useGravity = !isKinematic;
        }
    }

    private void ApplyForceToRagdoll(Vector3 forceDirection)
    {
        for (int i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            _ragdollRigidbodies[i].AddForce(forceDirection * _forceMultiplier, ForceMode.Impulse);
        }
    }
}
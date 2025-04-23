using UnityEngine;
using VodVas.InterfaceSerializer;

public class InterfaceSerializationExample : MonoBehaviour
{
    [SerializeField, InterfaceConstraint(typeof(IDamageable))]
    private MonoBehaviour _damageable;

    private IDamageable Damageable => _damageable as IDamageable;
}
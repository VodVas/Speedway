using UnityEngine;

public class MineExplosionMarker : MonoBehaviour
{
    [field: SerializeField, Range(0, 100)] public float Damage { get; private set; } = 50f;
}
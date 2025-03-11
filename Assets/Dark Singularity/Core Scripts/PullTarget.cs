using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[System.Serializable]
public class PullTarget
{
    [Tooltip("Ссылка на Transform объекта, который хотим тянуть.")]
    [SerializeField] private Transform _targetTransform;

    [Tooltip("Ссылка на Rigidbody объекта, который хотим тянуть.")]
    [SerializeField] private Rigidbody _targetRigidbody;

    [Tooltip("Если объект нужно тянуть, ставим true. Иначе пропускаем в цикле.")]
    [SerializeField] private bool _pullable = true;

    [Header("Cache static mass (optional)")]
    [Tooltip("Если масса объекта никогда не изменится — можно скопировать сюда один раз.")]
    [SerializeField] private float _cachedMass = 1f;

    /// <summary>
    /// Возвращает Transform объекта.
    /// </summary>
    public Transform TargetTransform
    {
        get { return _targetTransform; }
    }

    /// <summary>
    /// Возвращает Rigidbody объекта.
    /// </summary>
    public Rigidbody TargetRigidbody
    {
        get { return _targetRigidbody; }
    }

    /// <summary>
    /// Возвращает true, если объект нужно притягивать.
    /// </summary>
    public bool Pullable
    {
        get { return _pullable; }
        set { _pullable = value; }
    }

    /// <summary>
    /// Кэшированная масса для скорости. Если массу надо менять — можно брать из TargetRigidbody.mass.
    /// </summary>
    public float CachedMass
    {
        get { return _cachedMass; }
    }

#if UNITY_EDITOR
    // В редакторе при изменении ссылок мы можем обновлять _cachedMass
    // если хотим хранить текущую массу rigidbody. Иначе можно не делать.
    public void OnValidate()
    {
        if (_targetRigidbody != null)
        {
            _cachedMass = _targetRigidbody.mass;
        }
    }
#endif
}





//public class PullTarget : MonoBehaviour
//{
//    public bool IsPullable { get; private set; } = true;
//}
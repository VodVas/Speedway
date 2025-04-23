using System;
using UnityEngine;

[Serializable]
public sealed class PullTarget
{
    [SerializeField] private Rigidbody _targetRigidbody;
    [SerializeField] private bool _pullable = true;
    [SerializeField] private float _cachedMass = 1f;

    public Rigidbody GetRigidbody() => _targetRigidbody;
    public bool IsPullable() => _pullable;
    public float GetCachedMass() => _cachedMass;

    public void SetRigidbody(Rigidbody rb) => _targetRigidbody = rb;
    public void SetPullable(bool state) => _pullable = state;
    public void SetCachedMass(float mass) => _cachedMass = Mathf.Max(mass, 0.01f);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_targetRigidbody != null && _targetRigidbody.mass > 0)
        {
            _cachedMass = _targetRigidbody.mass;
        }
    }
#endif
}


/*[Serializable]
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

    //public Transform TargetTransform
    //{
    //    get { return _targetTransform; }
    //}

    public Rigidbody TargetRigidbody
    {
        get { return _targetRigidbody; }
    }

    public bool Pullable
    {
        get { return _pullable; }
        set { _pullable = value; }
    }

    public float CachedMass
    {
        get { return _cachedMass; }
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (_targetRigidbody != null)
        {
            _cachedMass = _targetRigidbody.mass;
        }
    }
#endif
}*/
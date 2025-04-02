using System;
using UnityEngine;

public class LootPaintSphere : MonoBehaviour, ITerminatable
{
    public event Action<ITerminatable> Terminated;

    private Renderer _cachedRenderer;
    private bool _initialized;
    private int _paintId; // Добавляем поле для хранения ID

    private void Awake()
    {
        _cachedRenderer = GetComponent<Renderer>();
        _initialized = true;
    }

    // Новый метод для установки ID краски
    public void SetPaintId(int id)
    {
        _paintId = id;
    }

    public int GetPaintId() => _paintId;


    public void SetMaterial(Material mat)
    {
        if (!_initialized || _cachedRenderer == null)
        {
            return;
        }

        _cachedRenderer.sharedMaterial = mat;
    }

    public void TerminateLoot()
    {
        Terminated?.Invoke(this);
    }
}
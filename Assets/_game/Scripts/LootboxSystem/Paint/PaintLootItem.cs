using System;
using UnityEngine;

[Serializable]
public sealed class PaintLootItem
{
    [SerializeField] private int _paintId;
    [SerializeField] private Material _paintMaterial;

    public int PaintId => _paintId;
    public Material PaintMaterial => _paintMaterial;
}
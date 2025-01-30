using System;
using UnityEngine;

public interface IFactory<T> where T : MonoBehaviour
{
    T Create(Type type, Vector3 position);
}
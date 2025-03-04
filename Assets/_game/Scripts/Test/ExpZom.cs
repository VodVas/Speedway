using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpZom : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (player != null)
        {
            Instantiate(_gameObject, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
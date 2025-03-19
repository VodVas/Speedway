using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaysoundpartTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;

    private void OnParticleTrigger()
    {
        Debug.Log("OnParticleTrigger");
        _audio.Play();
    }
}
using UnityEngine;
using YG;

public class OnlyDesktopParticlePlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private bool _isPlay;

    private void Awake()
    {
        if (YandexGame.EnvironmentData.isMobile && _isPlay)
        {
            _particleSystem.Play();
        }
    }
}
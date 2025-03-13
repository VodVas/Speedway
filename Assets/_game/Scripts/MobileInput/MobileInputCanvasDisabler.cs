using UnityEngine;
using YG;

public class MobileInputCanvasDisabler : MonoBehaviour
{
    [SerializeField] private GameObject _mobileControlCanvas;

    private void Start()
    {
        if (YandexGame.EnvironmentData.isDesktop)
        {
            _mobileControlCanvas.gameObject.SetActive(false);
        }
    }
}
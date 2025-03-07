using UnityEngine;
using YG;

public class MobileInputCanvasDisabler : MonoBehaviour
{
    [SerializeField] private GameObject _controlButtonCanvas;

    private void Start()
    {
        if (YandexGame.EnvironmentData.isMobile)
        {
            _controlButtonCanvas.gameObject.SetActive(false);
        }
    }
}
using UnityEngine;
using YG;

public class DesktopMobileCanvasSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _mobileCanvas;
    [SerializeField] private GameObject _desktopCanvas;

    private void Start()
    {
        if (YandexGame.EnvironmentData.isMobile)
        {
            _mobileCanvas.gameObject.SetActive(true);
        }
        else if(YandexGame.EnvironmentData.isDesktop)
        {
            _desktopCanvas.gameObject.SetActive(true);
        }
    }
}
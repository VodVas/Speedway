using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedDisplay : MonoBehaviour
{
    private const float METERS_PER_SECOND_TO_KMH = 3.6f;
    private const float MAX_SPEED_KMH = 300f;

    [SerializeField] private Rigidbody _targetRigidbody = null;
    [SerializeField] private TextMeshProUGUI _speedText = null;
    [SerializeField] private Image _speedFillImage = null;

    private void Awake()
    {
        if (_targetRigidbody == null || _speedText == null || _speedFillImage == null)
        {
            Debug.LogError("[SpeedDisplay] —сылки не назначены, скрипт отключЄн.");
            enabled = false;
        }
    }

    private void Update()
    {
        float velocityMPS = _targetRigidbody.velocity.magnitude;
        float velocityKMH = velocityMPS * METERS_PER_SECOND_TO_KMH;

        _speedText.text = ((int)velocityKMH / 2).ToString();

        float fillAmount = Mathf.Clamp((velocityKMH / MAX_SPEED_KMH) / 2, 0f, 1f);
        _speedFillImage.fillAmount = fillAmount;
    }
}
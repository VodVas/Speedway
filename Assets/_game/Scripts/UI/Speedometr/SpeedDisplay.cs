using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class SpeedDisplay : MonoBehaviour
{
    private const float MPS_TO_KMH = 3.6f;
    private const float MAX_SPEED_KMH = 300f;
    private const float DIVIDER = 2f; // ������ "2" ���������� ���������

    [SerializeField] private TextMeshProUGUI _speedText = null;
    [SerializeField] private Image _speedFillImage = null;

    private Rigidbody _targetRigidbody = null;

    private void Awake()
    {
        if (_speedText == null || _speedFillImage == null)
        {
            Debug.LogError("[SpeedDisplay] �� ��� UI-������ ���������!", this);
            enabled = false;
            return;
        }
    }

    /// <summary>
    /// ��������� Rigidbody, � �������� ���� ��������.
    /// </summary>
    /// <param name="newTarget">����� Rigidbody (�� null)</param>
    public void AssignTargetRigidbody(Rigidbody newTarget)
    {
        if (newTarget == null)
        {
            Debug.LogError("[SpeedDisplay] �������� null � AssignTargetRigidbody!", this);
            enabled = false;
            return;
        }
        _targetRigidbody = newTarget;
        enabled = true;
    }

    private void Update()
    {
        if (_targetRigidbody == null)
        {
            return; // ��� ������� � �� ���������
        }

        float velocityMPS = _targetRigidbody.velocity.magnitude;
        float velocityKMH = velocityMPS * MPS_TO_KMH;

        // ����� �� 2, ��� � ��� � ����: (int)velocityKMH / 2
        float displayValue = velocityKMH / DIVIDER;
        _speedText.text = ((int)displayValue).ToString();

        float fillAmount = Mathf.Clamp(displayValue / MAX_SPEED_KMH, 0f, 1f);
        _speedFillImage.fillAmount = fillAmount;
    }
}











//public class SpeedDisplay : MonoBehaviour
//{
//    private const float METERS_PER_SECOND_TO_KMH = 3.6f;
//    private const float MAX_SPEED_KMH = 300f;

//    [SerializeField] private Rigidbody _targetRigidbody = null;
//    [SerializeField] private TextMeshProUGUI _speedText = null;
//    [SerializeField] private Image _speedFillImage = null;

//    private void Awake()
//    {
//        if (_targetRigidbody == null || _speedText == null || _speedFillImage == null)
//        {
//            Debug.LogError("[SpeedDisplay] ������ �� ���������, ������ ��������.");
//            enabled = false;
//        }
//    }

//    private void Update()
//    {
//        float velocityMPS = _targetRigidbody.velocity.magnitude;
//        float velocityKMH = velocityMPS * METERS_PER_SECOND_TO_KMH;

//        _speedText.text = ((int)velocityKMH / 2).ToString();

//        float fillAmount = Mathf.Clamp((velocityKMH / MAX_SPEED_KMH) / 2, 0f, 1f);
//        _speedFillImage.fillAmount = fillAmount;
//    }
//}
using ArcadeVP;
using UnityEngine;

public class PlayerProgressTracker : MonoBehaviour
{
    [SerializeField] private WaypointCircuit _circuit;
    // ����� ����� ������ WaypointCircuit ����� float _trackLength = 1000f; 
    // �� �����, ����� ���� �� �� "�����" ��� � � AI.

    private int _currentLap;
    public int CurrentLap => _currentLap;

    private float _progressDistance;
    public float ProgressDistance => _progressDistance;

    private bool _isFinished;
    public bool IsFinished => _isFinished;

    private Vector3 _lastPosition;

    private void Start()
    {
        _currentLap = 0;
        _progressDistance = 0f;
        _lastPosition = transform.position;
    }

    private void Update()
    {
        if (_isFinished || _circuit == null)
            return;

        // �������, ����� ��������� ������ �� ����
        float delta = Vector3.Distance(transform.position, _lastPosition);
        _progressDistance += delta;
        _lastPosition = transform.position;

        // ���� ��������� ����� ����� * (����������� + 1), ������ ���� ��������
        if (_progressDistance >= _circuit.Length * (_currentLap + 1))
        {
            _currentLap++;
        }
    }

    public void MarkAsFinished()
    {
        _isFinished = true;
    }
}
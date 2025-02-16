using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private int _checkpointIndex = 0;
    [SerializeField] private RaceProgressTracker _raceFlow = null;

    private const string ErrorRaceManagerNull = "CheckpointTrigger: RaceManager is empty";
    private const string ErrorCheckpointIndexInvalid = "CheckpointTrigger: CheckpointIndex must be grater than zero.";

    private void Awake()
    {
        ValidateData();
    }

    private void OnTriggerEnter(Collider other)
    {
        Racer racer = other.GetComponent<Racer>();

        if (racer == null)
        {
            return;
        }

        _raceFlow?.HandleTriggerEnter(racer, _checkpointIndex);
    }

    private void ValidateData()
    {
        if (_raceFlow == null)
        {
            Debug.LogError(ErrorRaceManagerNull, this);
            enabled = false;
            return;
        }

        if (_checkpointIndex < 0)
        {
            Debug.LogError(ErrorCheckpointIndexInvalid, this);
            enabled = false;
        }
    }
}
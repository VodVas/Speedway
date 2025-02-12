using UnityEngine;

public class Racer : MonoBehaviour
{
    private const string ErrorRacerIdInvalid = "Racer: RacerId must be greater than zero.";
    private const string ErrorNewPositionInvalid = "Racer: Position must be greater than zero.";
    private const string ErrorCheckpointIndexInvalid = "Racer: CheckpointIndex must be greater than zero.";

    [field: SerializeField] public int RacerId { get; set; } = 0;

    private int _position = 0;
    private int _lastCheckpoint = -1;
    private int _previousPosition = 0;
    private int _lapsCompleted = 0;
    private bool _hasFinished = false;

    private void Awake()
    {
        ValidateData();
    }

    //public int RacerId => _racerId;
    public int Position => _position;
    public int LastCheckpoint => _lastCheckpoint;
    public int PreviousPosition => _previousPosition;
    public int LapsCompleted => _lapsCompleted;

    public bool HasFinished => _hasFinished;

    public void SetPosition(int newPosition)
    {
        if (newPosition < 1)
        {
            Debug.LogError(ErrorNewPositionInvalid, this);
            enabled = false;
            return;
        }

        _position = newPosition;
    }

    public void UpdateLastCheckpoint(int checkpointIndex)
    {
        if (checkpointIndex < 0)
        {
            Debug.LogError(ErrorCheckpointIndexInvalid, this);
            enabled = false;
            return;
        }

        if (checkpointIndex > _lastCheckpoint)
        {
            _lastCheckpoint = checkpointIndex;
        }
    }

    public void UpdatePreviousPosition()
    {
        _previousPosition = _position;
    }

    public void CompleteLap()
    {
        _lapsCompleted++;
        _lastCheckpoint = -1;
    }

    public void SetFinished(bool finished)
    {
        _hasFinished = finished;
    }

    private void ValidateData()
    {
        if (RacerId < 1)
        {
            Debug.LogError(ErrorRacerIdInvalid, this);
            enabled = false;
        }
    }
}
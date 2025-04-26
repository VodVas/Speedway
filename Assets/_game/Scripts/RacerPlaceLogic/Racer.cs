using UnityEngine;

public class Racer : MonoBehaviour
{
    private const string ErrorRacerIdInvalid = "Racer: RacerId must be greater than zero.";
    private const string ErrorNewPositionInvalid = "Racer: Position must be greater than zero.";
    private const string ErrorCheckpointIndexInvalid = "Racer: CheckpointIndex must be greater than zero.";
    private const string ErrorRacerIdReadOnly = "Racer: RacerId can only be set once and must be greater than zero.";

    private bool _racerIdSet = false;

    public int Position { get; private set; } = 0;
    public int LastCheckpoint { get; private set; } = -1;
    public int PreviousPosition { get; private set; } = 0;
    public int LapsCompleted { get; private set; } = 0;
    public bool HasFinished { get; private set; } = false;

    [field: SerializeField] public string Name { get; private set; } = "Игрок";
    [field: SerializeField] public int RacerId { get; private set; } = 0;

    private void Awake()
    {
        if (RacerId < 0)
        {
            Debug.LogError(ErrorRacerIdInvalid, this);
            enabled = false;
        }
    }

    public void SetRacerId(int newId)
    {
        if (_racerIdSet)
        {
            Debug.LogError($"{ErrorRacerIdReadOnly} Attempted to change from {RacerId} to {newId}", this);
            return;
        }

        if (newId < 1)
        {
            Debug.LogError(ErrorRacerIdInvalid, this);
            enabled = false;
            return;
        }

        RacerId = newId;
        _racerIdSet = true;
    }

    public void SetPosition(int newPosition)
    {
        if (newPosition < 1)
        {
            Debug.LogError(ErrorNewPositionInvalid, this);
            enabled = false;
            return;
        }

        Position = newPosition;
    }

    public void UpdateLastCheckpoint(int checkpointIndex)
    {
        if (checkpointIndex < 0)
        {
            Debug.LogError(ErrorCheckpointIndexInvalid, this);
            enabled = false;
            return;
        }

        if (checkpointIndex > LastCheckpoint)
        {
            LastCheckpoint = checkpointIndex;
        }
    }

    public void UpdatePreviousPosition()
    {
        PreviousPosition = Position;
    }

    public void CompleteLap()
    {
        LapsCompleted++;
        LastCheckpoint = -1;
    }

    public void SetFinished(bool finished)
    {
        HasFinished = finished;
    }
}
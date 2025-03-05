using UnityEngine;

public class Racer : MonoBehaviour
{
    private const string ErrorRacerIdInvalid = "Racer: RacerId must be greater than zero.";
    private const string ErrorNewPositionInvalid = "Racer: Position must be greater than zero.";
    private const string ErrorCheckpointIndexInvalid = "Racer: CheckpointIndex must be greater than zero.";

    public int Position { get; private set; } = 0;
    public int LastCheckpoint { get; private set; } = -1;
    public int PreviousPosition { get; private set; } = 0;
    public int LapsCompleted { get; private set; } = 0;
    public bool HasFinished { get; private set; } = false;


    [field: SerializeField] public string Name { get; private set; } = "";
    [field: SerializeField] public int RacerId { get; set; } = 0; //TODO: в метод

    private void Awake()
    {
        ValidateData();
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

    private void ValidateData()
    {
        if (RacerId < 1)
        {
            Debug.LogError(ErrorRacerIdInvalid, this);
            enabled = false;
        }
    }
}
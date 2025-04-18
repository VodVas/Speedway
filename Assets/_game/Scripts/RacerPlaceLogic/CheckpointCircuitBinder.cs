using UnityEngine;
using System;

public class CheckpointCircuitBinder : MonoBehaviour
{
    [Serializable]
    private struct CheckpointData
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private int _index;
        [SerializeField] private bool _isConfigured;

        public Transform Transform => _transform;
        public int Index => _index;
        public bool IsConfigured => _isConfigured;

        public CheckpointData(Transform transform, int index, bool isConfigured)
        {
            _transform = transform;
            _index = index;
            _isConfigured = isConfigured;
        }
    }

    [SerializeField] private CheckpointData[] _checkpointsData = Array.Empty<CheckpointData>();
    [SerializeField] private RaceProgressTracker _raceProgressTracker;

    public int SetupCheckpoints()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return 0;

        if (_checkpointsData.Length != childCount)
        {
            _checkpointsData = new CheckpointData[childCount];
        }

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            string checkpointName = $"CheckpointTrigger{i:D3}";

            child.name = checkpointName;

            _checkpointsData[i] = new CheckpointData(child, i, true);
        }

        return childCount;
    }

    public RaceProgressTracker GetRaceProgressTracker()
    {
        return _raceProgressTracker;
    }

    public void SetRaceProgressTracker(RaceProgressTracker tracker)
    {
        _raceProgressTracker = tracker;
    }

    public Transform[] GetCheckpointTransforms()
    {
        int childCount = transform.childCount;
        Transform[] childTransforms = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            childTransforms[i] = transform.GetChild(i);
        }

        return childTransforms;
    }
}
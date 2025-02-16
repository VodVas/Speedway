using UnityEngine;

public class MinimapInitializer : MonoBehaviour
{
    [SerializeField] private Transform[] _npcTransforms;
    [SerializeField] private RectTransform[] _npcIcons;
    [SerializeField] private MinimapDisplay _minimapDisplay;

    private void Start()
    {
        if (_npcTransforms.Length != _npcIcons.Length)
        {
            Debug.LogError(" оличество NPC и иконок должно совпадать!");
            enabled = false;
            return;
        }

        for (int i = 0; i < _npcTransforms.Length; i++)
        {
            _minimapDisplay.RegisterRacer(_npcTransforms[i], _npcIcons[i]);
        }
    }
}
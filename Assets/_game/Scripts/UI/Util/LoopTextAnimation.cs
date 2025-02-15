using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoopTextAnimation : MonoBehaviour
{
    private const string SpaceCharacterName = " ";

    [SerializeField] private TargetType _targetType = TargetType.letters;
    [SerializeField] private Color _focusedColor = Color.red;
    [SerializeField] private float _letterDelay = 0.5f;

    private List<int> _targetLetterIndices = new List<int>();
    private List<List<int>> _targetWordsList = new List<List<int>>();
    private TextMeshPro _text;
    private Color[] _originalColors;
    private WaitForSeconds _wait;

    private enum TargetType
    {
        letters,
        words
    }

    private void Awake()
    {
        _wait = new WaitForSeconds(_letterDelay);
        _text = GetComponent<TextMeshPro>();

        if (_text == null)
        {
            Debug.LogError("TextMeshPro component is missing.", this);
            enabled = false;
            return;
        }

        _originalColors = new Color[_text.textInfo.characterCount];

        for (int i = 0; i < _text.textInfo.characterCount; i++)
        {
            var characterInfo = _text.textInfo.characterInfo[i];

            if (characterInfo.isVisible)
            {
                int materialIndex = characterInfo.materialReferenceIndex;
                int vertexIndex = characterInfo.vertexIndex;

                var meshInfo = _text.textInfo.meshInfo[materialIndex];
                _originalColors[i] = meshInfo.colors32[vertexIndex];
            }
        }
    }

    private void OnEnable()
    {
        UpdateTargetList();

        if (_targetType == TargetType.letters)
            StartCoroutine(LetterAnimationRoutine());
        else
            StartCoroutine(WordAnimationRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ResetColors();
    }

    public void UpdateTargetList()
    {
        _targetLetterIndices.Clear();
        _targetWordsList.Clear();

        if (_targetType == TargetType.letters)
        {
            for (int i = 0; i < _text.textInfo.characterCount; i++)
            {
                var characterInfo = _text.textInfo.characterInfo[i];

                if (characterInfo.isVisible)
                {
                    _targetLetterIndices.Add(i);
                }
            }
        }
        else
        {
            List<int> letterList = new List<int>();

            for (int i = 0; i < _text.textInfo.characterCount; i++)
            {
                var characterInfo = _text.textInfo.characterInfo[i];

                if (characterInfo.isVisible)
                {
                    if (characterInfo.character.ToString() == SpaceCharacterName)
                    {
                        _targetWordsList.Add(letterList);
                        letterList = new List<int>();
                    }
                    else
                    {
                        letterList.Add(i);
                    }
                }
            }
            if (letterList.Count > 0)
            {
                _targetWordsList.Add(letterList);
            }
        }
    }

    private IEnumerator LetterAnimationRoutine()
    {
        yield return null;

        for (int i = 0; i < _targetLetterIndices.Count; i++)
        {
            Focus(_targetLetterIndices[i]);

            yield return _wait;

            UnFocus(_targetLetterIndices[i]);
        }

        StartCoroutine(LetterAnimationRoutine());
    }

    private IEnumerator WordAnimationRoutine()
    {
        yield return null;

        for (int i = 0; i < _targetWordsList.Count; i++)
        {
            for (int j = 0; j < _targetWordsList[i].Count; j++)
            {
                Focus(_targetWordsList[i][j]);
            }

            yield return _wait;

            for (int j = 0; j < _targetWordsList[i].Count; j++)
            {
                UnFocus(_targetWordsList[i][j]);
            }
        }

        StartCoroutine(WordAnimationRoutine());
    }

    private void Focus(int index)
    {
        if (index < 0 || index >= _text.textInfo.characterCount)
            return;

        var characterInfo = _text.textInfo.characterInfo[index];

        if (characterInfo.isVisible)
        {
            int materialIndex = characterInfo.materialReferenceIndex;
            int vertexIndex = characterInfo.vertexIndex;
            var meshInfo = _text.textInfo.meshInfo[materialIndex];
            meshInfo.colors32[vertexIndex] = _focusedColor;
            meshInfo.colors32[vertexIndex + 1] = _focusedColor;
            meshInfo.colors32[vertexIndex + 2] = _focusedColor;
            meshInfo.colors32[vertexIndex + 3] = _focusedColor;

            _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
    }

    private void UnFocus(int index)
    {
        if (index < 0 || index >= _text.textInfo.characterCount)
            return;

        var characterInfo = _text.textInfo.characterInfo[index];

        if (characterInfo.isVisible)
        {
            int materialIndex = characterInfo.materialReferenceIndex;
            int vertexIndex = characterInfo.vertexIndex;
            var meshInfo = _text.textInfo.meshInfo[materialIndex];
            meshInfo.colors32[vertexIndex] = _originalColors[index];
            meshInfo.colors32[vertexIndex + 1] = _originalColors[index];
            meshInfo.colors32[vertexIndex + 2] = _originalColors[index];
            meshInfo.colors32[vertexIndex + 3] = _originalColors[index];

            _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
    }

    private void ResetColors()
    {
        for (int i = 0; i < _text.textInfo.characterCount; i++)
        {
            UnFocus(i);
        }
    }
}
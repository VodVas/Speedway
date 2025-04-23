using TMPro;
using UnityEngine;

public sealed class ScoreboardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreboardText;
    private RacerDataContainer _racerData;
    private readonly System.Text.StringBuilder _sb = new(128);

    public void Initialize(RacerDataContainer data)
    {
        _racerData = data;
        _racerData.OnDataChanged += UpdateView;
        UpdateView();
    }

    private void OnDestroy()
    {
        if (_racerData != null)
            _racerData.OnDataChanged -= UpdateView;
    }

    public void UpdateView()
    {
        if (_scoreboardText == null) return;

        _sb.Clear();
        var racers = _racerData.GetActiveRacers();

        for (int i = 0; i < racers.Length; i++)
        {
            _sb.Append(racers[i].Name);
            _sb.Append(" — ");
            _sb.Append(racers[i].Kills);
            if (i < racers.Length - 1) _sb.Append('\n');
        }

        _scoreboardText.SetText(_sb);
    }
}
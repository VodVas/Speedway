using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

/// <summary>
/// UI-компонент, который показывает счётчики фрагов/смертей на экране.
/// </summary>
public class DeathMatchScoreUI : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI _scoreboardText;

    //private StringBuilder _stringBuilder;

    //private void Awake()
    //{
    //    // Резервируем память под строку, чтобы не делать лишних перераспределений
    //    _stringBuilder = new StringBuilder(256);

    //    // При желании можно сразу очистить поле UI
    //    if (_scoreboardText != null)
    //    {
    //        _scoreboardText.text = string.Empty;
    //    }
    //}

    ///// <summary>
    ///// Обновляет содержимое таблицы счётов.
    ///// </summary>
    ///// <param name="kills">Словарь (Racer -> kills)</param>
    ///// <param name="deaths">Словарь (Racer -> deaths)</param>
    //public void UpdateScoreboard(Dictionary<Racer, int> kills, Dictionary<Racer, int> deaths)
    //{
    //    if (_scoreboardText == null)
    //        return;

    //    _stringBuilder.Clear();
    //    _stringBuilder.AppendLine("DEATH MATCH SCOREBOARD:");

    //    // Фиксированный порядок вывода не гарантируем, так как Dictionary неупорядочен.
    //    // Если нужен порядок, заведите список-“турнирную таблицу” и сортируйте.
    //    foreach (KeyValuePair<Racer, int> kvp in kills)
    //    {
    //        Racer racer = kvp.Key;
    //        int killCount = kvp.Value;
    //        int deathCount = 0;
    //        deaths.TryGetValue(racer, out deathCount);

    //        // Выводим имя из Racer.Name
    //        _stringBuilder.Append(racer.Name);
    //        _stringBuilder.Append("   Kills: ");
    //        _stringBuilder.Append(killCount);
    //        _stringBuilder.Append("   Deaths: ");
    //        _stringBuilder.Append(deathCount);
    //        _stringBuilder.AppendLine();
    //    }

    //    _scoreboardText.text = _stringBuilder.ToString();
    //}
}

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RaceRewardHandler))]
public class RaceRewardHandlerEditor : Editor
{
    SerializedProperty _isBossBattleProperty;
    SerializedProperty _currentBossProperty;
    SerializedProperty _respectRewardsProperty;
    SerializedProperty _positionPenaltiesProperty;
    SerializedProperty _bossWinRespectProperty;
    SerializedProperty _bossLoseRespectProperty;

    private void OnEnable()
    {
        _isBossBattleProperty = serializedObject.FindProperty("_isBossBattle");
        _currentBossProperty = serializedObject.FindProperty("_currentBoss");
        _respectRewardsProperty = serializedObject.FindProperty("_respectRewards");
        _positionPenaltiesProperty = serializedObject.FindProperty("_positionPenalties");
        _bossWinRespectProperty = serializedObject.FindProperty("_bossWinRespect");
        _bossLoseRespectProperty = serializedObject.FindProperty("_bossLoseRespect");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_pauseManager"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_positionRewards"));
        EditorGUILayout.PropertyField(_isBossBattleProperty);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_desktopRewardMenu"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_desktopPlayerDisabledUIElements"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_desktopRacerDisplays"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_mobileRewardMenu"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_mobilePlayerDisabledUIElements"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_mobileRacerDisplays"), true);

        if (_isBossBattleProperty.boolValue)
        {
            EditorGUILayout.PropertyField(_currentBossProperty, new GUIContent("Текущий Босс"));
            EditorGUILayout.PropertyField(_bossWinRespectProperty);
            EditorGUILayout.PropertyField(_bossLoseRespectProperty);
        }
        else
        {
            EditorGUILayout.PropertyField(_respectRewardsProperty);
            EditorGUILayout.PropertyField(_positionPenaltiesProperty);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
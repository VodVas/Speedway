//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(CarInstanceModifier))]
//public class CarInstanceModifierEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        CarInstanceModifier optimizer = (CarInstanceModifier)target;

//        if (GUILayout.Button("Cache All GUIDs"))
//        {
//            Undo.RecordObject(optimizer, "Cache GUIDs");

//            foreach (var configurator in optimizer.CarsPrefabs)
//            {
//                AutoFillCar(configurator);
//            }
//            EditorUtility.SetDirty(optimizer);
//        }
//    }

//    private void AutoFillCar(CarInstanceConfigurator configurator)
//    {
//        if (configurator.CarPrefab == null)
//        {
//            Debug.LogWarning("Car Prefab is not assigned!");
//            return;
//        }

//        configurator.CacheGuid();
//         EditorUtility.SetDirty(target);
//    }
//}
//#endif
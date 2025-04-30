#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using NUnit.Framework;
using YG;

public class SaveSystemEditorTests
{
    [MenuItem("Tools/Apocalypse/Tests/Run Save Tests")]
    public static void RunTests()
    {
        TestCarPurchase();
        TestMoneySpend();
        TestSaveLoad();
        Debug.Log("All tests completed!");
    }

    [Test]
    public static void TestCarPurchase()
    {
        var saves = new SavesYG();
        saves.AddCar(5);
        Assert.IsTrue(saves.HasCar(5));
    }

    [Test]
    public static void TestMoneySpend()
    {
        var saves = new SavesYG();
        saves.AddMoney(1000);
        bool success = saves.TrySpendMoney(500);
        Assert.IsTrue(success);
        Assert.AreEqual(500, saves.Money);
    }

    [Test]
    public static void TestSaveLoad()
    {
        var original = new SavesYG();
        original.AddCar(5);
        string json = JsonUtility.ToJson(original);
        Debug.Log($"JSON: {json}");

        var loaded = JsonUtility.FromJson<SavesYG>(json);
        Assert.IsTrue(loaded.HasCar(5));
    }
}
#endif
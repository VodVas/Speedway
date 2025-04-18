//using Reflex.Core;
//using UnityEngine;

//public class DriftScoreInstaller : MonoBehaviour, IInstaller
//{
//    private const string ErrorMissingField = "[UIInstaller] Не все поля назначены в инспекторе!";

//    [SerializeField] private DriftScoreUIDisplayer _desktopDriftScoreUIDisplayer = null;
//    //[SerializeField] private DriftScoreUIDisplayer _mobileDriftScoreUIDisplayer = null;

//    private void Awake()
//    {
//        if (_desktopDriftScoreUIDisplayer == null/* || _mobileDriftScoreUIDisplayer == null*/)
//        {
//            Debug.LogError(ErrorMissingField, this);
//            enabled = false;
//            return;
//        }
//    }

//    public void InstallBindings(ContainerBuilder builder)
//    {
//        builder.AddSingleton(_desktopDriftScoreUIDisplayer, typeof(DriftScoreUIDisplayer));
//       // builder.AddSingleton(_mobileDriftScoreUIDisplayer, typeof(DriftScoreUIDisplayer));
//    }
//}
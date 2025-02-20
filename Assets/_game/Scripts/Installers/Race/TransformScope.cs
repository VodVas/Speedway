using UnityEngine;
using Reflex.Core;

public class TransformScope : MonoBehaviour, IInstaller
{
    [SerializeField] private Transform _obj;
    [SerializeField] private AiStuckHelper _aiStuckHelper;

    private const string ErrorMissingField = "[UIInstaller] Не все поля назначены в инспекторе!";

    private void Awake()
    {
        if (_obj == null ||
            _aiStuckHelper == null)
        {
            Debug.LogError(ErrorMissingField, this);
            enabled = false;
            return;
        }
    }

    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(_obj, typeof(Transform));
        builder.AddSingleton(_aiStuckHelper, typeof(AiStuckHelper));
    }
}
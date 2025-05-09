using UnityEngine;
using UnityEngine.UI;

public class SaveResetTrigger : MonoBehaviour
{
    [SerializeField] private ConfirmationDialog _dialog;
    [SerializeField] private SaveResetter _resetter;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(InitiateReset);
    }

    private void InitiateReset()
    {
        _dialog.Show(_resetter.Execute);
    }
}
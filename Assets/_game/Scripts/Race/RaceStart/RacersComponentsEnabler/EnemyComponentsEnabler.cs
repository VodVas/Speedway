using ArcadeVP;
using UnityEngine;

public class EnemyComponentsEnabler : BaseComponentsEnabler
{
    [SerializeField] private ArcadeAiVehicleController _aiController;

    protected override bool IsBoss
    {
        get { return false; }
    }

    protected override void EnableComponents()
    {
        if (_aiController != null)
        {
            _aiController.enabled = true;
        }
    }
}
using ArcadeVP;
using UnityEngine;

public class EnemyComponentsEnabler : BaseComponentsEnabler
{
    [SerializeField] private ArcadeAiVehicleController _aiController;

    protected override void Awake()
    {
        _aiController = GetComponent<ArcadeAiVehicleController>();
    }

    protected override void EnableComponents()
    {
        EnableColliders();

        _aiController.enabled = true;
    }
}
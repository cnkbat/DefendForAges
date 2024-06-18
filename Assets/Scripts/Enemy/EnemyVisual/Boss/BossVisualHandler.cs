using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossVisualHandler : EnemyVisualsHandler
{

    protected override void OnEnable()
    {
        base.OnEnable();

        indicatorAssigner?.OnEnableIndicator?.Invoke(PointableTypes.boss, Color.red);
    }

}

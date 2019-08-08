using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCC.Movement;
using RPGCC.Combat;


namespace RPGCC.Control
{
    public interface IAgentController
    {
        MoveToTargetCommand MoveToTarget(Mover mover, Vector3 target);
        AttackCommand Attack(Transform target);
        AnimatorSetTriggerCommand TriggerAnimation (string trigger);
    }

}

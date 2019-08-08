using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCC.Core;
using RPGCC.Combat;
using RPGCC.Movement;

namespace RPGCC.Control
{
    public class AgentController : MonoBehaviour, IAgentController
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public AttackCommand Attack(Transform target)
        {
            return new AttackCommand(target);
        }

        public MoveToTargetCommand MoveToTarget (Mover mover, Vector3 destination)
        {
            return new MoveToTargetCommand(mover, destination);
        }

        public AnimatorSetTriggerCommand TriggerAnimation(string trigger)
        {
            return new AnimatorSetTriggerCommand(trigger);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

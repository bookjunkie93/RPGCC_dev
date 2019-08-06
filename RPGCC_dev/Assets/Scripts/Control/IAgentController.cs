using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCC.Control
{
    public abstract class IAgentController : MonoBehaviour
    {
        public abstract bool InteractWithMovement();

        public abstract bool InteractWithCombat();
         
    }
}

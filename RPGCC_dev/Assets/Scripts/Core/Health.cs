using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCC.Core
{
    public class Health : MonoBehaviour, IAction
    {
        ActionScheduler m_rcScheduler;
        [SerializeField]
        float health = 100f;
        private bool m_rcIsDead;
        public bool IsDead {get{return m_rcIsDead; } }

        private void Start()
        {
            m_rcScheduler = GetComponent<ActionScheduler>();
        }
        public void TakeDamage (float damage)
        {
            health = Mathf.Max(health - damage, 0f);
            if(health.Equals(0))
            {
                OnHitPointsReachZero();
            }
            
        }

        public void Cancel()
        {
            //can't cancel death!
        }

        public float GetCurrentHitPoints()
        {
            return health;
        }

        void OnHitPointsReachZero()
        {
            if(!IsDead)
            {
                m_rcScheduler.CancelCurrentAction();
                GetComponent<Animator>().SetTrigger("die");
                m_rcIsDead = true;
                this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }

        }
    
    }
}

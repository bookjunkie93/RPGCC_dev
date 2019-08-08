using UnityEngine;
using System.Collections;
using RPGCC.Movement;
using RPGCC.Core;
namespace RPGCC.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        float weaponRange;

        [SerializeField]
        float attackCooldown = 1f;

        [SerializeField]
        float attackDamage = 5f;

        float timeSinceLastAttack = Mathf.Infinity;

        private Transform currentTarget;

        private Mover mover;

        private ActionScheduler actionScheduler;

        private Animator animator;

        void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        public bool CanAttack(GameObject target)
        {
            Health targetRef = target.GetComponent<Health>();
            if(targetRef != null && !targetRef.IsDead)
            {
                return true;
            }
            return false;
        }

        public void Attack (Transform target)
        {
            actionScheduler.StartAction(this);
            currentTarget = target;            
        }
        void AttackInternal ()
        {
            if (currentTarget != null)
            {
                if (Vector3.Distance(this.transform.position, currentTarget.transform.position) <= weaponRange)
                {
                    mover.Cancel();
                    StrikeTarget();
                }
                else
                {
                    mover.MoveTo(currentTarget.transform.position);
                }
            }
        }

        public void StrikeTarget()
        {
            if ((timeSinceLastAttack > attackCooldown) && currentTarget != null)
            {
                actionScheduler.StartAction(this);
                transform.LookAt(currentTarget);
                GetComponent<Animator>().ResetTrigger("cancelAttack");
                //this will trigger Hit()
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }

        //Animation Event
        void Hit()
        {
            if(currentTarget == null) return;
            Debug.Log("Take that!");
            Health targetHealth = currentTarget.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(attackDamage);
                if(targetHealth.IsDead)
                {
                    Cancel();
                }
            }
        }

        
        public void Cancel()
        {
            ClearTarget();
            animator.ResetTrigger("attack");
            animator.SetTrigger("cancelAttack");
            
        }

        public void ClearTarget ()
        {
            currentTarget = null;
        }
       

        // Update is called once per frame
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            AttackInternal();
            
        }
    }
}
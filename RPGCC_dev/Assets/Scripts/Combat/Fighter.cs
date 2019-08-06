using UnityEngine;
using System.Collections;
using RPGCC.Movement;
namespace RPGCC.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField]
        float weaponRange;

        private Transform currentTarget;

        private Mover mover;

        public void Attack (CombatTarget target)
        {
            currentTarget = target.transform;
            Debug.Log("Hiya!");
        }

        public void StrikeTarget (Transform target)
        {

            CombatTarget targetCombatRef = target.GetComponent<CombatTarget>();
            if(targetCombatRef != null)
            {
                targetCombatRef.OnAttack();
            }
        }

        public void CancelAttack ()
        {
            ClearTarget();
        }

        public void ClearTarget ()
        {
            currentTarget = null;
        }
        // Use this for initialization
        void Start()
        {
            mover = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            if(currentTarget != null)
            {
                if(Vector3.Distance(this.transform.position, currentTarget.position) <= weaponRange)
                {
                    mover.Stop();
                    StrikeTarget(currentTarget);
                    ClearTarget();
                }
                else
                {
                    mover.MoveTo(currentTarget.transform.position);
                }
            }
        }
    }
}
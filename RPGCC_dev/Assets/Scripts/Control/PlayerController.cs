using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCC.Movement;
using RPGCC.Combat;
using RPGCC.Core;
namespace RPGCC.Control
{

    public class PlayerController : IAgentController
    {
        [SerializeField]
        Mover m_rcMovementController;
        Fighter m_rcCombatController;
        [SerializeField]
        int MoveButton;
        [SerializeField]
        int AttackButton;


        // Start is called before the first frame update
        void Start()
        {
            m_rcMovementController = GetComponent<Mover>();
            if(m_rcMovementController == null)
            {
                throw new System.MissingMemberException("The Player is missing its Mover Component!");
            }
            m_rcCombatController = GetComponent<Fighter>();
            if(m_rcCombatController == null)
            {
                throw new System.MissingMemberException("The Player is missing its Fighter Component!");
            }
        }
        private Ray GetMouseRay ()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public void MoveToTarget(Vector3 target)
        {
            Disengage();
            if(m_rcMovementController != null)
            {
                m_rcMovementController.MoveTo(target);
            }
            
        }

        public void Attack(Transform target)
        {
            throw new System.NotImplementedException();
        }

	    public void Disengage()
        {
            m_rcCombatController.CancelAttack();
        }

        public override bool InteractWithMovement()
        {
            Ray ray = GetMouseRay();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {
                if(Input.GetMouseButton(MoveButton))
                {
                    MoveToTarget(hit.point);
                }
                return true;
            }
            
            return false;
        }

        public override bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                if(hit.transform.GetComponent<CombatTarget>())
                {
                    if(m_rcCombatController != null)
                    {
                        if(Input.GetMouseButtonDown(AttackButton)) 
                        {
                            m_rcCombatController.Attack(hit.transform.GetComponent<CombatTarget>());
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        void RunInteractQueue()
        {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }
	    // Update is called once per frame
	    void Update()
        {
            RunInteractQueue();          
        }

    }
}

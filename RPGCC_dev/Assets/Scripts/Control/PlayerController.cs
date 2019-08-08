using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCC.Movement;
using RPGCC.Combat;
using RPGCC.Core;
namespace RPGCC.Control
{

    public class PlayerController : MonoBehaviour
    {
        Mover m_rcMovementController;
        Fighter m_rcCombatController;
        AgentController m_rcController;
        Health m_rcHealth;
        [SerializeField]
        int MoveButton;
        [SerializeField]
        int AttackButton;


        // Start is called before the first frame update
        void Start()
        {
            m_rcController = GetComponent<AgentController>();
            if(m_rcController == null)
            {
                throw new System.MissingMemberException("the Player is Missing its CharacterController Component!");
            }
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
            m_rcHealth = GetComponent<Health>();
            if(m_rcHealth == null)
            {
                throw new System.MissingMemberException("The Player is missing its Health Component!");
            }
        }
        private Ray GetMouseRay ()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public bool InteractWithMovement()
        {
            Ray ray = GetMouseRay();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {
                MoveToTargetCommand moveCommand = m_rcController.MoveToTarget(m_rcMovementController, hit.point);
                if (Input.GetMouseButton(MoveButton))
                {
                    moveCommand.Execute();
                }
                return true;
            }
            
            return false;
        }

        public bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                if(hit.transform.GetComponent<CombatTarget>())
                {
                    if(m_rcCombatController != null && m_rcCombatController.CanAttack(hit.transform.gameObject))
                    {
                        
                        if(Input.GetMouseButton(AttackButton)) 
                        {
                            AttackCommand attack = m_rcController.Attack(hit.transform);
                            attack.Execute(m_rcCombatController);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public void RunInteractQueue()
        {
            if(!GetComponent<Health>().IsDead)
            {
                if (InteractWithCombat()) return;
                if (InteractWithMovement()) return;
            }
        }



        // Update is called once per frame
	    void Update()
        {
            
            RunInteractQueue();          
        }

    }
}

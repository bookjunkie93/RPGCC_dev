using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPGCC.Movement;
using RPGCC.Combat;
using RPGCC.Core;
using System;

namespace RPGCC.Control
{
    public class SoldierAIController : MonoBehaviour
    {
        Mover m_rcMovement;
        Fighter m_rcCombat;
        AgentController m_rcControl;
        ActionScheduler m_rcScheduler;
        [SerializeField]
        Transform RayCaster;
        [SerializeField]
        float fieldOfViewAngle;
        [SerializeField] float aggroRange;
        [SerializeField] float aggroTimeout = 5f;
        [SerializeField] float waypointTolerance= 1f;

        [SerializeField] PatrolPath patrolPath;
        Vector3 GuardPosition;
        private float timeSinceArrivedAtWaypoint;
        [SerializeField] float waypointDwellTime = 3f;

        private GameObject player;
        private Health m_rcHealth;
        private bool startedAttack;
        private float timeSinceLastAggro = Mathf.Infinity;
        private Vector3 nextPosition;
        private int currentWaypoint;


        // Start is called before the first frame update
        void Start()
        {
            m_rcMovement = GetComponent<Mover>();
            m_rcCombat = GetComponent<Fighter>();
            m_rcControl = GetComponent<AgentController>();
            m_rcHealth = GetComponent<Health>();
            m_rcScheduler = GetComponent<ActionScheduler>();
            player = GameObject.FindGameObjectWithTag("Player");
            GuardPosition = transform.position;
            nextPosition = GuardPosition;
        }

        bool InteractWithCombat()
        {
            if(ShouldChasePlayer() && m_rcCombat.CanAttack(player))
            {
                timeSinceLastAggro = 0;
                startedAttack = true;
                m_rcCombat.Attack(player.transform);
                return true;
            }
            else if(startedAttack &&(timeSinceLastAggro < aggroTimeout))
            {
                startedAttack = false;
                m_rcScheduler.CancelCurrentAction();
            }
            else if (startedAttack)
            {
                PatrolBehavior();
            }
            return false;
        }

        public void PatrolBehavior()
        {
            if (patrolPath != null && (transform.position != patrolPath.defaultPos))
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();                    
                }
                if(timeSinceArrivedAtWaypoint > waypointDwellTime)
                {
                    nextPosition = GetCurrentWaypoint();
                }
                
            }
            else
            {
                nextPosition = GuardPosition;
            }
            MoveToTarget(nextPosition);
        }

        private void MoveToTarget(Vector3 target)
        {
            if(m_rcMovement != null)
            {
                m_rcMovement.StartMoveAction(target);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypoint);
        }

        private void CycleWaypoint()
        {
            
            if ((currentWaypoint + 1).Equals(patrolPath.GetWaypointCount()))
            {
                currentWaypoint = 0;
            }
            else
            {
                currentWaypoint++;
            }
        }

        private bool AtWaypoint()
        {
            return (Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance);
               
        }

        bool InteractWithMovement()
        {
            if (timeSinceLastAggro > aggroTimeout)
            {
                if(patrolPath != null )
                {
                    PatrolBehavior();

                    return true;
                }
                else if(Vector3.Distance(transform.position, GuardPosition) > waypointTolerance)
                {
                    MoveToTarget(GuardPosition);
                    return true;
                }            
            }
            return false;            
        }

       
        void RunInteractQueue()
        {
            if(!m_rcHealth.IsDead)
            {
                if (InteractWithCombat()) return;
                if (InteractWithMovement()) return;                
            }
        }

        // Update is called once per frame
        void Update()
        {
            RunInteractQueue();
            UpdateTimers();

        }

        private void UpdateTimers()
        {
            timeSinceLastAggro += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private bool ShouldChasePlayer()
        {
            if (Vector3.Distance(this.transform.position, player.transform.position) <= aggroRange)
            {
                return true;
            }
            return false;
        }
        //called by Unity
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
        }
    }
}

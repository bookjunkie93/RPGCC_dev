using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPGCC.Core;
namespace RPGCC.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent m_rcAgent;
        Animator m_rcAnimator;
        ActionScheduler m_rcScheduler;
        private Vector3 currentVelocity;
        Ray lastRay;
        // Start is called before the first frame update
        void Start()
        {
            m_rcAgent = GetComponent<NavMeshAgent>();
            m_rcAnimator = GetComponent<Animator>();
            m_rcScheduler = GetComponent<ActionScheduler>();
        }

        public void StartMoveAction (Vector3 i_rcDestination)
        {
            if(m_rcScheduler != null)
            {
                m_rcScheduler.StartAction(this);
                
                MoveTo(i_rcDestination);
            }
        }

        public void StartRotationAction(Quaternion rotation)
        {
            if(m_rcScheduler != null)
            {
                m_rcScheduler.StartAction(this);
                RotateTo(rotation);
            }
        }

        public void RotateTo (Quaternion rotation)
        {
            Quaternion.Lerp(this.transform.rotation, rotation, 1);
        }

        public void MoveTo (Vector3 i_rcDestination)
        {
            m_rcAgent.isStopped = false;
            m_rcAgent.SetDestination(i_rcDestination);
        }

        public bool IsStopped()
        {
            return m_rcAgent.isStopped;
        }

        public bool HasReachedDestination()
        {
            return !(m_rcAgent.remainingDistance > m_rcAgent.stoppingDistance);
        }

        /// <summary>
        /// Set the Mover on a velocity 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        public void SetVelocity (float x, float z)
        {
            m_rcAgent.velocity = new Vector3(x, m_rcAgent.velocity.y, z);
        }

        public void Jump (float jumpHeight)
        {

        }

        public void Cancel()
        {
            m_rcAgent.isStopped = true;
        }

        public void UpdateAnimator ()
        {
            Vector3 globalVelocity = m_rcAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(globalVelocity);
            m_rcAnimator.SetFloat("forwardSpeed", localVelocity.z);
        }

        // Update is called once per frame
        void Update()
        {
            if(!currentVelocity.Equals(m_rcAgent.velocity))
            {
                currentVelocity = m_rcAgent.velocity;
                UpdateAnimator();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace RPGCC.Movement
{
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent m_rcAgent;
        Animator m_rcAnimator;
        private Vector3 lastVelocity;

        Ray lastRay;
        // Start is called before the first frame update
        void Start()
        {
            m_rcAgent = GetComponent<NavMeshAgent>();
            m_rcAnimator = GetComponent<Animator>();
        }

        public void MoveTo (Vector3 i_rcDestination)
        {
            m_rcAgent.isStopped = false;
            m_rcAgent.SetDestination(i_rcDestination);
        }

        public void Stop()
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
            if(!lastVelocity.Equals(m_rcAgent.velocity))
            {
                lastVelocity = m_rcAgent.velocity;
                UpdateAnimator();
            }
        }
    }
}

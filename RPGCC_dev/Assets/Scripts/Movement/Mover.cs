using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    private NavMeshAgent m_rcAgent;
    Animator m_rcAnimator;

    Ray lastRay;
    // Start is called before the first frame update
    void Start()
    {
        m_rcAgent = GetComponent<NavMeshAgent>();
        m_rcAnimator = GetComponent<Animator>();
    }

    public void OnUserClick ()
    {
              
    }

    void MoveToCursor ()
    {
        lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(lastRay, out hit);
        if(hasHit)
        {
            m_rcAgent.SetDestination(hit.point);
            
        }
        
    }

    void UpdateAnimator ()
    {
        Vector3 globalVelocity = m_rcAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(globalVelocity);
        m_rcAnimator.SetFloat("forwardSpeed", localVelocity.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
           MoveToCursor();
        }
        UpdateAnimator();
        Debug.DrawRay(lastRay.origin, lastRay.direction*100, Color.red);

        
    }
}

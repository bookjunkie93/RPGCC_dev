using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCC.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoMagnitude = 0.3f;
        public Vector3 defaultPos {get {return transform.position;}}
        [SerializeField] bool retracePath;
        private void Start()
        {
            
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for(int i=0; i< transform.childCount; i++)
            {
                Vector3 waypoint = GetWaypoint(i);
                Gizmos.DrawSphere(waypoint, waypointGizmoMagnitude);
                if (i > 0)
                {
                    Vector3 prevWaypoint = GetWaypoint(i-1);
                    Gizmos.DrawLine(prevWaypoint, waypoint);
                }
                if (i == (transform.childCount - 1) && !retracePath) //go from the end of the path to the start
                {
                    Vector3 start = GetWaypoint(0);
                    Gizmos.DrawLine(waypoint, start);
                }

            }
        }

        public Vector3 GetWaypoint(int i)
        {
            if(i < transform.childCount)
            {
                return transform.GetChild(i).transform.position;
            }
            return transform.position;
        }

        public int GetWaypointCount()
        {
            return transform.childCount;
        }

       
    }
}

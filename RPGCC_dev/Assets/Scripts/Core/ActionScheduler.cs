using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCC.Core
{
    public enum ActionPriority {Low = 0, Normal = 1, High = 2, Urgent = 3};
    public class ActionScheduler : MonoBehaviour
    {
        Queue<IAction> LowPriorityQueue;
        Queue<IAction> ActionQueue;
        Queue<IAction> HighPriorityQueue;
        Queue<IAction> UrgentQueue;

        IAction currentAction;
        ActionPriority currentPriority;
        void Start ()
        {
            LowPriorityQueue = new Queue<IAction>();
            ActionQueue = new Queue<IAction>();
            HighPriorityQueue = new Queue<IAction>();
            UrgentQueue = new Queue<IAction> ();
        }

        public void StartAction (IAction action, ActionPriority priority = ActionPriority.Normal)
        {
            if((currentAction != null) && !currentAction.Equals(action) )
            {
                if((priority > currentPriority))
                {
                    currentAction.Cancel();
                }
                else
                {
                    Enqueue(action, priority);
                }
            }
            currentAction = action;
        }

        private IAction DeQueueNextAction()
        {
            if(UrgentQueue.Count > 0)
            {
                return UrgentQueue.Dequeue();
            }
            else if (HighPriorityQueue.Count > 0)
            {
                return HighPriorityQueue.Dequeue();
            }
            else if (ActionQueue.Count > 0)
            {
                return ActionQueue.Dequeue();
            }
            else if (LowPriorityQueue.Count > 0)
            {
                return LowPriorityQueue.Dequeue();
            }
            return null;
        }

        private void Enqueue(IAction action, ActionPriority priority)
        {
            switch(priority)
            {
                case ActionPriority.Low:
                    LowPriorityQueue.Enqueue(action);
                    return;
                case ActionPriority.High:
                    HighPriorityQueue.Enqueue(action);
                    return;
                case ActionPriority.Urgent:
                    UrgentQueue.Enqueue(action);
                    return;
                default:
                    ActionQueue.Enqueue(action);
                    return;
            }
        }

        public void OnActionComplete (IAction action)
        {
            if((action != null) && action.Equals(currentAction))
            {
                IAction nextAction = DeQueueNextAction();
                StartAction(nextAction);
            }
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
        
    }
}

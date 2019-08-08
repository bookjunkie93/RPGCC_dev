using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPGCC.Movement;
using RPGCC.Combat;
using RPGCC.Core;
namespace RPGCC.Control
{
    #region Base Commands
    public abstract class Command
    {
        public abstract void Execute();
        public abstract void UnExecute();
    }
    public class NullCommand : Command
    {
        public override void Execute()
        {
            //do nothing
        }

        public override void UnExecute()
        {
        }
    }
    #endregion
    #region Mover commands
    public abstract class MoveCommand : Command
    {
        public Mover mover;
        public MoveCommand(Mover i_mover)
        {
            mover = i_mover;
        }
    }
    public class NullMoveCommand : MoveCommand
    {
        public NullMoveCommand(Mover i_mover) : base(i_mover) {}
        public override void Execute() { }
        public override void UnExecute() { }
    }
    public class MoveToTargetCommand : MoveCommand
    {
        Vector3 target;
        Vector3 start;
        public MoveToTargetCommand(Mover i_mover, Vector3 inputTarget): base (i_mover)
        {
           target = inputTarget;
        }
        public override void Execute()
        {
            if (mover != null)
            {
                start = mover.transform.position;
                mover.StartMoveAction(target);
            }
        }

        public override void UnExecute()
        {
            if(mover != null)
            {
                mover.StartMoveAction(start);
            }
        }
    }
    public class UpdateAnimatorCommand : MoveCommand
    {
        public UpdateAnimatorCommand(Mover i_mover):base(i_mover){}
        public override void Execute()
        {
            if(mover != null)
            {
                mover.UpdateAnimator();
            }
        }
        public override void UnExecute()
        {
            throw new NotImplementedException();
        }
    }

    public class JumpCommand : MoveCommand
    {
        float jumpHeight;
        public JumpCommand(Mover i_mover, float i_jumpHeight) : base (i_mover)
        {
            jumpHeight = i_jumpHeight;
        }
        public override void Execute()
        {
            throw new NotImplementedException();
        }
        public override void UnExecute()
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region GameObject Commands
    public abstract class GameObjectCommand
    {
        public abstract void Execute (GameObject @object);
    }
    public class NullGameObjectCommand : GameObjectCommand
    {
        public override void Execute(GameObject @object) {}        
    }
    #endregion
    #region Fight Commands
    public abstract class FightCommand
    {
        public abstract void Execute(Fighter fighter);
    }
    public class NullFightCommand : FightCommand
    {
        public override void Execute(Fighter fighter){}
    }
    public class AttackCommand : FightCommand
    {
        Transform target;
        public AttackCommand(Transform inputTarget)
        {
            target = inputTarget;
        }

        public override void Execute(Fighter fighter)
        {
            if(fighter != null)
            {
                fighter.Attack(target);
            }
        }
    }
    public class CancelAttackCommand : FightCommand
    {
        public override void Execute(Fighter fighter)
        {
            fighter.Cancel();
        }
    }
    #endregion
    #region Agent Commands
    public abstract class AgentCommand
    {
        public abstract void Execute(IAgentController @agent);
    }
    public class NullAgentCommand :AgentCommand
    {
        public override void Execute (IAgentController @agent){}
    }
    #endregion
    #region Animator Commands
    public abstract class AnimatorCommand
    {
        public abstract void Execute(Animator @animator);
    }

    public class NullAnimatorCommand : AnimatorCommand
    {
        public override void Execute(Animator animator)
        {
            
        }
    }
    public class AnimatorSetTriggerCommand : AnimatorCommand
    {
        string trigger;
        public AnimatorSetTriggerCommand(string i_trigger)
        {
            trigger = i_trigger;
        }
        public override void Execute(Animator animator)
        {
            if(animator != null)
            {
                animator.SetTrigger(trigger);
            }
        }
    }
    #endregion
    #region ActionCommands
    public abstract class ActionCommand : Command
    {
        public IAction action;
        public ActionCommand(IAction i_rcAction)
        {
            action = i_rcAction;
        }
    }
    public class CancelActionCommand : ActionCommand
    {
        public CancelActionCommand(IAction i_rcAction) : base (i_rcAction){}
        public override void Execute()
        {
            if (action != null)
            {
                action.Cancel();
            }
        }
        public override void UnExecute()
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region CombatTargetCommand
    public abstract class CombatTargetCommand : Command
    {
        public CombatTarget target;
        public CombatTargetCommand (CombatTarget i_target)
        {
            target = i_target;
        }
    }
    public class NullCombatTargetCommand : CombatTargetCommand
    {
        public NullCombatTargetCommand(CombatTarget i_target) : base(i_target){}
        public override void Execute(){}
        public override void UnExecute(){}
    }
    public class OnAttackCommand : CombatTargetCommand
    {
        float attackDamage;
        public OnAttackCommand(CombatTarget i_target, float i_AttackDamage):base(i_target)
        {
            attackDamage = i_AttackDamage;
        }
        public override void Execute()
        {
            if(target != null)
            {
                target.OnAttack(attackDamage);
            }
        }

        public override void UnExecute()
        {
            //undo attack stuff here
        }
    }
    #endregion    
}

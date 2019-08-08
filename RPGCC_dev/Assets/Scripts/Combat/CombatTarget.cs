using UnityEngine;
using System.Collections;
using RPGCC.Core;
namespace RPGCC.Combat
{
   [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {

        public void OnAttack (float attackDamage)
        {
            //modify damage
            float modifiedDamage = ModifyDamage(attackDamage);
            GetComponent<Health>().TakeDamage(modifiedDamage);
            Debug.Log ("Ow!");
        }

        float ModifyDamage(float attackDamage)
        {
            //any armor or elemental modifiers are calculated here
            return attackDamage;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

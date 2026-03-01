using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy Actions/Attack")]
public class A_Attack : GOAction
{
    public override TaskStatus OnUpdate()
    {
        Debug.Log("Attack");
        //A_ZombieFlockManager.instance.isAttacking = true;
        return TaskStatus.RUNNING;
    }
}
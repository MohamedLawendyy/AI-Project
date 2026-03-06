using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
[Action("Zombie Swarm Enemy/Leader/LeaderAttack")]
public class A_LeaderAttack : GOAction
{
    public override void OnStart()
    {
        A_ZombieFlockManager.instance.isAttacking = true;
    }
    public override TaskStatus OnUpdate()
    {
        Debug.Log("Leader Attack");
        return TaskStatus.RUNNING;
    }
}
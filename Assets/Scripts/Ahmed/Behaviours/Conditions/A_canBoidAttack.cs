using BBUnity.Conditions;
using Pada1.BBCore;
using UnityEngine;
[Condition("Zombie Swarm Enemy/canBoidAttack")]
public class A_canBoidAttack : GOCondition
{
    public override bool Check()
    {
        Vector3 TargetDirection = A_ZombieFlockManager.instance.Goal.transform.position - gameObject.transform.position;
        TargetDirection.y = 0.0f;
        return A_ZombieFlockManager.instance.isAttacking && TargetDirection.magnitude <= A_ZombieFlockManager.instance.StopDistance;
    }
}
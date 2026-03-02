using BBUnity.Conditions;
using Pada1.BBCore;
using UnityEngine;
[Condition("Zombie Swarm Enemy Conditions/isBoidLeader")]
public class A_isBoidLeader : GOCondition
{
    public override bool Check()
    {
        return gameObject.GetComponent<A_BoidStats>().IsLeader;
    }
}
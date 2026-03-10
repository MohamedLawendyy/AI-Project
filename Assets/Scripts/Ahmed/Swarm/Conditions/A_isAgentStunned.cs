using BBUnity.Conditions;
using Pada1.BBCore;
using UnityEngine;
[Condition("Swarm Enemy/isStunned")]
public class A_isAgentStunned : GOCondition
{
    public override bool Check()
    {
        return gameObject.GetComponent<A_SwarmData>().IsStunned;
    }
}
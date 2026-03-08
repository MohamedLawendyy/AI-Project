using BBUnity.Conditions;
using Pada1.BBCore;
[Condition("Swarm Enemy/isAgentDead")]
public class A_isAgentDead : GOCondition
{
    public override bool Check()
    {
        return gameObject.GetComponent<A_SwarmData>().CurrentHealth <= 0;
    }
}
using BBUnity.Conditions;
using Pada1.BBCore;
[Condition("Zombie Swarm Enemy/isBoidLeader")]
public class A_isBoidLeader : GOCondition
{
    public override bool Check()
    {
        return gameObject.GetComponent<A_BoidStats>().IsLeader;
    }
}
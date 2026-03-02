using BBUnity.Conditions;
using Pada1.BBCore;
[Condition("Zombie Swarm Enemy Conditions/isLeaderFar")]
public class A_isLeaderFar : GOCondition
{
    [InParam("FarDistancetoLeader")] public float closeDistance;
    public override bool Check()
    {
        return (gameObject.transform.position - A_ZombieFlockManager.instance.Leader.transform.position).sqrMagnitude >= closeDistance * closeDistance;
    }
}
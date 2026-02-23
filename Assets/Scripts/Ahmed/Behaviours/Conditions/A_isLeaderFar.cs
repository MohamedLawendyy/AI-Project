using BBUnity.Conditions;
using Pada1.BBCore;
[Condition("Zombie Swarm Enemy Conditions/isLeaderFar")]
public class A_isLeaderFar : GOCondition
{
    private A_ZombieSwarmManager SwarmManager;
    [InParam("FarDistancetoLeader")] public float closeDistance;
    public override bool Check()
    {
        SwarmManager = gameObject.GetComponent<A_ZombieBoid>().SwarmManager;
        return (gameObject.transform.position - SwarmManager.Leader.transform.position).sqrMagnitude >= closeDistance * closeDistance;
    }
}
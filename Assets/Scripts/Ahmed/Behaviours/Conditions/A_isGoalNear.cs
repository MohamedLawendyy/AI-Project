using BBUnity.Conditions;
using Pada1.BBCore;
[Condition("Zombie Swarm Enemy/isGoalNear")]
public class A_isGoalNear : GOCondition
{
    [InParam("NearDistancetoGoal")] public float closeDistance;
    public override bool Check()
    {
        return (A_ZombieFlockManager.instance.Leader.transform.position - A_ZombieFlockManager.instance.Goal.transform.position).sqrMagnitude < closeDistance * closeDistance;
    }
}
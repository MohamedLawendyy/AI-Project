using BBUnity.Conditions;
using Pada1.BBCore;
[Condition("Zombie Swarm Enemy/isBoidDead")]
public class A_isBoidDead : GOCondition
{
    public override bool Check()
    {
        return gameObject.GetComponent<A_BoidStats>().CurrentHealth <= 0;
    }
}
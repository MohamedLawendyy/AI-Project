using UnityEngine;
using Pada1.BBCore;             // Logic for attributes like [Condition]
using BBUnity.Conditions;       // Logic for GOCondition

[Condition("Dragon/IsPlayerInRange")]
public class IsPlayerInRange : GOCondition
{
    [InParam("Target")]
    public GameObject target;

    [InParam("Range")]
    public float range;

    public override bool Check()
    {
        if (target == null || gameObject == null) return false;

        float dist = Vector3.Distance(gameObject.transform.position, target.transform.position);
        return dist <= range;
    }
}
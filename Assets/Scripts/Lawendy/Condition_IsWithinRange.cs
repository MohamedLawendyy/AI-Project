using UnityEngine;
using Pada1.BBCore;
using BBUnity.Conditions;

[Condition("MyGame/IsWithinRange")]
public class Condition_IsWithinRange : GOCondition
{
    [InParam("Target")] public GameObject target;
    [InParam("Range")] public float range;

    public override bool Check()
    {
        if (target == null) return false;

        float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);
        return distance <= range;
    }
}

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
        // FOOLPROOF FIX: Find the real player in the scene dynamically
        if (target == null || target.scene.rootCount == 0)
        {
            target = GameObject.FindWithTag("Player");
        }

        if (target == null) return false;

        float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);
        return distance <= range;
    }
}
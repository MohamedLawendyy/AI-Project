using UnityEngine;
using Pada1.BBCore;
using BBUnity.Conditions; // Use this for Unity-specific conditions

[Condition("MyGame/IsDead")]
public class Condition_IsDead : GOCondition 
{
    public override bool Check()
    {
        // Now "gameObject" is recognized!
        BossHealth health = gameObject.GetComponent<BossHealth>();
        return health != null && health.isDead;
    }
}
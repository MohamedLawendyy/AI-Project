using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;

[Condition("Dragon/Probability")]
public class ProbabilityCheck : ConditionBase
{
    [InParam("Percentage (0-100)")]
    public float percentage;

    public override bool Check()
    {
        // Pick a random number between 0 and 100
        float roll = Random.Range(0f, 100f);
        // If roll is less than percentage, we succeed
        return roll <= percentage;
    }
}
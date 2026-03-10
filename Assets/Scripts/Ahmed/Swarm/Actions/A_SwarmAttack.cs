using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
[Action("Swarm Enemy/Attack")]
public class A_SwarmAttack : GOAction
{
    public override void OnStart()
    {
        gameObject.GetComponent<A_SwarmAnimationController>().Attack();
    }
    public override TaskStatus OnUpdate()
    {
        Debug.Log("Agent Attacking");
        return TaskStatus.RUNNING;
    }
}
using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy/Boids/Attack")]
public class A_Attack : GOAction
{
    public override void OnStart()
    {
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
    }
    public override TaskStatus OnUpdate()
    {
        Debug.Log("Boid Attacking");
        return TaskStatus.RUNNING;
    }
}
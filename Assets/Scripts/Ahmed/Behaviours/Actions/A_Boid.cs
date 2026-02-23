using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy Actions/Boid")]
public class A_Boid : GOAction
{
    private A_ZombieSwarmManager SwarmManager;
    private NavMeshAgent agent;
    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        SwarmManager = gameObject.GetComponent<A_ZombieBoid>().SwarmManager;
        agent.SetDestination(gameObject.transform.position);
        agent.ResetPath();
    }
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.RUNNING;
    }
}
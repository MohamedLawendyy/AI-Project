using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy Actions/Boid")]
public class A_Boid : GOAction
{
    //private A_ZombieSwarmManager SwarmManager;
    private NavMeshAgent agent;
    private A_ZombieBoid Boid;
    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        //SwarmManager = gameObject.GetComponent<A_ZombieBoid>().SwarmManager;
        Boid = gameObject.GetComponent<A_ZombieBoid>();
        agent.SetDestination(gameObject.transform.position);
        agent.ResetPath();
    }
    public override TaskStatus OnUpdate()
    {
        Boid.FollowTarget();
        return TaskStatus.RUNNING;
    }
}
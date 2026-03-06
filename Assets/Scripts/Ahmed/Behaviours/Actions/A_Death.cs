using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;
[Action("Zombie Swarm Enemy/Death")]
public class A_Death : GOAction
{
    public override TaskStatus OnUpdate()
    {
        Debug.Log("Boid Dead");
        A_ZombieFlockManager.instance.KillBoid(gameObject);
        return TaskStatus.COMPLETED;
    }
}
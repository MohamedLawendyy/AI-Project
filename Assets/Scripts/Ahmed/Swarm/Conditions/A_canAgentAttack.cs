using BBUnity.Conditions;
using Pada1.BBCore;
using UnityEngine;
using UnityEngine.AI;
[Condition("Swarm Enemy/canAgentAttack")]
public class A_canAgentAttack : GOCondition
{
    private NavMeshAgent agent;
    public override bool Check()
    {
        if (agent == null)
            agent = gameObject.GetComponent<NavMeshAgent>();

        if (A_SwarmSpawnManager.instance.Goal == null)
            return false;

        float dist = Vector3.Distance(
            gameObject.transform.position,
            A_SwarmSpawnManager.instance.Goal.transform.position
        );

        return dist <= agent.stoppingDistance + 0.2f;
    }
}
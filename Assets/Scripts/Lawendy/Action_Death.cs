using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("MyGame/DeathAction")]
public class Action_Death : GOAction
{
    public override void OnStart()
    {
        base.OnStart();
        // Stop NavMesh Agent
        var agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) 
        {
            agent.isStopped = true;
            agent.enabled = false; // Disable to prevent physics issues
        }
    }

    public override TaskStatus OnUpdate()
    {
        // Keep returning Running so the tree doesn't cycle back to life
        return TaskStatus.RUNNING;
    }
}
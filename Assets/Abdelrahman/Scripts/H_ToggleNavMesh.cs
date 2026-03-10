using UnityEngine;
using UnityEngine.AI;
using Pada1.BBCore;           
using Pada1.BBCore.Tasks;     
using BBUnity.Actions;        

[Action("Dragon/SetFlying")]
public class DragonFlyState : GOAction
{
    [InParam("IsFlying")]
    public bool isFlying;

    private NavMeshAgent agent;
    private Animator anim;

    public override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        
        if (agent != null)
        {
            if (isFlying)
            {
                agent.baseOffset = 2.0f; // Lift dragon 2 meters up visually
                if(anim) anim.SetBool("IsFlying", true);
            }
            else
            {
                agent.baseOffset = 0f;   // Put back on ground
                if(anim) anim.SetBool("IsFlying", false);
            }
        }
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.COMPLETED;
    }
}
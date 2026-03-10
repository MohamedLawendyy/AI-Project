using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
[Action("Swarm Enemy/Death")]
public class A_Death : GOAction
{
    private Animator AnimationController;
    public override void OnStart()
    {
        AnimationController = gameObject.GetComponent<Animator>();
        AnimationController.SetTrigger("Death");
        AnimationController.SetFloat("DeathRnD", Random.Range(0, 2));
        Object.Destroy(gameObject, 3.0f);
    }
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.SUSPENDED;
    }
}
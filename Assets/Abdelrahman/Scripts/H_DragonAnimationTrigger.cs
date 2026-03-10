using UnityEngine;
using Pada1.BBCore;           // Logic for attributes like [Action]
using Pada1.BBCore.Tasks;     // Logic for TaskStatus
using BBUnity.Actions;        // Logic for GOAction

[Action("Dragon/PlayAnim")]
public class PlayDragonAnim : GOAction
{
    [InParam("TriggerName")]
    public string triggerName;

    [InParam("Duration")]
    public float duration = 2.0f;

    private Animator anim;
    private float timer;

    public override void OnStart()
    {
        if (gameObject != null)
        {
            anim = gameObject.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger(triggerName);
            }
        }
        timer = 0;
    }

    public override TaskStatus OnUpdate()
    {
        timer += Time.deltaTime;
        
        if (timer >= duration)
        {
            return TaskStatus.COMPLETED;
        }
        
        return TaskStatus.RUNNING;
    }
}
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("MyGame/AttackLaser")]
public class Action_AttackLaser : GOAction
{
    [InParam("Target")] public GameObject target;
    [InParam("LaserPrefab")] public GameObject laserPrefab;
    [InParam("HandTransform")] public GameObject hand;
    
    private float timer = 0;
    private float attackDuration = 2.0f; // Total time the boss stays in "Attack Mode"
    private bool hasFired = false;

    public override void OnStart()
    {
        if(target == null) target = GameObject.FindWithTag("Player");
        
        base.OnStart();
        timer = 0;
        hasFired = false;
        
        // Trigger the animation once at the start
        gameObject.GetComponent<Animator>().SetTrigger("Attack");
    }

    public override TaskStatus OnUpdate()
{
    if (target == null) return TaskStatus.FAILED;

    // FORCED ROTATION: Every frame while the animation plays, look at the player
    Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
    direction.y = 0; // Don't tilt up/down
    if (direction != Vector3.zero)
    {
        gameObject.transform.rotation = Quaternion.Slerp(
            gameObject.transform.rotation, 
            Quaternion.LookRotation(direction), 
            Time.deltaTime * 10f // Speed of turning
        );
    }

    timer += Time.deltaTime;
    // ... rest of your firing logic ...
    
    if (timer >= attackDuration) return TaskStatus.COMPLETED;
    return TaskStatus.RUNNING;
}
}
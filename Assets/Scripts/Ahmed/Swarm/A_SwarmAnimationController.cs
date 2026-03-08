using System;
using UnityEngine;
using UnityEngine.AI;
public class A_SwarmAnimationController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator AnimationController;
    private float LastSpeed;
    private bool isAttacking = false;
    private bool isStunned = false;
    private int normalPriority = 50;
    private int attackPriority = 99;

    private void Awake()
    {
        AnimationController = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
    }
    private void Update()
    {
        if (isAttacking) return;
        if (isStunned) return;
        else AnimationController.SetFloat("Speed", LastSpeed);

        Vector3 velocity = agent.desiredVelocity;
        AnimationController.SetFloat("Speed", velocity.magnitude);
        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }
    }
    private void LateUpdate()
    {
        AnimationController.SetFloat("Speed", agent.velocity.magnitude);
    }
    private void OnAnimatorMove()
    {
        transform.position += AnimationController.deltaPosition;
        transform.rotation *= AnimationController.deltaRotation;
        agent.nextPosition = transform.position;
    }
    public void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;
        transform.LookAt(A_SwarmSpawnManager.instance.Goal.transform);
        AnimationController.SetBool("Attack", true);
        agent.avoidancePriority = attackPriority;
        //agent.enabled = false;
        //AnimationController.applyRootMotion = false;
    }
    public void StopAttack()
    {
        isAttacking = false;
        AnimationController.SetBool("Attack", false);
        agent.avoidancePriority = normalPriority;
        //agent.enabled = true;
        //AnimationController.applyRootMotion = true;
    }
    internal void Stun()
    {
        LastSpeed = AnimationController.GetFloat("Speed");
        if (isStunned) return;
        isStunned = true;
        AnimationController.SetBool("Stun", true);
    }
    internal void StopStun()
    {
        isStunned = false;
        AnimationController.SetBool("Stun", false);
    }
}
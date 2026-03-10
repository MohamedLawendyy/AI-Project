using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using UnityEngine.AI;

namespace BBUnity.Actions
{
    [Action("Zombie/MoveToPlayer")]
    [Help("Moves the game object towards a given target by using a NavMeshAgent")]
    public class MoveToPlayer : GOAction
    {
        [InParam("target")]
        [Help("Target game object towards this game object will be moved")]
        public GameObject target;

        [InParam("speed")]
        [Help("Movement speed used while chasing the target")]
        public float speed = 3.5f;

        private NavMeshAgent navAgent;
        private Animator animator;

        private Transform targetTransform;
        private HealthSystem targetHealth;

        public override void OnStart()
        {

            if (target == null || target.scene.rootCount == 0)
                target = GameObject.FindWithTag("Player");

            if (target == null)
            {
                Debug.LogWarning("MoveToPlayer could not find a valid target.", gameObject);
                SetMoveState(0);
                return;
            }

            targetTransform = target.transform;
            targetHealth = target.GetComponent<HealthSystem>();
            animator = gameObject.GetComponent<Animator>();

            navAgent = gameObject.GetComponent<NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogWarning("The " + gameObject.name + " game object does not have a Nav Mesh Agent component to navigate. One with default values has been added", gameObject);
                navAgent = gameObject.AddComponent<NavMeshAgent>();
            }

            navAgent.speed = Mathf.Max(0f, speed);

            if (IsTargetDead())
            {
                StopNavigation();
                SetMoveState(0);
                return;
            }

            if (!IsAgentReady())
            {
                SetMoveState(0);
                return;
            }

            navAgent.SetDestination(targetTransform.position);
            SetMoveState(2);

#if UNITY_5_6_OR_NEWER
                navAgent.isStopped = false;
#else
            navAgent.Resume();
#endif
        }

        public override TaskStatus OnUpdate()
        {
            if (target == null || targetTransform == null)
                return TaskStatus.FAILED;

            if (IsTargetDead())
            {
                StopNavigation();
                SetMoveState(0);
                return TaskStatus.COMPLETED;
            }

            if (!IsAgentReady())
            {
                SetMoveState(0);
                return TaskStatus.FAILED;
            }

            navAgent.speed = Mathf.Max(0f, speed);

            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                SetMoveState(0);
                return TaskStatus.COMPLETED;
            }
            else if (navAgent.destination != targetTransform.position)
                navAgent.SetDestination(targetTransform.position);

            SetMoveState(2);
            return TaskStatus.RUNNING;
        }

        public override void OnAbort()
        {
            StopNavigation();
            SetMoveState(0);

        }

        private bool IsTargetDead()
        {
            return targetHealth != null && targetHealth.IsDead;
        }

        private bool IsAgentReady()
        {
            return navAgent != null && navAgent.enabled && navAgent.isActiveAndEnabled && navAgent.isOnNavMesh;
        }

        private void SetMoveState(int moveState)
        {
            if (animator != null)
            {
                animator.SetInteger("moveState", moveState);
            }
        }

        private void StopNavigation()
        {
#if UNITY_5_6_OR_NEWER
                if (IsAgentReady())
                    navAgent.isStopped = true;
#else
            if (navAgent != null)
                navAgent.Stop();
#endif
        }
    }
}

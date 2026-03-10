using cowsins;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace BBUnity.Actions
{
    [Action("Zombie/AttackPlayer")]
    [Help("Triggers an attack animation and finishes when the attack time ends")]
    public class AttackPlayer : GOAction
    {
        [InParam("target")]
        [Help("Target game object to attack")]
        public GameObject target;

        [InParam("attackDuration")]
        [Help("Attack animation duration in seconds")]
        public float attackDuration = 1f;

        [InParam("attackDamage")]
        [Help("Damage dealt to the target 1 second after the attack trigger")]
        public int attackDamage = 20;

        private NavMeshAgent navAgent;
        private Animator animator;
        private PlayerStats playerStats;
        private float attackStartTime;
        private bool didAttack;
        private bool damageApplied;

        public override void OnStart()
        {
            if (target == null || target.scene.rootCount == 0)
                target = GameObject.FindWithTag("Player");

            if (target == null)
            {
                didAttack = false;
                damageApplied = false;
                return;
            }

            playerStats = target.GetComponent<PlayerStats>();
            if (IsTargetDead())
            {
                didAttack = false;
                damageApplied = false;
                return;
            }

            animator = gameObject.GetComponent<Animator>();
            navAgent = gameObject.GetComponent<NavMeshAgent>();

            StopNavigation();
            FaceTarget();

            if (animator != null)
            {
                animator.SetTrigger("attack");
            }

            attackStartTime = Time.time;
            didAttack = true;
            damageApplied = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (target == null)
                return TaskStatus.FAILED;

            if (IsTargetDead())
                return TaskStatus.COMPLETED;

            if (!didAttack)
                return TaskStatus.COMPLETED;

            float elapsed = Time.time - attackStartTime;

            if (!damageApplied && elapsed >= 1f)
            {
                if (playerStats != null && !IsTargetDead())
                {
                    playerStats.Damage(Mathf.Max(0, attackDamage), false);
                }

                damageApplied = true;
            }

            float clampedAttackDuration = Mathf.Max(0f, attackDuration);
            bool attackDurationFinished = elapsed >= clampedAttackDuration;

            if (!attackDurationFinished || !damageApplied)
            {
                // Keep this action active (paused in-place) until the attack window is done.
                StopNavigation();
                return TaskStatus.RUNNING;
            }

            return TaskStatus.COMPLETED;
        }

        public override void OnAbort()
        {
            StopNavigation();
        }

        private bool IsTargetDead()
        {
            return playerStats != null && playerStats.isDead;
        }

        private void FaceTarget()
        {
            if (target == null)
                return;

            Vector3 directionToTarget = target.transform.position - gameObject.transform.position;
            directionToTarget.y = 0f;

            if (directionToTarget.sqrMagnitude > 0.0001f)
            {
                gameObject.transform.rotation = Quaternion.LookRotation(directionToTarget.normalized, Vector3.up);
            }
        }

        private void StopNavigation()
        {
#if UNITY_5_6_OR_NEWER
            if (navAgent != null)
                navAgent.isStopped = true;
#else
            if (navAgent != null)
                navAgent.Stop();
#endif
        }
    }
}

using UnityEngine;
public class A_BoidFormationManager : MonoBehaviour
{
    private float MovementSpeedBlend;
    private Vector3 Direction;
    private Vector3 SeparationForce;
    private Rigidbody RB;
    private void Awake()
    {
        Direction = Vector3.zero;
        RB = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (A_ZombieFlockManager.instance.isAttacking)
        {
            if (!RB.isKinematic)
                RB.isKinematic = true;
            FollowTarget();
        }
    }
    private void FollowTarget()
    {
        SeparationForce = Vector3.zero;
        Vector3 TargetDirection = A_ZombieFlockManager.instance.Goal.transform.position - transform.position;
        TargetDirection.y = 0.0f;
        Direction = TargetDirection;
        float distance = Direction.magnitude;

        Collider[] neigbours = GetNeighbours();
        if (neigbours.Length > 0)
        {
            CalculateSeparationForce(neigbours);
            ApplyAllignment(neigbours);
            ApplyCohesion(neigbours);
        }
        if (distance > A_ZombieFlockManager.instance.StopDistance)
        {
            MoveTowardsTarget();
            RotateTowardsTarget();
        }
        else
        {
            StopMove();
            StopRotate();
        }
    }
    private Collider[] GetNeighbours()
    {
        return Physics.OverlapSphere(transform.position, A_ZombieFlockManager.instance.DetectionDistance, A_ZombieFlockManager.instance.EnemyLayerMask);
    }
    private void CalculateSeparationForce(Collider[] neighbours)
    {
        foreach (Collider neighbour in neighbours)
        {
            if (neighbour.transform == transform) continue;

            Vector3 dir = transform.position - neighbour.transform.position;
            float distance = dir.magnitude;

            if (distance <= 0.001f) continue;

            SeparationForce += dir.normalized * (A_ZombieFlockManager.instance.SeparationWeight / (distance * distance));
        }
    }
    private void ApplyAllignment(Collider[] neighbours)
    {
        Vector3 neighboursForward = Vector3.zero;
        foreach (Collider neighbour in neighbours)
        {
            if (neighbour.transform == transform) continue;
            neighboursForward += neighbour.transform.forward;
        }
        if (neighboursForward != Vector3.zero)
            neighboursForward.Normalize();
        SeparationForce += neighboursForward * A_ZombieFlockManager.instance.AlignmentWeight;
    }
    private void ApplyCohesion(Collider[] neighbours)
    {
        Vector3 averagePosition = Vector3.zero;
        foreach (Collider neighbour in neighbours)
        {
            if (neighbour.transform == transform) continue;
            averagePosition += neighbour.transform.position;
        }
        averagePosition /= neighbours.Length - 1;
        Vector3 cohesionDir = (averagePosition - transform.position).normalized;
        SeparationForce += cohesionDir * A_ZombieFlockManager.instance.CohesionWeight;
    }
    private void MoveTowardsTarget()
    {
        Direction = Direction.normalized;

        Vector3 x = A_ZombieFlockManager.instance.FormationSpeed * Time.deltaTime * (Direction + SeparationForce);
        x.y = 0.0f;
        transform.position += x;

        MovementSpeedBlend = Mathf.Lerp(MovementSpeedBlend, 1, Time.deltaTime * A_ZombieFlockManager.instance.FormationSpeed);
        //ZombieAnimator.SetFloat("Speed", MovementSpeedBlend);
    }
    private void RotateTowardsTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), Time.deltaTime * A_ZombieFlockManager.instance.RotationSpeed);
    }
    private void StopMove()
    {
        MovementSpeedBlend = Mathf.Lerp(MovementSpeedBlend, 0, Time.deltaTime * A_ZombieFlockManager.instance.FormationSpeed);
        //ZombieAnimator.SetFloat("Speed", MovementSpeedBlend);
    }
    private void StopRotate()
    {
        transform.rotation = Quaternion.LookRotation(Direction);
    }
}
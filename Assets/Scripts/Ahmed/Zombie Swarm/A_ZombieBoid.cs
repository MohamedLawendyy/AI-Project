using UnityEngine;
public class A_ZombieBoid : MonoBehaviour
{
    [HideInInspector] public A_ZombieSwarmManager SwarmManager;
    private Vector3 Direction = Vector3.zero;
    private Vector3 SeparationForce;
    private float MovementSpeedBlend;

    private Rigidbody RB;
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        FollowTarget();
    }
    private void FollowTarget()
    {
        SeparationForce = Vector3.zero;
        Vector3 TargetDirection = SwarmManager.GetTargetPosition() - transform.position;
        TargetDirection.y = 0.0f;
        Direction = TargetDirection;
        float distance = Direction.magnitude;

        Collider[] neigbours = GetNeighbours();
        if (neigbours.Length > 0)
        {
            CalculateSeparationForce(neigbours);
            //ApplyAllignment(neigbours);
            ApplyCohesion(neigbours);
        }
        if (distance > SwarmManager.StopDistance)
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
        return Physics.OverlapSphere(transform.position, SwarmManager.DetectionDistance, SwarmManager.EnemyLayerMask);
    }
    private void CalculateSeparationForce(Collider[] neighbours)
    {
        foreach (Collider neighbour in neighbours)
        {
            if (neighbour.transform == transform)
                continue;
            Vector3 dir = neighbour.transform.position - transform.position;
            float distance = dir.magnitude;
            Vector3 away = -dir.normalized;
            if (distance > 0)
                SeparationForce += (away / distance) * SwarmManager.SeparationWeight;
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
        SeparationForce += neighboursForward * SwarmManager.AlignmentWeight;
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
        SeparationForce += cohesionDir * SwarmManager.CohesionWeight;
    }
    private void MoveTowardsTarget()
    {
        Direction = Direction.normalized;
        transform.position += SwarmManager.MovementSpeed * Time.deltaTime * (Direction + SeparationForce).normalized;
        MovementSpeedBlend = Mathf.Lerp(MovementSpeedBlend, 1, Time.deltaTime * SwarmManager.MovementSpeed);
        //ZombieAnimator.SetFloat("Speed", MovementSpeedBlend);
    }
    private void RotateTowardsTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), Time.deltaTime * SwarmManager.RotationSpeed);
    }
    private void StopMove()
    {
        MovementSpeedBlend = Mathf.Lerp(MovementSpeedBlend, 0, Time.deltaTime * SwarmManager.MovementSpeed);
        //ZombieAnimator.SetFloat("Speed", MovementSpeedBlend);
    }
    private void StopRotate()
    {
        transform.rotation = Quaternion.LookRotation(Direction);
    }
}
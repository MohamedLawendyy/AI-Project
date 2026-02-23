using UnityEngine;
public class A_ZombieBoid : MonoBehaviour
{
    [HideInInspector] public A_ZombieSwarmManager SwarmManager;
    private Vector3 Direction = Vector3.zero;
    private Vector3 SeparationForce;

    private float MovementSpeedBlend;
    private float BoidLastSpeed;
    private float BoidCurrentSpeed;
    private float BoidNewSpeed;
    private float BoidSpeedTime;
    private float ChangeBoidSpeedTimer;
    private float Timer;

    private void Start()
    {
        RecalculateBoidSpeed();
    }
    public void FollowTarget()
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
            ApplyAllignment(neigbours);
            ApplyCohesion(neigbours);
        }
        if (distance > SwarmManager.StopDistance)
        {
            ChangeBoidSpeed();
            MoveTowardsTarget();
            RotateTowardsTarget();
        }
        else
        {
            StopMove();
            StopRotate();
        }
    }
    private void RecalculateBoidSpeed()
    {
        Timer = 0.0f;
        ChangeBoidSpeedTimer = 0.0f;
        BoidSpeedTime = Random.Range(2, 5);
        BoidLastSpeed = BoidCurrentSpeed;
        BoidNewSpeed = Random.Range(SwarmManager.MovementSpeed - 1, SwarmManager.MovementSpeed + 1);
    }
    private void ChangeBoidSpeed()
    {
        if (Timer >= BoidSpeedTime)
        {
            RecalculateBoidSpeed();
        }
        if (ChangeBoidSpeedTimer <= 1.0f)
        {
            BoidCurrentSpeed = Mathf.Lerp(BoidLastSpeed, BoidNewSpeed, ChangeBoidSpeedTimer);
        }
        Timer += Time.deltaTime;
        ChangeBoidSpeedTimer += Time.deltaTime;
    }
    private Collider[] GetNeighbours()
    {
        return Physics.OverlapSphere(transform.position, SwarmManager.DetectionDistance, SwarmManager.EnemyLayerMask);
    }
    private void CalculateSeparationForce(Collider[] neighbours)
    {
        foreach (Collider neighbour in neighbours)
        {
            if (neighbour.transform == transform) continue;
            Vector3 dir = neighbour.transform.position - transform.position;
            float distance = dir.magnitude;
            Vector3 away = -dir.normalized;
            if (distance > 0)
            {
                SeparationForce += (away / distance) * SwarmManager.SeparationWeight;
            }
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
        {
            neighboursForward.Normalize();
        }
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
        averagePosition /= neighbours.Length;
        Vector3 cohesionDir = (averagePosition - transform.position).normalized;
        SeparationForce += cohesionDir * SwarmManager.CohesionWeight;
    }
    private void MoveTowardsTarget()
    {
        Direction = Direction.normalized;
        transform.position += BoidCurrentSpeed * Time.deltaTime * (Direction + SeparationForce).normalized;
        MovementSpeedBlend = Mathf.Lerp(MovementSpeedBlend, 1, Time.deltaTime * BoidCurrentSpeed);
        //ZombieAnimator.SetFloat("Speed", MovementSpeedBlend);
    }
    private void RotateTowardsTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), Time.deltaTime * SwarmManager.RotationSpeed);
    }
    private void StopMove()
    {
        MovementSpeedBlend = Mathf.Lerp(MovementSpeedBlend, 0, Time.deltaTime * BoidCurrentSpeed);
        //ZombieAnimator.SetFloat("Speed", MovementSpeedBlend);
    }
    private void StopRotate()
    {
        transform.rotation = Quaternion.LookRotation(Direction);
    }
}
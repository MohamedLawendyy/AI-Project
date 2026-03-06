using UnityEngine;
public class A_BoidFollowLeader : MonoBehaviour
{
    private float BoidLastSpeed = 0.0f;
    private float BoidCurrentSpeed = 0.0f;
    private float BoidNewSpeed = 0.0f;
    private float BoidSpeedTime = 0.0f;
    private float Timer;
    private Rigidbody RB;

    private void Awake()
    {
        BoidCurrentSpeed = A_ZombieFlockManager.instance.MovementSpeed;
        RB = GetComponent<Rigidbody>();
        //transform.GetChild(0).GetComponent<Animator>().speed = Random.Range(1, 4);
    }
    private void Update()
    {
        if (!A_ZombieFlockManager.instance.isAttacking)
        {
            ChangeBoidSpeed();
        }
    }
    private void FixedUpdate()
    {
        if (!A_ZombieFlockManager.instance.isAttacking)
        {
            if (RB.isKinematic)
                RB.isKinematic = false;
            FollowLeader();
        }
    }
    private void FollowLeader()
    {
        RB.linearVelocity = (A_ZombieFlockManager.instance.CohesionWeight * Cohesion())
                          + (A_ZombieFlockManager.instance.SeparationWeight * Seperation())
                          + (A_ZombieFlockManager.instance.AlignmentWeight * Alignment());
        transform.LookAt(A_ZombieFlockManager.instance.Leader.transform);
    }
    private void RecalculateBoidSpeed()
    {
        Timer = 0.0f;
        BoidSpeedTime = Random.Range(2, 5);
        BoidLastSpeed = BoidCurrentSpeed;
        BoidNewSpeed = Random.Range(A_ZombieFlockManager.instance.MovementSpeed - 0.3f, A_ZombieFlockManager.instance.MovementSpeed + 0.3f);
    }
    private void ChangeBoidSpeed()
    {
        if (Timer >= BoidSpeedTime)
            RecalculateBoidSpeed();

        if (Timer <= 0.5f)
            BoidCurrentSpeed = Mathf.Lerp(BoidLastSpeed, BoidNewSpeed, Mathf.Clamp01(Timer * 2.0f));
        else
            BoidCurrentSpeed = BoidNewSpeed;

        Timer += Time.deltaTime;
    }
    private Vector3 Cohesion()
    {
        Vector3 AveragePosition = Vector3.zero;
        foreach (GameObject boid in A_ZombieFlockManager.instance.Boids)
        {
            if (boid != this.gameObject)
                AveragePosition += boid.transform.position;
        }
        return (AveragePosition / (A_ZombieFlockManager.instance.Count - 1)) - transform.position;
    }
    private Vector3 Alignment()
    {
        Vector3 AverageForward = Vector3.zero;
        foreach (GameObject boid in A_ZombieFlockManager.instance.Boids)
        {
            if (boid != this.gameObject)
                AverageForward += A_ZombieFlockManager.instance.Leader.transform.position - boid.transform.position;
        }
        return (AverageForward / (A_ZombieFlockManager.instance.Count - 1)) * BoidCurrentSpeed;
    }
    private Vector3 Seperation()
    {
        int nAvoid = 0;
        Vector3 AverageAvoidPosition = Vector3.zero;
        foreach (GameObject boid in A_ZombieFlockManager.instance.Boids)
        {
            if (A_ZombieFlockManager.instance.Boids.Contains(gameObject) && Vector3.Distance(transform.position, boid.transform.position) < A_ZombieFlockManager.instance.SeperationDistance)
            {
                AverageAvoidPosition += (transform.position - boid.transform.position).normalized * A_ZombieFlockManager.instance.SeperationDistance;
                nAvoid++;
            }
        }
        if (nAvoid == 0)
            return Vector3.zero;
        else
            return (AverageAvoidPosition / nAvoid);
    }
}
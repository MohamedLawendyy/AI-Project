using UnityEngine;
public class A_BoidBehaviourManager : MonoBehaviour
{
    private Rigidbody RB;

    private float BoidLastSpeed;
    private float BoidCurrentSpeed;
    private float BoidNewSpeed;
    private float BoidSpeedTime;
    private float ChangeBoidSpeedTimer;
    private float Timer;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        //transform.GetChild(0).GetComponent<Animator>().speed = Random.Range(1, 4);
    }
    private void LateUpdate()
    {
        //ChangeBoidSpeed();
    }
    private void Update()
    {
        //if (!A_ZombieFlockManager.instance.isAttacking)
        {
            //if (RB.isKinematic)
                //RB.isKinematic = false;
            FollowLeader();
        }
    }
    private void FollowLeader()
    {
        RB.linearVelocity = (A_ZombieFlockManager.instance.CohesionWeight * Cohesion())
                          + (A_ZombieFlockManager.instance.SeparationWeight * Seperation())
                          + (A_ZombieFlockManager.instance.AlignmentWeight * Alignment());
        transform.LookAt(A_ZombieFlockManager.instance.Leader);
    }

    private void RecalculateBoidSpeed()
    {
        Timer = 0.0f;
        ChangeBoidSpeedTimer = 0.0f;
        BoidSpeedTime = Random.Range(2, 5);
        BoidLastSpeed = BoidCurrentSpeed;
        BoidNewSpeed = Random.Range(A_ZombieFlockManager.instance.MovementSpeed - 0.5f, A_ZombieFlockManager.instance.MovementSpeed + 0.5f);
    }
    private void ChangeBoidSpeed()
    {
        if (Timer >= BoidSpeedTime)
            RecalculateBoidSpeed();

        if (ChangeBoidSpeedTimer <= 1.0f)
            BoidCurrentSpeed = Mathf.Lerp(BoidLastSpeed, BoidNewSpeed, ChangeBoidSpeedTimer);

        Timer += Time.deltaTime;
        ChangeBoidSpeedTimer += Time.deltaTime;
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
                AverageForward += A_ZombieFlockManager.instance.Leader.position - boid.transform.position;
        }
        return (AverageForward / (A_ZombieFlockManager.instance.Count - 1)) * BoidCurrentSpeed;
    }
    private Vector3 Seperation()
    {
        int nAvoid = 0;
        Vector3 AverageAvoidPosition = Vector3.zero;
        for (int i = 0; i < A_ZombieFlockManager.instance.Count; i++)
        {
            if (A_ZombieFlockManager.instance.Boids[i] != this.gameObject && Vector3.Distance(transform.position, A_ZombieFlockManager.instance.Boids[i].transform.position) < A_ZombieFlockManager.instance.SeperationDistance)
            {
                AverageAvoidPosition += (transform.position - A_ZombieFlockManager.instance.Boids[i].transform.position).normalized * A_ZombieFlockManager.instance.SeperationDistance;
                nAvoid++;
            }
        }
        if (nAvoid == 0)
            return Vector3.zero;
        else
            return (AverageAvoidPosition / nAvoid);
    }
}
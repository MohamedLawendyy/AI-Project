using UnityEngine;
public class BoidManager : MonoBehaviour
{
    private GameObject[] Boids;
    private Rigidbody RB;

    private float BoidLastSpeed;
    private float BoidCurrentSpeed;
    private float BoidNewSpeed;
    private float BoidSpeedTime;
    private float ChangeBoidSpeedTimer;
    private float Timer;

    private void Start()
    {
        Boids = A_boidmanager_x.instance.Boids;
        RB = GetComponent<Rigidbody>();
        //transform.GetChild(0).GetComponent<Animator>().speed = Random.Range(1, 4);
    }
    private void FixedUpdate()
    {
        ChangeBoidSpeed();
        RB.linearVelocity = (A_boidmanager_x.instance.CohesionWeight * Cohesion())
                          + (A_boidmanager_x.instance.SeparationWeight * Seperation())
                          + (A_boidmanager_x.instance.AlignmentWeight * Alignment());
        transform.LookAt(A_boidmanager_x.instance.Goal.transform);
    }

    private void RecalculateBoidSpeed()
    {
        Timer = 0.0f;
        ChangeBoidSpeedTimer = 0.0f;
        BoidSpeedTime = Random.Range(2, 5);
        BoidLastSpeed = BoidCurrentSpeed;
        BoidNewSpeed = Random.Range(A_boidmanager_x.instance.Speed - 0.5f, A_boidmanager_x.instance.Speed + 0.5f);
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
    private Vector3 Cohesion()
    {
        Vector3 AveragePosition = Vector3.zero;
        foreach (GameObject boid in Boids)
        {
            if (boid != this.gameObject)
                AveragePosition += boid.transform.position;
        }
        return (AveragePosition / (Boids.Length - 1)) - transform.position;
    }
    private Vector3 Alignment()
    {
        Vector3 AverageForward = Vector3.zero;
        foreach (GameObject boid in Boids)
        {
            if (boid != this.gameObject)
                //AverageForward += boid.transform.forward;
                AverageForward += A_boidmanager_x.instance.Goal.transform.position - boid.transform.position;
        }
        return (AverageForward / (Boids.Length - 1)) * BoidCurrentSpeed;
    }
    private Vector3 Seperation()
    {
        int nAvoid = 0;
        Vector3 AverageAvoidPosition = Vector3.zero;
        for (int i = 0; i < Boids.Length; i++)
        {
            if (Boids[i] != this.gameObject && Vector3.Distance(transform.position, Boids[i].transform.position) < A_boidmanager_x.instance.SeperationDistance)
            {
                AverageAvoidPosition += (transform.position - Boids[i].transform.position).normalized * A_boidmanager_x.instance.SeperationDistance;
                nAvoid++;
            }
        }
        if (nAvoid == 0)
            return Vector3.zero;
        else
            return (AverageAvoidPosition / nAvoid);
    }
}
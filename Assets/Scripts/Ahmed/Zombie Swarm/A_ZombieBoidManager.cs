//using UnityEngine;
//public class A_ZombieBoidManager : MonoBehaviour
//{
//    private GameObject[] Boids;
//    private Rigidbody RB;
//    private float Speed;
//    void Start()
//    {
//        Boids = A_ZombieSwarmManager.instance.ZombieBoids;
//        RB = GetComponent<Rigidbody>();
//        //transform.GetChild(0).GetComponent<Animator>().speed = Random.Range(1, 4);
//        //Speed = Random.Range(0.5f, 1.5f);
//        Speed = 2.0f;
//    }
//    void FixedUpdate()
//    {
//        Vector3 BoidVelocity = Cohesion() + Seperation() + Alignment();
//        BoidVelocity.y = -9.8f;
//        RB.linearVelocity = BoidVelocity;
//        //transform.LookAt(A_ZombieSwarmManager.instance.Goal.transform);
//        Vector3 forward = A_ZombieSwarmManager.instance.Goal.transform.position - transform.position;
//        forward.y = 0.0f;
//        transform.rotation = Quaternion.LookRotation(forward);
//    }
//    private Vector3 Cohesion()
//    {
//        Vector3 AveragePosition = Vector3.zero;
//        foreach (GameObject boid in Boids)
//        {
//            if (boid != this.gameObject)
//                AveragePosition += boid.transform.position;
//        }
//        return (AveragePosition / (Boids.Length - 1)) - transform.position;
//    }
//    private Vector3 Alignment()
//    {
//        Vector3 AverageForward = Vector3.zero;
//        foreach (GameObject boid in Boids)
//        {
//            if (boid != this.gameObject)
//            {
//                //AverageForward += boid.transform.forward;
//                AverageForward += A_ZombieSwarmManager.instance.Goal.transform.position - boid.transform.position;
//            }
//        }
//        return (AverageForward / (Boids.Length - 1)) * Speed;
//    }
//    private Vector3 Seperation()
//    {
//        int nAvoid = 0;
//        Vector3 AverageAvoidPosition = Vector3.zero;
//        for (int i = 0; i < Boids.Length; i++)
//        {
//            if (Boids[i] != gameObject && Vector3.Distance(transform.position, Boids[i].transform.position) < A_ZombieSwarmManager.instance.SeperationDistance)
//            {
//                AverageAvoidPosition += (transform.position - Boids[i].transform.position).normalized * A_ZombieSwarmManager.instance.SeperationDistance;
//                nAvoid++;
//            }
//        }
//        if (nAvoid == 0)
//        {
//            return Vector3.zero;
//        }
//        else
//        {
//            return (AverageAvoidPosition / nAvoid);
//        }
//    }
//}
using System.Collections.Generic;
using UnityEngine;
public class A_ZombieFlockManager : MonoBehaviour
{
    public static A_ZombieFlockManager instance;

    [Header("Boid Settings")]
    public float FormationSpeed = 1.0f;
    public float MovementSpeed = 4.0f;
    public float RotationSpeed = 8.0f;
    public float StopDistance = 3.0f;
    public LayerMask EnemyLayerMask;

    [Header("Flock Settings")]
    public float DetectionDistance = 5.0f;
    public float SeparationWeight = 1.0f;
    public float AlignmentWeight = 1.0f;
    public float CohesionWeight = 1.0f;

    [Header("Swarm Settings")]
    public int Count;
    [Range(0, 10)] public float SeperationDistance;
    [SerializeField] private Vector3 Bounds;

    [Header("References")]
    public GameObject Goal;
    public GameObject Leader;
    public GameObject LastLeader;
    [SerializeField] private GameObject BoidMesh;

    [HideInInspector] public bool isAttacking;
    [HideInInspector] public List<GameObject> Boids;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        isAttacking = false;
        CreateBoids();
    }
    private void CreateBoids()
    {
        Boids = new List<GameObject>();
        for (int i = 0; i < Count; i++)
        {
            Vector3 Position = new(
                Random.Range(-Bounds.x + transform.position.x, Bounds.x + transform.position.x),
                1f,
                Random.Range(-Bounds.z + transform.position.z, Bounds.z + transform.position.z)
                );
            Boids.Add(Instantiate(BoidMesh, Position, Quaternion.identity));
        }
        //Leader = Boids[0];
    }
    public void KillBoid(GameObject boid)
    {
        if (Boids.Contains(gameObject))
        {
            Boids.Remove(gameObject);
            Count--;
            Destroy(boid, 3.0f);
        }
    }
}
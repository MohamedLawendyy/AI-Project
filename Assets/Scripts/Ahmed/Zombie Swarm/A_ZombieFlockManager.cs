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
    public Transform Target;
    public Transform Leader;
    [SerializeField] private GameObject BoidMesh;

    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public GameObject[] Boids;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        CreateBoids();
    }
    private void CreateBoids()
    {
        Boids = new GameObject[Count];
        for (int i = 0; i < Count; i++)
        {
            Vector3 Position = new(
                Random.Range(-Bounds.x + transform.position.x, Bounds.x + transform.position.x),
                0.5f,
                Random.Range(-Bounds.z + transform.position.z, Bounds.z + transform.position.z)
                );
            Boids[i] = Instantiate(BoidMesh, Position, Quaternion.identity);
        }
    }
}
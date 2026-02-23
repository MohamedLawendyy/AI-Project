using UnityEngine;
/*
public class A_ZombieSwarmManager : MonoBehaviour
{
    public static A_ZombieSwarmManager instance;
    public GameObject Goal;
    [HideInInspector] public GameObject[] ZombieBoids;
    [SerializeField] private GameObject ZombiePrefab;
    [SerializeField] private int Count;
    [SerializeField] private Vector3 BoundsCenter;
    [Range(0, 10)] public float SeperationDistance;
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        CreateBoids();
    }
    private void CreateBoids()
    {
        ZombieBoids = new GameObject[Count];
        for (int i = 0; i < Count; i++)
        {
            Vector3 Position = new(
                Random.Range(-BoundsCenter.x, BoundsCenter.x),
                Random.Range(-BoundsCenter.y, BoundsCenter.y),
                Random.Range(-BoundsCenter.z, BoundsCenter.z)
                );
            ZombieBoids[i] = Instantiate(ZombiePrefab, Position, Quaternion.identity);
        }
    }
}*/
public class A_ZombieSwarmManager : MonoBehaviour
{
    private A_ZombieBoid[] ZombieBoids;

    [Header("Swarm Settings")]
    [SerializeField] private int Count;
    [SerializeField] private Vector3 BoundsCenter;

    [Header("Boid Settings")]
    public float MovementSpeed = 4.0f;
    public float RotationSpeed = 8.0f;
    public float StopDistance = 3.0f;
    public LayerMask EnemyLayerMask;

    [Header("Flock Settings")]
    public float DetectionDistance = 5.0f;
    public float SeparationWeight = 1.0f;
    public float AlignmentWeight = 1.0f;
    public float CohesionWeight = 1.0f;

    [Header("References")]
    [SerializeField] private Transform Target;
    [SerializeField] private GameObject ZombiePrefab;

    private void Awake()
    {
        CreateBoids();
    }
    private void CreateBoids()
    {
        ZombieBoids = new A_ZombieBoid[Count];
        for (int i = 0; i < Count; i++)
        {
            Vector3 Position = new(
                Random.Range(-BoundsCenter.x, BoundsCenter.x),
                transform.position.y,
                Random.Range(-BoundsCenter.z, BoundsCenter.z)
                );
            //ZombieBoids[i]
            GameObject ZombieObject = Instantiate(ZombiePrefab, Position, Quaternion.identity);
            ZombieBoids[i] = ZombieObject.GetComponent<A_ZombieBoid>();
            ZombieBoids[i].SwarmManager = this;
        }
    }
    public Vector3 GetTargetPosition()
    {
        return Target.position;
    }
    public void SetTargetPosition(Vector3 newTarget)
    {
        Target.position = newTarget;
    }
}
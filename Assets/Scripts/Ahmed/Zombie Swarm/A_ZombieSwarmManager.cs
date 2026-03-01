using UnityEngine;
public class A_ZombieSwarmManager : MonoBehaviour
{
    private A_BoidFormationManager[] ZombieBoids;
    private Vector3 LastTargetPosition;

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
    public GameObject Leader;
    private void Awake()
    {
        LastTargetPosition = Target.position;
        CreateBoids();
    }
    private void CreateBoids()
    {
        ZombieBoids = new A_BoidFormationManager[Count];
        for (int i = 0; i < Count; i++)
        {
            Vector3 Position = new(
                Random.Range(-BoundsCenter.x, BoundsCenter.x),
                transform.position.y,
                Random.Range(-BoundsCenter.z, BoundsCenter.z)
                );
            GameObject ZombieObject = Instantiate(ZombiePrefab, Position, Quaternion.identity);
            ZombieBoids[i] = ZombieObject.GetComponent<A_BoidFormationManager>();
            //ZombieBoids[i].SwarmManager = this;
        }
        //Leader = ZombieBoids[0].gameObject;
    }
    public Vector3 GetTargetPosition()
    {
        return LastTargetPosition;
    }
    public void SetTargetPosition(Vector3 newTarget)
    {
        LastTargetPosition = newTarget;
    }
}
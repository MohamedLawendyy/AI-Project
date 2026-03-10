using UnityEngine;
using UnityEngine.AI;
public class A_SwarmSpawnManager : MonoBehaviour
{
    public static A_SwarmSpawnManager instance;

    [Header("Agent Settings")]
    [SerializeField] private float MovementSpeed = 3.5f;
    [SerializeField] private float AngularSpeed = 120.0f;
    [SerializeField] private float Acceleration = 1.0f;
    [SerializeField] private float StopDistance = 3.0f;

    [Header("Spawn Settings")]
    [SerializeField] private int Count;
    [SerializeField] private Vector3 SpawnBounds;

    [Header("References")]
    public GameObject Goal;
    [SerializeField] private GameObject[] ZombiePrefabs;

    private int PrefabCount;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        SpawnSwarm();
    }
    public void SpawnSwarm()
    {
        NavMeshAgent agent;
        PrefabCount = ZombiePrefabs.Length;
        for (int i = 0; i < Count; i++)
        {
            Vector3 Position = new(
                Random.Range(-SpawnBounds.x + transform.position.x, SpawnBounds.x + transform.position.x),
                1f,
                Random.Range(-SpawnBounds.z + transform.position.z, SpawnBounds.z + transform.position.z)
                );
            agent = Instantiate(ZombiePrefabs[Random.Range(0, PrefabCount)], Position, Quaternion.identity).GetComponent<NavMeshAgent>();
            agent.speed = MovementSpeed;
            agent.angularSpeed = AngularSpeed;
            agent.stoppingDistance = StopDistance;
            agent.acceleration = Acceleration;
            agent.avoidancePriority = Random.Range(40, 60);
        }
    }
}
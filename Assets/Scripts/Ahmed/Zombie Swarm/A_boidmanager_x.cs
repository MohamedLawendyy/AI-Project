using UnityEngine;
public class A_boidmanager_x : MonoBehaviour
{
    public static A_boidmanager_x instance;
    public GameObject[] Boids;
    public GameObject Goal;

    public float SeparationWeight = 1.0f;
    public float AlignmentWeight = 1.0f;
    public float CohesionWeight = 1.0f;
    public float Speed = 1.0f;

    [SerializeField] private GameObject BoidMesh;
    [SerializeField] private int Count;
    [SerializeField] private Vector3 Bounds;
    [Range(0, 10)] public float SeperationDistance;
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        Boids = new GameObject[Count];
        for (int i = 0; i < Count; i++)
        {
            Vector3 Position = new(
                Random.Range(-Bounds.x, Bounds.x),
                0.5f,
                Random.Range(-Bounds.z, Bounds.z)
                );
            Boids[i] = Instantiate(BoidMesh, Position, Quaternion.identity);
        }
    }
}
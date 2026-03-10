using UnityEngine;
using UnityEngine.VFX;

public class L_BossLaser : MonoBehaviour
{
    public VisualEffect laserVFX;
    public Transform laserOrigin;

    public float maxLength = 20f;


    void Start()
    {
        laserVFX.Stop();
    }
    void Update()
    {
        Ray ray = new Ray(laserOrigin.position, laserOrigin.forward);
        RaycastHit hit;

        float length = maxLength;

        if (Physics.Raycast(ray, out hit, maxLength))
        {
            length = hit.distance;

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player Hit");
            }
        }

        laserVFX.SetFloat("BeamLength", length);
    }

    public void StartLaser()
    {
        laserVFX.SetBool("FireLaser", true);
    }

    public void StopLaser()
    {
        laserVFX.SetBool("FireLaser", false);
    }
}

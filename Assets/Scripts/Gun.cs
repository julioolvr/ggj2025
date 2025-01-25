using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform BulletSpawnPoint;

    [SerializeField] private TrailRenderer BulletTrail;

    [SerializeField] private float ShootDelay = 0.5f;

    [SerializeField] private LayerMask Mask;

    [SerializeField] private float BulletSpeed = 100;

    private float LastShootTime;


    private void OnEnable()
    {
        InputData.Instance.RightHandInputData.TriggerButtonPressed += Shoot;
    }

    private void OnDisable()
    {
        InputData.Instance.RightHandInputData.TriggerButtonPressed -= Shoot;
    }

   

    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            Vector3 direction = -transform.right;

            if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue, Mask))
            {
                Debug.Log("Instancia objeto BALA -> Choco con algo");
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
                
                hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;

                LastShootTime = Time.time;

            }
            // this has been updated to fix a commonly reported problem that you cannot fire if you would not hit anything
            else
            {
                Debug.Log("Instancia objeto BALA -> NO CHOCO");
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + direction * 100, Vector3.zero, false));

                LastShootTime = Time.time;
            }
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        Trail.transform.position = HitPoint;

        if (MadeImpact)
        {
            //Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
        }

        Destroy(Trail.gameObject, Trail.time);
    }


    
}

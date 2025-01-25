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
    [SerializeField] public LayerMask bubbleLayer;

    private float LastShootTime;

    public bool isRightHand;
    public AudioClip[] shootingSounds;
    private AudioSource audioSource;

    public float vibrationDuration = .25f;  // Duraci�n de la vibraci�n en segundos
    public float vibrationStrength = .5f;  // Intensidad de la vibraci�n (0 a 1)
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        InputData.Instance.RightHandInputData.TriggerButtonPressed += ShootRight;
        InputData.Instance.LeftHandInputData.TriggerButtonPressed += ShootLeft;
    }

    private void OnDisable()
    {
        InputData.Instance.RightHandInputData.TriggerButtonPressed -= ShootRight;
        InputData.Instance.LeftHandInputData.TriggerButtonPressed -= ShootLeft;
    }

    private void ShootRight()
    {
        if (isRightHand)
        {
            Shoot();
            StartCoroutine(HapticPulse(vibrationDuration, vibrationStrength, OVRInput.Controller.RTouch));
        }
    }

    private void ShootLeft()
    {
        if (!isRightHand)
        {
            Shoot();
            StartCoroutine(HapticPulse(vibrationDuration, vibrationStrength, OVRInput.Controller.LTouch));
        }

    }

    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            Vector3 direction = -transform.right;
            PlayRandomShootingSound();

            if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue, Mask))
            {
                Debug.Log("Instancia objeto BALA -> Choco con algo");
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

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

            if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit bubbleHit, Mathf.Infinity, bubbleLayer))
            {
                // Trigger the bubble's pop method if hit
                Bubble bubble = hit.collider.GetComponent<Bubble>();
                if (bubble != null)
                {
                    bubble.PopBubble(false); // Call your bubble's pop effect
                }
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

    private void PlayRandomShootingSound()
    {
        if (shootingSounds.Length > 0)
        {
            AudioClip randomClip = shootingSounds[UnityEngine.Random.Range(0, shootingSounds.Length)];
            audioSource.PlayOneShot(randomClip);
        }
    }


    IEnumerator HapticPulse(float duration, float strength, OVRInput.Controller controller)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            OVRInput.SetControllerVibration(strength, strength, controller);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        OVRInput.SetControllerVibration(0, 0, controller);  // Detener vibraci�n
    }
}

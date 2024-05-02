using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurretController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] AudioSource audiosource;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float minShootInterval = 1f;
    [SerializeField] float maxShootInterval = 2f;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float shootDuration = 0.5f;
    [SerializeField] float maxBulletDistance = 10f;
    [SerializeField] AudioClip takedamage;
    [SerializeField] Image fillImage;
    private EnemyHealthController enemyHealthController;

    private float shootTimer = 0f;
    private float currentShootInterval;
    private bool isShooting = false;
    private Animator turretAnimator;
    private int shotsReceived = 0;
    private SpriteRenderer turretRenderer;

    void Start()
    {
        turretAnimator = GetComponent<Animator>();
        turretRenderer = GetComponent<SpriteRenderer>();
        currentShootInterval = Random.Range(minShootInterval, maxShootInterval);
        enemyHealthController = GetComponent<EnemyHealthController>();
        audiosource = GetComponent<AudioSource>();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (isShooting)
        {
            if (shootTimer >= shootDuration)
            {
                if (turretAnimator != null)
                {
                    turretAnimator.SetBool("CanShoot", false);
                }
                isShooting = false;
                shootTimer = 0f;
            }
        }
        else
        {
            if (shootTimer >= currentShootInterval)
            {
                Shoot();
                currentShootInterval = Random.Range(minShootInterval, maxShootInterval);
                shootTimer = 0f;
            }
        }
    }

    void Shoot()
    {
        if (turretAnimator != null)
        {
            turretAnimator.SetBool("CanShoot", true);
        }

        isShooting = true;

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.left * bulletSpeed;

        StartCoroutine(DestroyBulletAfterDistance(bullet));
    }

    IEnumerator DestroyBulletAfterDistance(GameObject bullet)
    {
        Vector3 initialPosition = bullet.transform.position;
        float distanceTraveled = 0f;

        while (true)
        {
            distanceTraveled = Vector3.Distance(initialPosition, bullet.transform.position);

            if (distanceTraveled >= maxBulletDistance)
            {
                if (bullet != null)
                {
                    Destroy(bullet);
                    break;
                }
                else
                {
                    break;
                }
            }

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Shoot"))
        {
            float fillAmount = 0.01f;
            fillImage.fillAmount += fillAmount;
            Destroy(other.gameObject);
            enemyHealthController.TakeDamage(1);
        }
        if (other.CompareTag("UltimateShoot"))
        {
            float fillAmount = 0.09f;
            fillImage.fillAmount += fillAmount;
            audiosource.PlayOneShot(takedamage);
            enemyHealthController.TakeDamage(15);
        }
    }
}

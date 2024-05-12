using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurretController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    public AudioSource audiosource;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float minShootInterval = 1f;
    [SerializeField] float maxShootInterval = 2f;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float shootDuration = 0.5f;
    [SerializeField] float maxBulletDistance = 10f;
    public AudioClip takedamage;
    [SerializeField] Image fillImage;
    private EnemyHealthController enemyHealthController;
    private float shootTimer = 0f;
    private float currentShootInterval;
    private bool isShooting = false;
    private Animator turretAnimator;
    private SpriteRenderer turretRenderer;
    private GameObject player;

    void Start()
    {
        turretAnimator = GetComponent<Animator>();
        turretRenderer = GetComponent<SpriteRenderer>();
        currentShootInterval = Random.Range(minShootInterval, maxShootInterval);
        enemyHealthController = GetComponent<EnemyHealthController>();
        audiosource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Function to handle collision with player's bullets
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Shoot"))
        {
            fillImage.fillAmount += 0.02f;
            Destroy(other.gameObject);
            enemyHealthController.TakeDamage(1);
        }
        if (other.CompareTag("UltimateShoot"))
        {
            fillImage.fillAmount += 0.09f;
            audiosource.PlayOneShot(takedamage);
            enemyHealthController.TakeDamage(15);
        }
    }

    // Main update loop
    void Update()
    {
        Vector3 playerDirection = player.transform.position - transform.position;

        if (playerDirection.x > 0)
        {
            turretRenderer.flipX = true;
            spawnPoint.localPosition = new Vector3(-spawnPoint.localPosition.x, spawnPoint.localPosition.y, spawnPoint.localPosition.z);
        }
        else
        {
            turretRenderer.flipX = false;
            spawnPoint.localPosition = new Vector3(Mathf.Abs(spawnPoint.localPosition.x), spawnPoint.localPosition.y, spawnPoint.localPosition.z);
        }

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

    // Function to handle shooting behavior
    void Shoot()
    {
        if (turretAnimator != null)
        {
            turretAnimator.SetBool("CanShoot", true);
        }

        isShooting = true;

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (turretRenderer.flipX)
        {
            rb.velocity = Vector2.right * bulletSpeed;
        }
        else
        {
            rb.velocity = Vector2.left * bulletSpeed;
        }

        StartCoroutine(DestroyBulletAfterDistance(bullet));
    }

    // Coroutine to destroy bullet after traveling a certain distance
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
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalBossController : MonoBehaviour
{
    [SerializeField] private Image fillImage; // Imagen de barra de la ultimate
    [SerializeField] private AudioSource deadAudioSource;
    [SerializeField] private AudioSource audioSource; // AudioSource para la animaci�n Active2

    private BossHealth enemyHealthController;

    public Animator animator; // Referencia al componente Animator del jefe
    public GameObject projectilePrefab; // Prefab del proyectil para Active1
    public GameObject multipleProjectilePrefab; // Prefab del proyectil para Active3
    public Transform[] spawnPoints; // Puntos de generaci�n de los proyectiles m�ltiples
    public Transform shotController; // Controlador de disparo
    public float attackInterval = 1f; // Intervalo entre cada r�faga de disparo
    public int projectilesPerAttack = 6; // Cantidad de proyectiles por r�faga
    public float projectileSpeedMin = 5f; // Velocidad m�nima del proyectil
    public float projectileSpeedMax = 15f; // Velocidad m�xima del proyectil
    public float active1Duration = 5f; // Duraci�n de la animaci�n Active1
    public float active2Duration = 5f; // Duraci�n de la animaci�n Active2
    public float active3Duration = 5f; // Duraci�n de la animaci�n Active3
    public float cooldownDuration = 10f; // Duraci�n del enfriamiento entre ataques
    public float projectileAngle = 35f; // �ngulo de rotaci�n del proyectil

    private void Start()
    {
        enemyHealthController = GetComponent<BossHealth>();
        animator.SetBool("Idlemov", true);

        // Comenzar la rutina de ataques
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (!animator.GetBool("Dead"))
        {
            // Esperar el tiempo de enfriamiento entre ataques
            yield return new WaitForSeconds(cooldownDuration);

            // Seleccionar aleatoriamente entre Active1, Active2 y Active3
            int randomAnimation = Random.Range(0, 3);
            switch (randomAnimation)
            {
                case 0:
                    StartCoroutine(Active1Routine());
                    break;
                case 1:
                    StartCoroutine(Active2Routine());
                    break;
                case 2:
                    StartCoroutine(Active3Routine());
                    break;
            }

            // Esperar el tiempo de duraci�n de la animaci�n
            float duration = randomAnimation == 0 ? active1Duration : (randomAnimation == 1 ? active2Duration : active3Duration);
            yield return new WaitForSeconds(duration);

            // Restablecer la animaci�n a Idle
            animator.SetBool("Active1", false);
            animator.SetBool("Active2", false);
            animator.SetBool("Active3", false);
            animator.SetBool("Idlemov", true);
        }
    }

    private IEnumerator Active1Routine()
    {
        // Cambiar animaciones para el ataque
        animator.SetBool("Idlemov", false);
        animator.SetBool("Active1", true);

        yield return new WaitForSeconds(0.1f); // Peque�o retraso para asegurar que la animaci�n se active correctamente

        // Lanzar proyectiles durante la animaci�n Active1
        for (int i = 0; i < projectilesPerAttack; i++)
        {
            // Generar una posici�n aleatoria dentro del rango de disparo
            Vector3 randomTarget = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f);

            // Calcular la direcci�n hacia la posici�n aleatoria
            Vector3 direction = (randomTarget - shotController.position).normalized;

            // Calcular la rotaci�n para que el proyectil apunte hacia adelante en la direcci�n Z
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle + projectileAngle);

            // Instanciar el proyectil en la posici�n del controlador de disparo con la rotaci�n adecuada
            GameObject projectile = Instantiate(projectilePrefab, shotController.position, rotation);

            // Obtener el componente Rigidbody2D del proyectil
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            // Calcular velocidad aleatoria para el proyectil
            float projectileSpeed = Random.Range(projectileSpeedMin, projectileSpeedMax);

            // Aplicar velocidad al proyectil en la direcci�n calculada
            rb.velocity = direction * projectileSpeed;

            // Destruir el proyectil despu�s de 3 segundos
            Destroy(projectile, 3f);

            // Esperar un tiempo antes de disparar el siguiente proyectil
            yield return new WaitForSeconds(attackInterval);
        }
    }

    private IEnumerator Active2Routine()
    {
        // Cambiar animaciones para el ataque
        animator.SetBool("Idlemov", false);
        animator.SetBool("Active2", true);

        // Reproducir sonido
        if (audioSource != null)
            audioSource.Play();

        // Esperar la duraci�n de la animaci�n
        yield return new WaitForSeconds(active2Duration);

        // Detener la reproducci�n del sonido al finalizar la animaci�n
        if (audioSource != null)
            audioSource.Stop();
    }

    private IEnumerator Active3Routine()
    {
        // Cambiar animaciones para el ataque
        animator.SetBool("Idlemov", false);
        animator.SetBool("Active3", true);

        yield return new WaitForSeconds(0.1f); // Peque�o retraso para asegurar que la animaci�n se active correctamente

        // Lanzar proyectiles desde m�ltiples puntos durante la animaci�n Active3
        for (int i = 0; i < projectilesPerAttack; i++)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                // Generar una posici�n aleatoria dentro del rango de disparo
                Vector3 randomTarget = new Vector3(Random.Range(-10f, 25f), Random.Range(-25f, 10f), 0f);

                // Calcular la direcci�n hacia la posici�n aleatoria
                Vector3 direction = (randomTarget - spawnPoint.position).normalized;

                // Calcular la rotaci�n para que el proyectil apunte hacia adelante en la direcci�n Z
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle + projectileAngle);

                // Instanciar el proyectil en la posici�n del spawn point con la rotaci�n adecuada
                GameObject projectile = Instantiate(multipleProjectilePrefab, spawnPoint.position, rotation);

                // Agregar este proyectil como hijo del spawn point
                projectile.transform.parent = spawnPoint;

                // Obtener el componente Rigidbody2D del proyectil
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

                // Calcular velocidad aleatoria para el proyectil
                float projectileSpeed = Random.Range(projectileSpeedMin, projectileSpeedMax);

                // Aplicar velocidad al proyectil en la direcci�n calculada
                rb.velocity = direction * projectileSpeed;

                // Destruir el proyectil despu�s de 3 segundos
                Destroy(projectile, 3f);
            }

            // Esperar un tiempo antes de disparar el siguiente grupo de proyectiles
            yield return new WaitForSeconds(attackInterval);
        }
    }

    // M�todo para manejar las colisiones con el jefe y las torretas
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Shoot"))
        {
            float fillAmount = 0.02f;
            fillImage.fillAmount += fillAmount;
            Destroy(other.gameObject);
            enemyHealthController.TakeDamage(1);
        }
        if (other.CompareTag("UltimateShoot"))
        {
            float fillAmount = 0.09f;
            fillImage.fillAmount += fillAmount;
            enemyHealthController.TakeDamage(15);
        }
    }
}

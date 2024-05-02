using UnityEngine;
using System.Collections;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    private int currentHealth;

    private SpriteRenderer enemyRenderer;
    private Color originalColor;
    private Color damageColor = new Color(1f, 0.5f, 0.5f, 1f); // Vermell Clar

    void Start()
    {
        currentHealth = maxHealth;
        enemyRenderer = GetComponent<SpriteRenderer>();
        originalColor = enemyRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Cambiar el color del enemigo temporalmente
            StartCoroutine(FlashColor(damageColor));
        }
    }

    void Die()
    {
        // Aquí puedes agregar cualquier lógica adicional cuando el enemigo muera
        Destroy(gameObject);
    }

    IEnumerator FlashColor(Color flashColor)
    {
        // Cambiar el color a flashColor durante 0.5 segundos
        enemyRenderer.color = flashColor;

        yield return new WaitForSeconds(0.5f);

        // Devolver el color original después de 0.5 segundos
        enemyRenderer.color = originalColor;
    }
}

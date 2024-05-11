using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackBossAnim : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("Explode");
        }
        if (collision.CompareTag("UltimateShoot"))
        {
            Destroy(gameObject);
        }
    }
}

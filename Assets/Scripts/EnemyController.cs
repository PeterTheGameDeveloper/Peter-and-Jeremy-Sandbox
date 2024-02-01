using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform playerTransform;
    public float enemyMoveSpeed = 0.05f;
    public int hitPoints = 10;

    SpriteRenderer enemySpriteRenderer;
    Animator enemyAnimator;

    private void Start()
    {
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        enemyAnimator.SetBool("IsHit", false);
        if (playerTransform.position == null)
        {
            return;
        }
        Vector3 positionToMoveTowards = Vector3.Lerp(transform.position, playerTransform.position, enemyMoveSpeed);
        transform.position = positionToMoveTowards;

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damageTaken)
    {
        enemyAnimator.SetBool("IsHit", true);
        StartCoroutine(FlashRed());
        hitPoints -= damageTaken;
    }

    public void TakeKnockback(float knockBack, float direction)
    {
        Vector3 positionToMoveTowards = Vector3.Lerp(transform.position, new Vector3(transform.position.x + (direction * knockBack), transform.position.y, transform.position.z), enemyMoveSpeed);
        transform.position = positionToMoveTowards;
    }

    private IEnumerator FlashRed()
    {
        float timerDuration = 2.0f;
        while (timerDuration > 0)
        {
            timerDuration -= Time.deltaTime;
            if ((timerDuration > 1.5f && timerDuration <= 2.0f) || (timerDuration > 0.5f && timerDuration <= 1.0f))
            {
                enemySpriteRenderer.color = Color.red;
            }
            else if ((timerDuration > 1.0f && timerDuration <= 1.5f) || (timerDuration > 0 && timerDuration <= 0.5f))
            {
                enemySpriteRenderer.color = Color.white;
            }
            yield return null;
        }
        enemySpriteRenderer.color = Color.white;
    }
}

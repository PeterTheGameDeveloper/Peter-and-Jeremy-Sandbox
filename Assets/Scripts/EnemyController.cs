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
        if (playerTransform.position == null)
        {
            return;
        }
        Vector3 positionToMoveTowards = Vector3.Lerp(transform.position, playerTransform.position, enemyMoveSpeed);
        if (positionToMoveTowards.x < transform.position.x && transform.localScale.x >= 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (positionToMoveTowards.x >= transform.position.x && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        transform.position = positionToMoveTowards;
        DetermineAnimation();

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damageTaken)
    {
        enemyAnimator.SetTrigger("IsHit");
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

    private void DetermineAnimation()
    {
        Vector3 distanceFromPlayer = transform.position - playerTransform.transform.position;
        if (Mathf.Abs(distanceFromPlayer.x) <= 0.5f && Mathf.Abs(distanceFromPlayer.y) <= 0.5f)
        {
            enemyAnimator.SetBool("IsIdle", true);
            enemyAnimator.SetBool("IsWalking", false);
        }
        else
        {
            enemyAnimator.SetBool("IsIdle", false);
            enemyAnimator.SetBool("IsWalking", true);
        }
    }
}

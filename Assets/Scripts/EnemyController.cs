using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform playerTransform;
    public float enemyMoveSpeed = 0.05f;
    public int hitPoints = 10;

    SpriteRenderer enemySpriteRenderer;

    private void Start()
    {
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
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
        for (int i = 0; i < 50; i++)
        {
            enemySpriteRenderer.color = Color.red;
            enemySpriteRenderer.color = Color.white;
        }
        hitPoints -= damageTaken;
    }

    public void TakeKnockback(float knockBack, float direction)
    {
        Vector3 positionToMoveTowards = Vector3.Lerp(transform.position, new Vector3(transform.position.x + (direction * knockBack), transform.position.y, transform.position.z), enemyMoveSpeed);
        transform.position = positionToMoveTowards;
    }
}

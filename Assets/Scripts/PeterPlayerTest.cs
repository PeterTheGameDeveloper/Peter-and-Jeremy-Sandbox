using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterPlayerTest : MonoBehaviour
{
    private UIController uiController;
    private Animator playerAnimator;
    private bool isRunning = false;

    public float xMoveSpeed = 0.25f;
    public float yMoveSpeed = 0.25f;

    public int currentWeaponDamage = 5;
    public float currentKnockback = 1.0f;

    Rigidbody2D playerRigidBody;

    private bool inventoryOpen = false;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        uiController = GetComponent<UIController>();
    }

    private void Update()
    {
        playerAnimator.SetBool("IsRunning", false);
        playerAnimator.SetBool("IsAttacking", false);
        Vector3 currentPlayerPosition = transform.position;
        Vector3 currentPlayerScale = transform.localScale;

        if (Input.GetKey(KeyCode.A))
        {
            currentPlayerPosition.x = transform.position.x - xMoveSpeed;
            playerAnimator.SetBool("IsRunning", true);
            if (currentPlayerScale.x >= 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            currentPlayerPosition.x = transform.position.x + xMoveSpeed;
            playerAnimator.SetBool("IsRunning", true);
            if (currentPlayerScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            currentPlayerPosition.y = transform.position.y - yMoveSpeed;
            playerAnimator.SetBool("IsRunning", true);
        }
        if (Input.GetKey(KeyCode.W))
        {
            currentPlayerPosition.y = transform.position.y + yMoveSpeed;
            playerAnimator.SetBool("IsRunning", true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackEnemy(Mathf.Sign(transform.localScale.x));
            playerAnimator.SetBool("IsAttacking", true);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryOpen = !inventoryOpen;
            uiController.Inventory(inventoryOpen);
        }

        transform.position = currentPlayerPosition;
        /*Debug.Log(transform.position);*/
    }

    public void AttackEnemy(float direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, 0.25f);
        if (hit.collider == null)
        {
            return;
        }
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Hit!");
            EnemyController enemyToAttack = hit.transform.GetComponent<EnemyController>();
            enemyToAttack.TakeKnockback(currentKnockback, direction);
            enemyToAttack.TakeDamage(currentWeaponDamage);
        }
    }
}

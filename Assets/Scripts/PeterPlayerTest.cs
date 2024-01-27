using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterPlayerTest : MonoBehaviour
{
    private Animator playerAnimator;
    private bool isRunning = false;

    public float xMoveSpeed = 5.0f;
    public float yMoveSpeed = 2.5f;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        playerAnimator.SetBool("IsRunning", false);
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

        transform.position = currentPlayerPosition;
    }
}

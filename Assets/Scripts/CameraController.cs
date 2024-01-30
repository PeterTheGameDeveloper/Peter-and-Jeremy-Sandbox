using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerCharacter;
    public float smoothSpeed = 0.4f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (playerCharacter == null)
        {
            return;
        }
        /*Vector3 desiredPosition = new Vector3(playerCharacter.position.x + offset.x, playerCharacter.position.y + offset.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;*/
        Debug.Log(Camera.main.orthographicSize);

        float cameraSize = Camera.main.orthographicSize;
        float leftBound = transform.position.x - cameraSize;
        float rightBound = transform.position.x + cameraSize;
        float upperBound = transform.position.y + cameraSize;
        float lowerBound = transform.position.y - cameraSize;

        if (playerCharacter.transform.position.x > rightBound)
        {
            transform.position = new Vector3(transform.position.x + (playerCharacter.position.x - rightBound), transform.position.y, transform.position.z);
        }
        if (playerCharacter.transform.position.x < leftBound)
        {
            transform.position = new Vector3(transform.position.x + (playerCharacter.position.x - leftBound), transform.position.y, transform.position.z);
        }
        if (playerCharacter.transform.position.y + 1.5f > upperBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (playerCharacter.position.y + 1.5f - upperBound), transform.position.z);
        }
        if (playerCharacter.transform.position.y - 2.0f < lowerBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (playerCharacter.position.y - 2.0f - lowerBound), transform.position.z);
        }
    }
}

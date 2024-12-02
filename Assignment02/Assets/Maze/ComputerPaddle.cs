using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPaddle : MonoBehaviour
{
    public Transform ball;
    public float speed = 5.0f;
    private float paddleLength = 1.0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;

        float targetZ = ball.position.z;

        float newZ = Mathf.Lerp(currentPosition.z, targetZ, speed * Time.deltaTime);

        newZ = Mathf.Clamp(newZ, -6.0f + paddleLength, 6.0f - paddleLength);

        transform.position = new Vector3(currentPosition.x, currentPosition.y, newZ);
    }
}

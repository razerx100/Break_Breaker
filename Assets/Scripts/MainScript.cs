using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    [SerializeField]
    GameObject border;
    const float BorderPosX = 0, BorderPosY = -0.1f;

    [SerializeField]
    GameObject ball;
    const float BallVelocity = 25;
    const float BallStartX = 0, BallStartY = -33.8f;
    bool ballMoving = false;

    [SerializeField]
    GameObject platform;
    const float PlatformPosX = 0, PlatformPosY = -36.12555f;
    float colliderHalfWidth;

    void Start()
    {
        spawner();
        colliderHalfWidth = platform.GetComponent<BoxCollider2D>().size.x / 2;
    }

    void Update()
    {
        keyInput();
        clampPlatform();
    }

    private void FixedUpdate()
    {
        Vector2 ballOrientation = ball.GetComponent<Rigidbody2D>().velocity.normalized;
        ball.GetComponent<Rigidbody2D>().velocity = ballOrientation * BallVelocity;
    }

    void spawner()
    {
        float z = -Camera.main.transform.position.z;
        border = Instantiate(border);
        platform = Instantiate(platform);
        ball = Instantiate(ball);
    }
    void launchBall()
    {
        Vector2 angle = new Vector2(Mathf.Sin(45), Mathf.Cos(45));
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.AddForce(angle * BallVelocity, ForceMode2D.Impulse);
        ballMoving = true;
    }

    void keyInput()
    {
        if (Input.GetAxis("Submit") > 0 && !ballMoving)
        {
            launchBall();
        }
        Vector3 platformPos = platform.transform.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            platformPos.x += horizontalInput * 25 * Time.deltaTime;
            platform.transform.position = platformPos;
        }
        if(Input.GetAxis("Cancel") > 0)
        {
            Application.Quit();
        }
    }

    void clampPlatform()
    {
        const float LeftBound = -15.4f, RightBound = 15.4f;
        Vector3 platformPos = platform.transform.position;
        if(platformPos.x - colliderHalfWidth < LeftBound)
        {
            platformPos.x = LeftBound + colliderHalfWidth;
        }
        else if(platformPos.x + colliderHalfWidth > RightBound)
        {
            platformPos.x = RightBound - colliderHalfWidth;
        }
        platform.transform.position = platformPos;
    }
}

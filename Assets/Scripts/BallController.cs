using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    float speed;

    bool ball_moving = false;
    private void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (!ball_moving)
        {
            float launch = Input.GetAxis("Submit");
            if (launch > 0)
            {
                rb.velocity = new Vector3(launch, 0.0f, launch) * speed;
                ball_moving = true;
            }
        }
        else
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject gObj = other.gameObject;
        if (gObj.CompareTag("Brick"))
        {
            Destroy(gObj);
        }
        else if (gObj.CompareTag("Brick2"))
        {
            if (gObj.GetComponent<HitCount>().hits == 0)
            {
                gObj.GetComponent<HitCount>().hits++;
            }
            else
            {
                Destroy(gObj);
            }
        }
    }
}

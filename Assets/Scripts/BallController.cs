using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    GameController game_controller;

    [SerializeField]
    BaseController basee;

    bool game_running = true;

    bool ball_moving = false;
    private void FixedUpdate()
    {
        if (game_running)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (!ball_moving)
            {
                float launch = Input.GetAxis("Submit");
                if (launch > 0)
                {
                    rb.velocity = new Vector3(launch, 0.0f, launch) * speed;
                    ball_moving = true;
                    basee.is_ball_moving();
                }
            }
            else
            {
                rb.velocity = rb.velocity.normalized * speed;
            }
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject gObj = other.gameObject;
        if (gObj.CompareTag("Brick"))
        {
            game_controller.add_score(5);
            Destroy(gObj);
            game_controller.decrease_bricks();
        }
        else if (gObj.CompareTag("Brick2"))
        {
            if (gObj.GetComponent<HitCount>().hits == 0)
            {
                gObj.GetComponent<HitCount>().hits++;
            }
            else
            {
                game_controller.add_score(10);
                game_controller.decrease_bricks();
                Destroy(gObj);
            }
        }
    }

    public void is_running(bool flag)
    {
        game_running = flag;
    }
}

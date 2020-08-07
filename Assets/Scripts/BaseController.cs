using UnityEngine;
using Assets.Scripts;

public class BaseController : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    Boundary boundary;

    [SerializeField]
    Rigidbody ball_rb;

    bool game_running = true;
    bool ball_moving = false;

    private void FixedUpdate()
    {
        if (game_running)
        {
            float horizontal_velocity = Input.GetAxis("Horizontal");
            float vertical_velocity = Input.GetAxis("Vertical");

            Rigidbody rb = GetComponent<Rigidbody>();
            move(rb, horizontal_velocity, vertical_velocity);
            if (!ball_moving)
            {
                move(ball_rb, horizontal_velocity, vertical_velocity);
            }
        }
    }

    void move(Rigidbody rb, float horizontal_velocity, float vertical_velocity)
    {
        rb.velocity = new Vector3(horizontal_velocity, 0.0f, vertical_velocity) * speed;
        rb.position = new Vector3(
        Mathf.Clamp(rb.position.x, boundary.min_x, boundary.max_x),
              0.0f,
              rb.position.z
              );
    }

    public void is_running(bool flag)
    {
        game_running = flag;
    }

    public void is_ball_moving()
    {
        ball_moving = true;
    }
}

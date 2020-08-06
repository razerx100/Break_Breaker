using UnityEngine;
using Assets.Scripts;

public class BaseController : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    Boundary boundary;

    private void FixedUpdate()
    {
        float horizontal_velocity = Input.GetAxis("Horizontal");
        float vertical_velocity = Input.GetAxis("Vertical");

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(horizontal_velocity, 0.0f, vertical_velocity) * speed;
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.min_x, boundary.max_x),
            0.0f,
            rb.position.z
            );
    }
}

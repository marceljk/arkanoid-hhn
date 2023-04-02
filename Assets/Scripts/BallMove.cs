using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    public float speed;
    public float maxX;
    public Transform playArea;
    public Vector3 Velocity
    {
        get { return velocity; }
    }

    private Vector3 velocity;
    private int lastCollisionFrame = 0;
    private GameObject lastCollisionObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float maxDist = 0.5f * other.transform.localScale.x + 0.5f * transform.localScale.x;
            float actualDist = transform.position.x - other.transform.position.x;

            float distNorm = actualDist / maxDist;
            Vector3 movement = new Vector3(distNorm * maxX, velocity.y, velocity.z * -1);
            velocity = movement.normalized * speed;
        }
        else if (other.CompareTag("Frame"))
        {
            velocity.x *= -1;
        }
        else if (other.CompareTag("FrameTop"))
        {
            velocity.z *= -1;
        }
        else if (other.CompareTag("Brick") && (lastCollisionObject != other.gameObject || lastCollisionFrame + 2 < Time.frameCount))
        {
            lastCollisionFrame = Time.frameCount;
            lastCollisionObject = other.gameObject;
            Vector3 collision = transform.position - other.ClosestPointOnBounds(transform.position);
            Vector3 reflection = Vector3.Reflect(velocity, collision.normalized);
            velocity = reflection.normalized * speed;
            if (Mathf.Abs(velocity.z) < 1)
            {
                velocity.z = velocity.z < 0 ? -1 : 1;
            }
            other.gameObject.GetComponent<BrickManager>().TakeDamage(1);
        }
    }

    public void StartBall()
    {
        if (velocity.z == 0)
        {
            velocity = new Vector3(0, 0, speed);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime;

        float maxZPosition = playArea.localScale.z * 0.5f * 10 + 2;
        if (transform.position.z < -maxZPosition)
        {
            ResetBall();
            GameManager.instance.Health -= 1;
        }

        if (!GameManager.instance.IsGameRunning)
        {
            ResetBall();
        }
    }

    void ResetBall()
    {
        float limit = playArea.localScale.x * 10 * 0.5f - transform.localScale.x * 0.5f;
        float clampedX = Mathf.Clamp(transform.position.x, -limit, limit);
        transform.position = new Vector3(clampedX, 0.5f, -8.75f);
        velocity = new Vector3(0, 0, 0);
    }
}


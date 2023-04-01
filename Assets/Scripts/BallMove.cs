using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    public float speedZ;
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
            velocity.x = distNorm * maxX;
            velocity.z *= -1;
        }
        else if (other.CompareTag("Frame"))
        {
            velocity.x *= -1;
        }
        else if (other.CompareTag("FrameTop"))
        {
            velocity.z *= -1;
        }
        else if (other.CompareTag("Brick") && (lastCollisionObject != other.gameObject || lastCollisionFrame < Time.frameCount))
        {
            lastCollisionFrame = Time.frameCount;
            lastCollisionObject = other.gameObject;
            Vector3 collision = transform.position - other.ClosestPointOnBounds(transform.position);
            if (Mathf.Abs(collision.x) > Mathf.Abs(collision.z))
            {
                velocity.x *= -1;
                Debug.Log("X");
            }
            else
            {
                velocity.z *= -1;
                Debug.Log("Y");
            }
            other.gameObject.GetComponent<BrickManager>().TakeDamage(1);
        }
        else 
        {
            Debug.Log("Catched!");
            Debug.Log("LastCollision" + lastCollisionFrame);
            Debug.Log("NOW" + Time.frameCount);
        }
    }

    public void StartBall()
    {
        if (velocity.z == 0)
        {
            velocity = new Vector3(0, 0, speedZ);
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
            float limit = playArea.localScale.x * 10 * 0.5f - transform.localScale.x * 0.5f;
            float clampedX = Mathf.Clamp(transform.position.x, -limit, limit);
            transform.position = new Vector3(clampedX, 0.5f, -8.75f);
            // velocity.z *= -1;
            velocity = new Vector3(0, 0, 0);
            GameManager.instance.Health -= 1;
            if (GameManager.instance.Health < 1)
            {
                velocity = new Vector3(0, 0, 0);
            }
        }
    }
}


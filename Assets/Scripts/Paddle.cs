using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Transform playArea;
    public float speed;
    public GameObject ball;

    private float dir = 0;
    private Vector3 originlocalScale;
    private Vector3 originlocalScaleBall;
    private float increasePaddleTimeout = 0f;
    private float increaseBallTimeout = 0f;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            if (other.gameObject.GetComponent<PowerUp>().type == "IncreasePaddle")
            {
                increasePaddleTimeout += 10.0f;
                transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y, transform.localScale.z);
            }
            else if (other.gameObject.GetComponent<PowerUp>().type == "ExtraLive")
            {
                GameManager.instance.Health += 1;
            }
            else if (other.gameObject.GetComponent<PowerUp>().type == "IncreaseBall")
            {
                increaseBallTimeout += 10f;
                ball.transform.localScale *= 1.5f;
            }
            else if (other.gameObject.GetComponent<PowerUp>().type == "DoublePoints")
            {
                GameManager.instance.MultiplyPoints();
            }
            Destroy(other.gameObject);
        }
    }

    void ResetIncreasePaddle()
    {
        transform.localScale = originlocalScale;
    }
    void ResetIncreaseBall()
    {
        ball.transform.localScale = originlocalScaleBall;
    }

    void Start()
    {
        originlocalScale = transform.localScale;
        originlocalScaleBall = ball.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (increasePaddleTimeout <= 0)
        {
            ResetIncreasePaddle();
        }
        else
        {
            increasePaddleTimeout -= Time.deltaTime;
        }

        if (increaseBallTimeout <= 0)
        {
            ResetIncreaseBall();
        }
        else
        {
            increaseBallTimeout -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && GameManager.instance.Health > 0)
        {
            ball.GetComponent<BallMove>().StartBall();
        }
        if (ball.GetComponent<BallMove>().Velocity == new Vector3(0, 0, 0))
        {
            ball.transform.position = new Vector3(transform.position.x, ball.transform.position.y, ball.transform.position.z);
        }
        dir = Input.GetAxis("Horizontal");

        float limit = playArea.localScale.x * 10 * 0.5f - transform.localScale.x * 0.5f;
        float newX = transform.position.x + Time.deltaTime * speed * dir;
        float clampedX = Mathf.Clamp(newX, -limit, limit);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        ;
        //transform.position += new Vector3(Time.deltaTime * speed * dir, 0, 0);
    }
}

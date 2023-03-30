using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Transform playArea;
    public float speed;
    public GameObject ball;

    private float dir = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.instance.Health > 0)
        {
            ball.GetComponent<BallMove>().StartBall();
        }
        if (ball.GetComponent<BallMove>().Velocity == new Vector3(0,0,0))
        {
            ball.transform.position = new Vector3(transform.position.x, ball.transform.position.y, ball.transform.position.z);
        }
        dir = Input.GetAxis("Horizontal");

        float limit = playArea.localScale.x * 10 * 0.5f - transform.localScale.x * 0.5f;
        float newX = transform.position.x + Time.deltaTime * speed * dir;
        float clampedX = Mathf.Clamp(newX, -limit, limit);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        //transform.position += new Vector3(Time.deltaTime * speed * dir, 0, 0);
    }
}

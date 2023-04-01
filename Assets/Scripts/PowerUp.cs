using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speedZ;
    public Transform playArea;
    private Vector3 velocity;
    public string type;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, -speedZ);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime;

        float maxZPosition = playArea.localScale.z * 0.5f * 10 + 2;
        if (transform.position.z < -maxZPosition)
        {
            Destroy(this.gameObject);
        }
    }
}

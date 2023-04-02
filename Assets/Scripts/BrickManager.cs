using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public int health;
    public Material brickStrong;
    public Material brickNormal;
    public Material brickWeak;
    public GameObject powerUp;
    public string powerUpType;

    public void TakeDamage(int amount)
    {
        health -= amount;
        UpdateMaterial();
        GameManager.instance.AddScorePoint(10);
        if (health < 1 )
        {
            if (powerUp != null)
            {
                var powerUpInstance = Instantiate(powerUp, transform.position, Quaternion.identity);
                powerUpInstance.GetComponent<PowerUp>().type = powerUpType;
            }
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if (health == 3)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = brickStrong;
        }
        else if (health == 2)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = brickNormal;
        }
        else if (health == 1)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = brickWeak;
        }
    } 
}

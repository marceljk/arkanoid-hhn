using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public int health;
    public Material brickStrong;
    public Material brickNormal;
    public Material brickWeak;

    public void TakeDamage(int amount)
    {
        health -= amount;
        UpdateMaterial();
        GameManager.instance.Score += 10;
        if (health < 1 )
        {
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

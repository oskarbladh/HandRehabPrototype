using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour
{
    GameManagerScript GameManager;

    void Start()
    {
        GameManager = GameManagerScript.instance;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Mushroom")
        {
            GameManager.AllMushrooms.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}

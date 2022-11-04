using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour
{

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag=="Mushroom"){
            Destroy(other.gameObject);
        }
    }
}

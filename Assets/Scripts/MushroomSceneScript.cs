using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSceneScript : MonoBehaviour
{
    GameObject MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera=GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Hands")){
            //Cameramovement
            MainCamera.transform.position = new Vector3(0f,0.497999996f,-0.597000003f);
        }
    }
     private void OnTriggerExit(Collider other) {
         if(!other.CompareTag("Hands")){
            //Cameramovement
            MainCamera.transform.position = new Vector3(0,0.970000029f,-0.769999981f);
        }
     }
}

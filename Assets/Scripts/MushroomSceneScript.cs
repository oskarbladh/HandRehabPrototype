using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///When mushroom comes near a interactable range then the camera zooms in
///</summary>
public class MushroomSceneScript : MonoBehaviour
{
    [SerializeField]
    GameManagerScript GameManager;
    GameObject MainCamera;

    Vector3 initialPosition;
    Rigidbody mushroomPhysics;

    void Start()
    {
        MainCamera=GameObject.Find("Main Camera");
        initialPosition=transform.position;
        mushroomPhysics=GetComponent<Rigidbody>();
    }


    void Update()
    {
        if(transform.position.z<-0.12f)
        {
            mushroomPhysics.AddForce(transform.forward * 5f);
            // mushroomPhysics.isKinematic = true;
            // transform.position = initialPosition;
            //  mushroomPhysics.isKinematic = false;
            Debug.Log("Position reseted");
        }
         if(transform.position.y<-1f)
         {
            transform.position = initialPosition;
            //transform.rotation = initialtransform.rotation;
            Debug.Log(initialPosition);
         }
          if(transform.position.x<-0.8f || transform.position.x>0.8f)
         {
            transform.position = initialPosition;
            //transform.rotation = initialtransform.rotation;
            Debug.Log(initialPosition);
         }
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Got In Range")){
            //Camera Movement
            //MainCamera.transform.position = new Vector3(0f,0.497999996f,-0.597000003f);
            GameManager.cameraMovementNeeded = true;
            GameManager.mushroomGotOut = false;
            GameManager.startTime = Time.time;
            GameManager.startCamPos=MainCamera.transform.position;
            GameManager.endCamPos=new Vector3(0f,0.497999996f,-0.597000003f);
            GameManager.journeyLength = Vector3.Distance(GameManager.startCamPos, GameManager.endCamPos);
        }
        // else if(!other.CompareTag("Got Out of Range")){
        //     //Cameramovement
        //     MainCamera.transform.position = new Vector3(0,0.970000029f,-0.769999981f);
        // }
    }

}

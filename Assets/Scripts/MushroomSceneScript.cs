using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

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
            GameManager.settingUpLerpValues(true,false,new Vector3(0,0.888999999f,-0.744000018f),37);
            GameManager.mushRoomData = this.GetComponent<MushroomInfo>();
        }
        else if(other.CompareTag("Good Basket"))
        {
            GameManager.updateScore();
            if(GameManager.MushroomsInRange.Count == 0){
            GameManager.settingUpLerpValues(true,true,new Vector3(0,1.09899998f,-0.568000019f),70);
            }
            GameManager.objectIsWithinRadius=false;
            
            //remove the object from both AllMushrooms and MushroomsInRange
            GameManager.MushroomsInRange.Remove(this.gameObject);
            GameManager.AllMushrooms.Remove(this.gameObject);
            Destroy(this.gameObject);
            //Destroy(GetComponent<InteractionBehaviour>());
            //Destroy(GetComponent<InteractionBehaviour>());
            //Destroy(GetComponent<InteractionBehaviour>());
        }
        /////////////////////////////////////////////////////////////////////////
        ////////OUT OF RANGE IS HANDLED IN HandleCameraWhenGotOutOfRange/////////
        /////////////////////////////////////////////////////////////////////////
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

///<summary>
///When mushroom comes near a interactable range then the camera zooms in
///</summary>
public class MushroomSceneScript : MonoBehaviour
{
    //[SerializeField]
    GameManagerScript GameManager;
    GameObject MainCamera;

    Vector3 initialPosition;
    Rigidbody mushroomPhysics;

    float timer = 2f;

    float elapsedTime = 0f;

    bool scalingIsDone=false;

    void Start()
    {
         GameManager=GameManagerScript.instance;
        MainCamera=GameObject.Find("Main Camera");
        initialPosition=transform.position;
        mushroomPhysics=GetComponent<Rigidbody>();
        mushroomPhysics.isKinematic = true;
    }


    void Update()
    {
        // if(transform.position.z<-0.12f)
        // {
        //     mushroomPhysics.AddForce(transform.forward * 5f);
        //     // mushroomPhysics.isKinematic = true;
        //     // transform.position = initialPosition;
        //     //  mushroomPhysics.isKinematic = false;
        //     Debug.Log("Position reseted");
        // }
         if(transform.position.y<-6f)
         {
            transform.position = initialPosition;
            //transform.rotation = initialtransform.rotation;
            //Debug.Log(initialPosition);
         }
        //   if(transform.position.x<63f || transform.position.x>62f)
        //  {
        //     transform.position = initialPosition;
        //     //transform.rotation = initialtransform.rotation;
        //     Debug.Log(initialPosition);
        //  }
      
        
    }
    
    private void OnTriggerEnter(Collider triggerCollider) {
        if(triggerCollider.CompareTag("Got In Range")){
            //Camera Movement
            //MainCamera.transform.position = new Vector3(0f,0.497999996f,-0.597000003f);
            //GameManager.settingUpLerpValues(true,false,new Vector3(0,0.888999999f,-0.744000018f),37);
            GameManager.mushRoomData = this.GetComponent<MushroomInfo>();
           
            mushroomPhysics.isKinematic = false;
            //Debug.Log("Got in Range");
        }
        else if(triggerCollider.CompareTag("Good Basket"))
        {
            
            // // if(!(GameManager.MushroomsInRange.Count > 0)){
            // // GameManager.settingUpLerpValues(true,true,new Vector3(0,1.09899998f,-0.568000019f),70);
            // // }
            // // GameManager.objectIsWithinRadius=false;
            // if(GameManager.MushroomsInRange.Contains(this.gameObject))
            //     GameManager.updateScore();
            // //remove the object from both AllMushrooms and MushroomsInRange
            // transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            // GameManager.MushroomsInRange.Remove(this.gameObject);
            // GameManager.AllMushrooms.Remove(this.gameObject);
            // GameManager.explorationMode=true;
            // GameManager.objectIsSelected=false;
           
            // //Destroy(this.gameObject);'
            // // this.gameObject.transform.parent = triggerCollider.gameObject.transform;
            // // Destroy(GetComponent<CapsuleCollider>());
            // // Destroy(GetComponent<InteractionBehaviour>());
            // Destroy(this.gameObject);
            if(!scalingIsDone){
                transform.localScale *=0.8f;
                scalingIsDone = true;
            }
        
        }
        /////////////////////////////////////////////////////////////////////////
        ////////OUT OF RANGE IS HANDLED IN HandleCameraWhenGotOutOfRange/////////
        /////////////////////////////////////////////////////////////////////////
    }

    private void OnTriggerStay(Collider other) {
         if(other.gameObject.tag=="Good Basket")
        {
            if(elapsedTime<timer){
                elapsedTime+=Time.deltaTime;
                Debug.Log("TIME:"+elapsedTime);
                return;
            }
            // if(!(GameManager.MushroomsInRange.Count > 0)){
            // GameManager.settingUpLerpValues(true,true,new Vector3(0,1.09899998f,-0.568000019f),70);
            // }
            // GameManager.objectIsWithinRadius=false;
            if(GameManager.MushroomsInRange.Contains(this.gameObject))
                GameManager.updateScore();
            //remove the object from both AllMushrooms and MushroomsInRange
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            GameManager.MushroomsInRange.Remove(this.gameObject);
            GameManager.AllMushrooms.Remove(this.gameObject);
            GameManager.explorationMode=true;
            GameManager.objectIsSelected=false;
            elapsedTime=0;
            //Destroy(this.gameObject);'
            this.gameObject.tag = "CollectedMushroom";
            this.gameObject.transform.parent = other.gameObject.transform;
            Destroy(GetComponent<InteractionBehaviour>());
            Destroy(this);
            // Destroy(GetComponent<InteractionBehaviour>());
            //Destroy(this.gameObject);

        }
    }

}

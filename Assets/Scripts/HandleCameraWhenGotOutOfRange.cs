using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///When mushroom goes out of a particular range the camera zooms out
///</summary>
public class HandleCameraWhenGotOutOfRange : MonoBehaviour
{
    ///Instance of the GameManager
    [SerializeField]
    GameManagerScript GameManager;
    GameObject MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera=GameObject.Find("Main Camera");
    }

        private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Mushroom")){
            //Cameramovement
            //MainCamera.transform.position = new Vector3(0,0.970000029f,-0.769999981f);
            GameManager.cameraMovementNeeded = true;
            GameManager.mushroomGotOut = true;
            GameManager.startTime = Time.time;
            GameManager.startCamPos=MainCamera.transform.position;
            GameManager.endCamPos=new Vector3(0,0.970000029f,-0.769999981f);
            GameManager.journeyLength = Vector3.Distance(GameManager.startCamPos, GameManager.endCamPos);
        }
    }
}

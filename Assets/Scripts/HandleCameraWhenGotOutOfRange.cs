using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///MUSHROOM ONLY////////////////////////////////////////////////////
///When MUSHROOM goes out of a particular range the camera zooms out
///</summary>
public class HandleCameraWhenGotOutOfRange : MonoBehaviour
{
    ///Instance of the GameManager
    [SerializeField]
    GameManagerScript GameManager;

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Mushroom")){
            //Cameramovement
            //MainCamera.transform.position = new Vector3(0,0.970000029f,-0.769999981f);
            if(GameManager.MushroomsInRange.Count>0)
            {
                //also acts as a check saying that Mushrooms are still within pickup range
                GameManager.MushroomsInRange.Remove(other.gameObject);
            }else if(GameManager.MushroomsInRange.Count==0)
            {
                GameManager.objectIsWithinRadius=false;
                GameManager.settingUpLerpValues(true,true,new Vector3(0,1.09899998f,-0.568000019f),70);
            }
        }
    }
}

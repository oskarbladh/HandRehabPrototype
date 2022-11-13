using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    [SerializeField]
    GameManagerScript GameManager;
    public float speed=0.7f;
    // Start is called before the first frame update
    void Start()
    {
        //GameManager=GameManagerScript.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.cameraMovementNeeded){
            StartCoroutine(MoveCamera((moveIsDone) => {if(moveIsDone) { 
                GameManager.cameraMovementNeeded=false;
                //make the canvas appear n disappear
                if(GameManager.mushroomGotOut)
                    GameManager.MushroomCanvas.SetActive(false);
                else
                    GameManager.MushroomCanvas.SetActive(true);
                };
            }
        ));
        }
    }

    IEnumerator MoveCamera(System.Action<bool> callback)
    {
        if(GameManager.startCamPos!=null && GameManager.endCamPos!=null && !float.IsNaN(GameManager.startCamPos.x) && !float.IsNaN(GameManager.endCamPos.x) && !float.IsNaN(transform.position.x)){
        float distCovered = (Time.time - GameManager.startTime) * speed;
        float fractionOfJourney = distCovered / GameManager.journeyLength;
        transform.position = Vector3.Lerp(GameManager.startCamPos, GameManager.endCamPos, fractionOfJourney);
        yield return new WaitForSeconds(1.0f);
        callback(true);
        }
    }
}

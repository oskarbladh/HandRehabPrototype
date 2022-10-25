using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    [SerializeField]
    GameObject PlayerCanvas;
    public GameObject MushroomCanvas;
    public List<GameObject> AllMushrooms;
    public List<GameObject> MushroomsInRange;
    public bool cameraMovementNeeded=false;
    public bool mushroomGotOut=false;
    public bool objectIsWithinRadius=false;
    public float startTime;
    public Vector3 startCamPos;
    public Vector3 endCamPos;
    public float journeyLength;
    GameObject MainCamera;
    //picked mushroom data to display on the canvas
    public MushroomInfo mushRoomData=null;
    
    public Text Name;
    public Text Description;
    TextMeshProUGUI Score;

    private int score=0;

    //GameObject TextUI;

    // Start is called before the first frame update
    void Start()
    {
        instance=this;
        MainCamera=GameObject.Find("Main Camera");
        //Name = MushroomCanvas.transform.Find("Text (Legacy)").gameObject.GetComponent<Text>();
        Score = PlayerCanvas.transform.Find("Score").gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void updateScore(){
       if(mushRoomData!=null)
       {
        if(mushRoomData.poison)
            score-=2;
        else
            score+=1;
       }
    }

    // Update is called once per frame
    void Update()
    {
        if(mushRoomData!=null)
        if(Name.text != mushRoomData.mName)
        {
            Name.text = "Name:"+mushRoomData.mName;
            Description.text = "Description:"+mushRoomData.mProperties;
        }
        Score.text="Score: "+(score);

            if(!(MushroomsInRange.Count > 0)){
                settingUpLerpValues(true,true,new Vector3(0,1.0998f,-0.5680f),70);
                objectIsWithinRadius=false;
            }
            else
            {
                settingUpLerpValues(true,false,new Vector3(0,0.8889f,-0.7440f),37);
            }

        //Mushroom Spawning with all props and also Object pooling should be handled in the script
    }


    public void settingUpLerpValues(bool cameraMovementNeeded,bool mushroomGotOut,Vector3 endPos,int fieldOfView){
        this.cameraMovementNeeded = cameraMovementNeeded;
        this.mushroomGotOut = mushroomGotOut;
        startTime = Time.time;
        startCamPos=MainCamera.transform.position;
        endCamPos=endPos;
        MainCamera.GetComponent<Camera>().fieldOfView = fieldOfView;
        journeyLength = Vector3.Distance(startCamPos, endCamPos);
    }

    public void ResetLevel(){
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}

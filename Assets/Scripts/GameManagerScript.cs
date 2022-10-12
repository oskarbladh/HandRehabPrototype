using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerCanvas;
    public GameObject MushroomCanvas;
    public bool cameraMovementNeeded=false;
    public bool mushroomGotOut=false;
    public bool objectIsWithinRadius=false;
    public float startTime;
    public Vector3 startCamPos;
    public Vector3 endCamPos;
    public float journeyLength;
    public MushroomInfo mushRoomData=null;
    
    public Text Name;
    TextMeshProUGUI Score;

    private int score=0;

    //GameObject TextUI;

    // Start is called before the first frame update
    void Start()
    {
        //Name = MushroomCanvas.transform.Find("Text (Legacy)").gameObject.GetComponent<Text>();
        Score = PlayerCanvas.transform.Find("Score").gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void updateScore(){
        score+=1;
    }

    // Update is called once per frame
    void Update()
    {
        if(mushRoomData!=null)
        if(Name.text != mushRoomData.Mname)
        {
            Name.text = mushRoomData.Mname;
        }
        Score.text="Score: "+(score/2);
    }
}

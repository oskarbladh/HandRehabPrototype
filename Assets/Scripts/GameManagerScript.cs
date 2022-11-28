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
  public GameObject AimUIDisplay;
  public GameObject MushroomCanvas;
  public List<GameObject> AllMushrooms;
  public List<GameObject> MushroomsInRange;

  public List<GameObject> MushroomsInBasket;
  GameObject MainCamera;
  public Toggle isLeftToggle;
  public bool isLeft;
  public MushroomInfo mushRoomData = null;
  public Transform MushroomTranslatePoint;
  public bool cameraMovementNeeded = false;
  public bool mushroomGotOut = false;
  public bool objectIsSelected = false;

  public MushroomInfo selectedComponentMushroonInfoScript = null;
  public bool explorationMode = true;
  public float startTime;
  public Vector3 startCamPos;
  public Vector3 endCamPos;
  public float journeyLength;

  public GameObject Instructions;



  //picked mushroom data to display on the canvas


  public Text Name;
  public Text Description;
  TextMeshProUGUI Score;

  private int score = 0;

  public GameObject LeapHandRight;
  public GameObject LeapHandLeft;

  public GameObject WarningScreen;

  public GameObject RightBasket;

  public GameObject LeftBasket;

  public GameObject WinningScreen;
  public int currentIndex = 0;

  public string levelName = "5";



  //GameObject TextUI;

  // public void setObjectIsSelected(bool value){
  //     GameManagerScript.instance.objectIsSelected =value;
  // }

  void Awake()
  {
    if (instance != null && instance != this)
    {
      Destroy(this);
    }
    else
    {
      instance = this;
    }
    isLeft = isLeftToggle.isOn;
    //DontDestroyOnLoad(gameObject);
  }
  // Start is called before the first frame update
  void Start()
  {
    MainCamera = GameObject.Find("Main Camera");
    //Name = MushroomCanvas.transform.Find("Text (Legacy)").gameObject.GetComponent<Text>();
    Score = PlayerCanvas.transform.Find("Score").gameObject.GetComponent<TextMeshProUGUI>();
  }

  public void updateScore()
  {
    if (mushRoomData != null)
    {
      score += mushRoomData.pointsForTheMushroom;
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (LeapHandLeft.activeSelf || LeapHandRight.activeSelf)
    {
      Time.timeScale = 1;
      WarningScreen.SetActive(false);
    }
    else
    {
      //Time.timeScale =0;
      WarningScreen.SetActive(true);
    }
    isLeft = isLeftToggle.isOn;
    if (mushRoomData != null)
      if (Name.text != mushRoomData.mName)
      {
        Name.text = "Name:" + mushRoomData.mName;
        Description.text = "Description:" + mushRoomData.mProperties;
      }
    switch (levelName)
    {
      case "1":
        {
          Score.gameObject.SetActive(false);
          //check for 5 good mushrooms collided with the platform where the mushroom lands
          if (AllMushrooms.Count == 0)
          {
            Time.timeScale = 0;

            //Show winning screen

            //Transition to next level
            SceneManager.LoadScene("Level 2");
          }
        }
        break;
      case "2":
        {
          Score.text = "Score: " + (score);
          if (score == 500)
          {
            Debug.Log("Level completed next level");
            SceneManager.LoadScene("Level 3");
          }
          Name.text = "";
          Description.text = "";
          //Transition to next level
        }
        break;
      case "3":
        {
          Score.gameObject.SetActive(false);
          //check for 5 mushrooms in basket and Transition to next level
          if (MushroomsInBasket.Count == 8)
          {
            //Transition to next level
            SceneManager.LoadScene("Level 4");
          }
        }
        break;
      case "4":
        {
          Score.gameObject.SetActive(false);
          //check for 5 mushrooms present in the scene without leaves and Transition to next level
          if (checkLeavesAllLeavesAreRemoved())
          {
            //Transition to next level
            SceneManager.LoadScene("Level 5");
          }
        }
        break;
      case "5":
        {
          Score.gameObject.SetActive(false);
          //check for 5 mushrooms in basket and Transition to next level
          if (MushroomsInBasket.Count == 8)
          {
            //Transition to next level
            SceneManager.LoadScene("Level 6");
          }
        }
        break;
      case "6":
        {
          Score.text = "Score: " + (score);
          if (MushroomsInBasket.Count > 5 && score == 500)
          {
            //Transition to next level
            // SceneManager.LoadScene("");
            WinningScreen.SetActive(true);
            //Winning screen
          }
        }
        break;
    }


    if (!(MushroomsInRange.Count > 0))
    {
      settingUpLerpValues(true, true, new Vector3(0, 1.0998f, -0.5680f), 75);
      explorationMode = true;
      //objectIsSelected=false;
    }
    else
    {
      settingUpLerpValues(true, false, new Vector3(0, 0.8889f, -0.7440f), 45);
    }
    if (isLeft)
    {
      LeftBasket.SetActive(true);
      RightBasket.SetActive(false);
    }
    else
    {
      LeftBasket.SetActive(false);
      RightBasket.SetActive(true);
    }

    //Mushroom Spawning with all props and also Object pooling should be handled in the script
  }


  public void settingUpLerpValues(bool cameraMovementNeeded, bool mushroomGotOut, Vector3 endPos, int fieldOfView)
  {
    this.cameraMovementNeeded = cameraMovementNeeded;
    this.mushroomGotOut = mushroomGotOut;
    startTime = Time.time;
    startCamPos = MainCamera.transform.position;
    endCamPos = endPos;
    MainCamera.GetComponent<Camera>().fieldOfView = fieldOfView;
    journeyLength = Vector3.Distance(startCamPos, endCamPos);
  }

  public void ResetLevel()
  {
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
  }

  public void showInstructions()
  {
    Instructions.SetActive(!Instructions.activeSelf);
  }

  public void closeInstructions()
  {
    Instructions.SetActive(false);
  }

  bool checkLeavesAllLeavesAreRemoved()
  {
    bool levelCleared = false;
    foreach (var mushroom in AllMushrooms)
    {
      if (!mushroom.GetComponent<MushroomInfo>().isCoveredByLeaves)
      {
        levelCleared = true;
      }
      else
      {
        levelCleared = false;
        break;
      }
    }
    return levelCleared;
  }

}

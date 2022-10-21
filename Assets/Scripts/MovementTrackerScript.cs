using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.UI;
using TMPro;

public class MovementTrackerScript : MonoBehaviour
{
    public TextMeshProUGUI Pinch;
    public TextMeshProUGUI Grab;
    public TextMeshProUGUI Push;
    public TextMeshProUGUI Pull;
    public TextMeshProUGUI Rotate;
    public TextMeshProUGUI FinMovement;
    public TextMeshProUGUI Glide;
    public GameObject testCube;
    Vector3 handPosition=new Vector3(0,0,0);
    void Update()
    {
        Hand _leftHand = Hands.Left;
        Hand _rightHand = Hands.Right;
        Hand _prioritisedHand = Hands.Left ?? Hands.Right;
        Hand _specificHand = Hands.Get(Chirality.Left);
        List<Hand> _allHands = Hands.Provider.CurrentFrame.Hands;
        //Debug.Log(_leftHand);
        handPosition=_rightHand.WristPosition;
        OnUpdateHand(_rightHand);

        //OnUpdateHand(_leftHand);
    }
     void OnUpdateHand(Hand _hand)
    {
        string fingerString="";
        if(_hand==null)
        {
             Grab.color = new Color32(255,255,255,255);
              Pinch.color = new Color32(255,255,255,255);
            return;
        }
        

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////Rotate
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if(_hand.IsLeft){
        if(_hand.PalmNormal.y>0.5) 
        {
            Rotate.color = new Color32(0,255,0,255);
            Rotate.text="Left Palm rotated towards Right";
        }
        // else if(_hand.PalmNormal.y<0)
        // {
        //      Rotate.color = new Color32(255,0,0,255);
        //     Rotate.text="Left Palm rotated towards Left";
        // }
        else
        {
             Rotate.color = new Color32(255,255,255,255);
            Rotate.text="Rotate";
        }
        }else{
        if(_hand.PalmNormal.y>0.5) 
        {
            Rotate.color = new Color32(0,255,0,255);
            Rotate.text="Right Palm rotated towards Right";
        }
        // else if(_hand.PalmNormal.y<0)
        // {
        //      Rotate.color = new Color32(255,0,0,255);
        //     Rotate.text="Right palm rotated towards Left";
        // }
        else
        {
             Rotate.color = new Color32(255,255,255,255);
            Rotate.text="Rotate";
        }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////Fin
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int factor = 1;
        //Debug.Log(_hand.Direction.y);
        //Debug.Log(_hand.Rotation.z);
        //Debug.Log(_hand.WristPosition);

        float dis = Vector3.Distance(handPosition, _hand.WristPosition); // Calculating Distance
        if(dis <= factor) // checking if distance is less than required distance.
        {
            //do something
            if(_hand.Direction.x<-0.45f)
            {
                FinMovement.text="Pointing towards left";
                 FinMovement.color = new Color32(0,255,0,255);
            }
            else if(_hand.Direction.x>0.15f){
                 FinMovement.text="Pointing towards right";
                  FinMovement.color = new Color32(0,255,0,255);
            }
            else{
                 FinMovement.text="Fin Movement";
                  FinMovement.color = new Color32(255,255,255,255);
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////Push and Pull
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if(Mathf.Round(_hand.PalmVelocity.z)>0 && _hand.PalmPosition.z>0)
        {
            Push.color = new Color32(0,255,0,255);
            Pull.color = new Color32(255,255,255,255);
        }
        else if(Mathf.Round(_hand.PalmVelocity.z)<-0.01f && _hand.PalmPosition.z<0)
        {
            Pull.color = new Color32(0,255,0,255);
            Push.color = new Color32(255,255,255,255);
        }
        else
        {
            Pull.color = new Color32(255,255,255,255);
            Push.color = new Color32(255,255,255,255);
        }
        

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////Glide
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //up/down tilt sometimes can act as pointing direction
         if(_hand.Direction.y<-0.1f)
        {
            Glide.text="Go Forward-> Down";
            Glide.color = new Color32(0,255,0,255);
        }
        else if(_hand.Direction.y>0.45f){
            Glide.text="Go Back-> Up";
            Glide.color = new Color32(0,255,0,255);
        }else{
            Glide.text="Glide";
            Glide.color = new Color32(255,255,255,255);
        }
        //left/right tilt
        if(_hand.Rotation.z<-0.1f){
            Glide.text="Tilt right";
            Glide.color = new Color32(0,255,0,255);
        }
        else if(_hand.Rotation.z>0.1f)
        {
            Glide.text="Tilt left";
            Glide.color = new Color32(0,255,0,255);
        }
        else{
            Glide.text="Glide";
            Glide.color = new Color32(255,255,255,255);
        }




        //Use _hand to Explicitly get the specified fingers from it
        Finger _thumb = _hand.GetThumb();
        Finger _index = _hand.GetIndex();
        Finger _middle = _hand.GetMiddle();
        Finger _ring = _hand.GetRing();
        Finger _pinky = _hand.GetPinky();
        //Explicitly get the fingers associated with the hand
        _thumb = Hands.GetThumb(_hand);
        _index = Hands.GetIndex(_hand);
        _middle = Hands.GetMiddle(_hand);
        _ring = Hands.GetRing(_hand);
        _pinky = Hands.GetPinky(_hand);

        //Use the FingerType Enum cast to an int to select a finger from the hand
        _thumb = _hand.Finger((int)Finger.FingerType.TYPE_THUMB);
        _index = _hand.Finger((int)Finger.FingerType.TYPE_INDEX);
        _middle = _hand.Finger((int)Finger.FingerType.TYPE_MIDDLE);
        _ring = _hand.Finger((int)Finger.FingerType.TYPE_RING);
        _pinky = _hand.Finger((int)Finger.FingerType.TYPE_PINKY);

        //Use an index to define what finger you want.
        _thumb = _hand.Fingers[0];
        _index = _hand.Fingers[1];
        _middle = _hand.Fingers[2];
        _ring = _hand.Fingers[3];
        _pinky = _hand.Fingers[4];
        Arm _arm = _hand.Arm;
        float _armLength = _arm.Length;
        float _armWidth = _arm.Width;
        Vector3 _elbowPosition = _arm.ElbowPosition;
        //Debug.Log("ArmLength:"+_armLength);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////Pinch
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
         float _pinchStrength = _hand.PinchStrength;
         //Debug.Log("Pinch:"+_pinchStrength);
        float _pinchDistance = _hand.PinchDistance;
        Vector3 _pinchPosition = _hand.GetPinchPosition();
        if(!_index.IsExtended)
            fingerString = "Index";
        if(!_middle.IsExtended)
         fingerString = "Middle";
        if(!_ring.IsExtended)
            fingerString = "Ring";
        if(!_pinky.IsExtended)
        fingerString = "Pinky";
        //Debug.Log("PinchDistance:"+_pinchDistance);
        Vector3 _predictedPinchPosition = _hand.GetPredictedPinchPosition();
        bool isPinching = _hand.IsPinching();
       // Debug.Log("iSPinching:"+isPinching);
         if(isPinching)
         {
            Pinch.color = new Color32(0,255,0,255);
            Pinch.text = fingerString+" Pinch Strength:"+_pinchStrength;

         }
         else
         {
             Pinch.color = new Color32(255,255,255,255);
              Pinch.text = "Pinch";
         }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////Grab
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
          float _grabStrength = _hand.GrabStrength;
         // Debug.Log("Grab:"+_grabStrength);
          if(_grabStrength>0.8)
            Grab.color = new Color32(0,255,0,255);
            else
             Grab.color = new Color32(255,255,255,255);



               handPosition =_hand.WristPosition;
    }

   
}

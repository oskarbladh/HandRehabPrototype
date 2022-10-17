using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is used to attach it to the gameobject
public class MushroomInfo : MonoBehaviour
{

    //Mname = MushroomName

    public string mName;  //This should be SetName'd for each mushroom type and made into a prefab
    public string mProperties;
    public float mSize=1.0f; //This should be SetSize'd for each mushroom type and made into a prefab, or randomised if we have time

    public bool isEaten, isInfested, isMiscolored, isMoldy; // Hardcode these into each prefab, I can do it once we have our models - David || or randomised if we have time

    //Temp size modifier - remove it after object pooling is done
    // void Start(){
    //     transform.localScale = new Vector3(mSize,mSize,mSize);
    // }

    // public void changeSize(){
    //     transform.localScale = new Vector3(mSize,mSize,mSize);
    // }
}

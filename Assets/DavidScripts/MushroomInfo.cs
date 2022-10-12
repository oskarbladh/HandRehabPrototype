using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomInfo : MonoBehaviour
{

    //Mname = MushroomName

    public string Mname;  //This should be SetName'd for each mushroom type and made into a prefab
    public string MProperties;
    public int Msize; //This should be SetSize'd for each mushroom type and made into a prefab, or randomised if we have time

    public bool isEaten, isInfested, isMiscolored, isMoldy; // Hardcode these into each prefab, I can do it once we have our models - David || or randomised if we have time


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Data class for holding a Sound properties
[System.Serializable]
public class Sounds
{
  public string name;
  public AudioClip clip;
  [Range(0f, 1f)]
  public float volume;
  [Range(.1f, 3f)]
  public float pitch;
  public float timeToPlay;
  public bool timeredPlay = false;
  public float timer;

  [HideInInspector]
  public AudioSource source;
  public bool loop;
}

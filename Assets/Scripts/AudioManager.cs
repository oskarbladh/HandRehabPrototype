using System;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///AudioManager - Handles all the sound and playing that needs to be done
///</summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    Sounds[] soundAudioClips;
    // public Dictionary<Sounds,float> soundTImerDictionary;

    // public void initializeDictionaryAndComponents(){
    //     soundTImerDictionary = new Dictionary<Sounds, float>();
    //     soundTImerDictionary[Sounds.RacoonMove]=0f;
    //     foreach(SoundAudioClip sounds in soundAudioClips){

    //     }
    // }
    private void Awake()
    {

        //initializeDictionaryAndComponents();
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sounds sound in soundAudioClips)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }


    bool CanPlaySound(Sounds sound)
    {
        if (sound.timeredPlay)
        {
            float lastTimePlayed = sound.timer;
            float racoonMoveTimeMax = sound.timeToPlay;
            if (lastTimePlayed + racoonMoveTimeMax < Time.time)
            {
                sound.timer = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }


    // private AudioClip GetAudioClip(Sounds sound){
    //     if(CanPlaySound(sound)){
    //     foreach(SoundAudioClip soundClip in soundAudioClips){
    //         if(soundClip.sound == sound){
    //             return soundClip.audioClip;
    //         }
    //         else{


    //         }
    //     }
    //     }
    //     //not reachable
    //      return null;
    // }

    public void PlaySound(string name)
    {
        //GameObject audioObject = new GameObject("Sound");
        // AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
        // audioSource.PlayOneShot(GetAudioClip(sound));
        Sounds s = Array.Find(soundAudioClips, soundAudioClips => soundAudioClips.name == name);
        if (s == null)
        {
            Debug.LogWarning(name + "Sound not found,check the spelling");
            return;
        }
        if (CanPlaySound(s))
        {
            s.source.Play();
            Debug.Log("Played");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSoundScript : MonoBehaviour {

    //Play Global
    private static BGSoundScript instance = null;
    public AudioClip[] BGMusics;
    public static BGSoundScript Instance
    {
        get { return instance; }
    }

    void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("current level");     
        this.GetComponent<AudioSource>().clip = BGMusics[(currentLevel / 10)] ;
        this.GetComponent<AudioSource>().Play();
    }
    
    void Awake()
    {
        if (instance != null && instance != this)
        {
            //Destroy game object if multiple instance exists
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    //Play Gobal End

    // Update is called once per frame
    void Update () {
        
        if(PlayerPrefs.GetInt("set sound") == 2)
        {
            this.GetComponent<AudioSource>().volume = 0;
        }
        else
        {
         //   this.GetComponent<AudioSource>().Play();
            //check music volume on runtime
            if (PlayerPrefs.GetInt("set music") != 0)
            {
                this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("music volume");
            }
            else
            {
                this.GetComponent<AudioSource>().volume = 1;
            }
        }
        
    }
}

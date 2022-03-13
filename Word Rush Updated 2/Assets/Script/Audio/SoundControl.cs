using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    AudioSource audioSource;
   

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void OnButtonClick()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);  
        }

    }

}

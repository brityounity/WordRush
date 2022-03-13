using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//<summary>
//level screen UI controller
//</summary>
public class LevelScreenUIManagaer : MonoBehaviour
{
    AudioSource audioSource;
    
    
    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
       
    }

    public void LoadHomeScreen()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home"); //load home screen 
    }
   
    private void OnApplicationQuit()
    {
      
        PlayerPrefs.DeleteKey("previous temp sentence"); //delete temp sentence 
    }
}

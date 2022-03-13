using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//<summary>
//To load completed level gameplay
//</summary>
public class LoadTempGameplayScreen : MonoBehaviour
{
    AudioSource audioSource;
    public int level; //level number
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners(); ;
        gameObject.GetComponent<Button>().onClick.AddListener((() => { LoadTempGameplay(); }));
    }

    public void LoadTempGameplay()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1) //checking if soundeffect on or off
        {
            audioSource.PlayOneShot(audioSource.clip); //play button click sound
        }
        PlayerPrefs.SetInt("temp current level", level); //completed level stored as "temp current level" for further use
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay 2"); // Gameplay 2 scene is for those level wich are all ready completed
    }
}

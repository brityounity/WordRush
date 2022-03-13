using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//<summary>
//To load current level gameplay
//</summary>
public class LoadGameplayScreen : MonoBehaviour
{

    AudioSource audioSource;
    public int level;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners(); ;//remove previous or unsual button listeners
        gameObject.GetComponent<Button>().onClick.AddListener((() => { LoadGameplay(); })); //dynamic button click event detection
    }

    public void LoadGameplay()
    {
        if (PlayerPrefs.GetInt("set sound") == 0 || PlayerPrefs.GetInt("set sound") == 1)
        {
            audioSource.PlayOneShot(audioSource.clip); //playing button click sound once
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay"); //load scene
    }
}

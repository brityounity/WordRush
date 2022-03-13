using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WordTransform : MonoBehaviour
{
   
    
    bool textAdded = false;
    Vector3 targetPosition;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
       // float step = wordTransformSpeed * Time.deltaTime;
       // transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
       
        
           // this.gameObject.SetActive(false);
            if (textAdded == false)
            {
                this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 40f);

            
               
                this.gameObject.GetComponentInChildren<Text>().text = " ";
                textAdded = true;
                this.enabled = false;
            }
       
    }
}

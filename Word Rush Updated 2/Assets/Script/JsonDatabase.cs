using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;


//<summary>
//All kind of data transection will be controlled through this script
// data collection, storing, updating etc. 
//</summary>
public class JsonDatabase : MonoBehaviour
{
    public static JsonDatabase jsonDatabase = null; // instance
    protected internal string data;
    // Start is called before the first frame update
    void Awake()
    {
        jsonDatabase = this;

    }
   
    //creating class instance controller
    public static JsonDatabase GetJsonData()
    {
        return jsonDatabase;
    }

    //List of all sentence objects based on specific level
    public  List<Sentence> GetSentenceList(int level)
    {
       
        string json;
       if(level==PlayerPrefs.GetInt("current level")) // check if asking level matched with player current incomplete level
        {
            if (PlayerPrefs.GetString("data") != "")
            {
                json = PlayerPrefs.GetString("data"); // if previous stored data exist, get previous data
            }
            else
            {
                TextAsset file = Resources.Load("Data/sentences_" + level.ToString()) as TextAsset;
                json = file.text; //get specific file as text if no previous data available for asking level
            }
        }
        else //load specific level data if not matched with current level
        {
            TextAsset file = Resources.Load("Data/sentences_" + level.ToString()) as TextAsset;
            json = file.text;
        }
        

        var arrStr = Newtonsoft.Json.Linq.JObject.Parse(json); //convert string as json
        string Data = arrStr["level"] != null ? arrStr["level"].ToString() : string.Empty; // chekc data exist or not
        // GameObject.FindGameObjectWithTag("sentence holder").GetComponent<Text>().text = Data;
        List<Sentence> items = JsonConvert.DeserializeObject<List<Sentence>>(Data); //convert json data into list
      
        return items; //return data

    }

    //return sentence list as object
    public  SentenceCollection GetSentenceCollection(int level)
    {
        string json;

            if (PlayerPrefs.GetString("data") != "")
            {

                json = PlayerPrefs.GetString("data");
            }
            else
            {
                TextAsset file = Resources.Load("Data/sentences_" + level.ToString()) as TextAsset;
                json = file.text;
            }
        
       
        SentenceCollection objSentenceCollection = JsonConvert.DeserializeObject<SentenceCollection>(json);//convert json text as object list
        return objSentenceCollection;

    }

    //Updating json data
    public void UpdateJsonData(int level, bool answerStatus)
    {
        //<summary>
        //Set same sentence if answer is wrong
        //Go to new level and generate new sentence if answer is right
        //</summary>
       
        var vResult = GetSentenceCollection(level);
        // want to update 'complete' to TRUE for correct answer
        vResult.level.FirstOrDefault(x => x.id == PlayerPrefs.GetString("currentSentenceID")).complete = answerStatus == true ? "true" : "false";
        //To write the change in JSON file
        string newJSONtoWrite = JsonConvert.SerializeObject(vResult);
        PlayerPrefs.SetString("data", newJSONtoWrite);
        //write changes to file

    // System.IO.File.WriteAllText(Application.dataPath + "/Resources/Data/sentences_" + level.ToString() + ".json", newJSONtoWrite);

    // System.IO.File.WriteAllText(Application.persistentDataPath + "/Resources/Data/sentences_"+level.ToString()+".json", newJSONtoWrite);




    }



}

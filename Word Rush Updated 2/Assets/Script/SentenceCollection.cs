using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<summary>
//This class define the list of all sentences in a level
//</summary>
[Serializable]
public class SentenceCollection 
{
    public List<Sentence> level { get; set; } //level as all sentence collection
}

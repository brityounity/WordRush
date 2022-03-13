using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//<summary>
//Score count class
//</summary>
public class ScoreCount : MonoBehaviour
{
    int maxScore = 100; //max score for each sentence
    int sentenceScore; //total score for particular sentence
    int tempSentenceScore; //temporary score variable to count total score


    // Score() function determine level and sentence complete time
    public int Score(int level, int playTime)
    {
        sentenceScore = 0;
        if (level >= 1 && level <= 10)
        {
            sentenceScore = OneToTenScore(playTime);

        }else if(level >= 11 && level <= 20)
        {
            sentenceScore = ElevenToTwenty(playTime);
        }
        else if (level >= 21 && level <= 30)
        {
            sentenceScore = TwentyoneToThirty(playTime);
        }
        else if (level >= 31 && level <= 40)
        {
            sentenceScore = ThirtyoneToForty(playTime);
        }
        else if (level >= 41 && level <= 50)
        {
            sentenceScore = FortyoneToFifty(playTime);
        }

        return sentenceScore;
    }

    //count score for level 1-10
    int OneToTenScore(int tempTime)
    {
        int timeExceed; //how much extra time user take to complete the sentence 
        tempSentenceScore = 0;//initial sentence score
        if (tempTime <= 6) //for level 1-10 sentence complete normal time is 6 second
        {
            tempSentenceScore = maxScore;
        }
        else
        {
            timeExceed = tempTime - 6; //calculating extra time
            tempSentenceScore = maxScore - (timeExceed * 2); //decresing score if fixed time over
        }

        return tempSentenceScore; //return score 
    }

    //count score for level 11-20
    int ElevenToTwenty(int tempTime)
    {
        int timeExceed;
        tempSentenceScore = 0;
        if (tempTime <= 8) //for level 11-20 sentence complete normal time is 8 second
        {
            tempSentenceScore = maxScore;
        }
        else
        {
            timeExceed = tempTime - 8;
            tempSentenceScore = maxScore - (timeExceed * 3);
        }

        return tempSentenceScore;
    }

    //count score for level 21-30
    int TwentyoneToThirty(int tempTime)
    {
        int timeExceed;
        tempSentenceScore = 0;
        if (tempTime <= 9) //for level 21-30 sentence complete normal time is 9 second
        {
            tempSentenceScore = maxScore;
        }
        else
        {
            timeExceed = tempTime - 9;
            tempSentenceScore = maxScore - (timeExceed * 4);
        }
        return tempSentenceScore;
    }

    //count score for level 31-40
    int ThirtyoneToForty(int tempTime)
    {
        int timeExceed;
        tempSentenceScore = 0;
        if (tempTime <= 10) //for level 31-40 sentence complete normal time is 10 second
        {
            tempSentenceScore = maxScore;
        }
        else
        {
            timeExceed = tempTime - 10;
            tempSentenceScore = maxScore - (timeExceed * 5);
        }
        return tempSentenceScore;
    }

    //count score for level 41-50
    int FortyoneToFifty(int tempTime)
    {
        int timeExceed;
        tempSentenceScore = 0;
        if (tempTime <= 12) //for level 41-50 sentence complete normal time is 12 second
        {
            tempSentenceScore = maxScore;
        }
        else
        {
            timeExceed = tempTime - 12;
            tempSentenceScore = maxScore - (timeExceed * 6);
        }
        return tempSentenceScore;
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class HighScoreData : IComparable<HighScoreData>
{
    public float time;
    public long date;

    public float getScore()
    {
        return time;
    }

    public int CompareTo(HighScoreData value)
    {
        if (value.Equals(null))
        {
            return 1;
        }

        return Mathf.RoundToInt(time - value.time);
    }
}

[Serializable]
public class HighScoreGroup
{
    public string id;
    public List<HighScoreData> scores;

    public HighScoreGroup(string n = "")
    {
        id = n;
    }
}

public class HighscoreManagerScript : MonoBehaviour
{
    public bool clearList = false;

    public enum OrderEnum
    {
        Ascending,
        Decending
    }
    public OrderEnum sortOrder = OrderEnum.Decending;

    public int totalPositions = 5;
    public string highscoreKey = "";

    public string[] HighScoreGroupName;
    private List<HighScoreGroup> highScoreGroups;

    [Serializable]
    public struct HighscoreDataHelper
    {
        public List<HighScoreGroup> groups;
    }

    void Awake()
    {
        if (clearList)
        {
            PlayerPrefs.DeleteAll();
        }

        string jsonString = PlayerPrefs.GetString(highscoreKey);

        if(jsonString == "")
        {
            if(HighScoreGroupName.Length > 0)
            {
                highScoreGroups = new List<HighScoreGroup>();

                for(int i =0; i < HighScoreGroupName.Length; ++i)
                {
                    CreateHighscoreGroup(HighScoreGroupName[i]);
                }
            }

        }
        else
        {
            HighscoreDataHelper jsonData;
            jsonData = JsonUtility.FromJson<HighscoreDataHelper>(jsonString);
            highScoreGroups = jsonData.groups;
        }
    }

    private void CreateHighscoreGroup(string id)
    {
        HighScoreGroup group = new HighScoreGroup(id);
        highScoreGroups.Add(group);

        List<HighScoreData> scores = group.scores = new List<HighScoreData>();

        for(int i = 0; i < totalPositions; ++i)
        {
            scores.Add(new HighScoreData());
        }
    }

    public List<HighScoreData> GetHighScores(string groupID)
    {
        foreach(HighScoreGroup value in highScoreGroups)
        {
            if(value.id == groupID)
            {
                return value.scores;
            }
        }

        return null;
    }

    public bool IsHighScore(float value, string id)
    {
        List<HighScoreData> scores = GetHighScores(id);

        if(scores == null || scores.Count == 0)
        {
            return false;
        }

        if(scores[totalPositions - 1].time == 0.0f)
        {
            return true;
        }

        if(sortOrder == OrderEnum.Decending)
        {
            return (value >= scores[totalPositions - 1].time);
        }

        return (value <= scores[totalPositions - 1].time);
    }

    public HighScoreData AddNewHighScore(float value, string id)
    {
        List<HighScoreData> scores = GetHighScores(id);

        HighScoreData data = new HighScoreData();

        data.time = value;
        data.date = DateTime.Now.ToFileTime();

        for(int i = 0; i < totalPositions; ++i)
        {
            if(sortOrder == OrderEnum.Ascending)
            {
                if(scores[i].time == 0.0f || value <= scores[i].time)
                {
                    scores.Insert(i, data);
                    scores.RemoveAt(totalPositions);
                    SaveHighscores(id);
                    return data;
                }
            }
            else
            {
                if(scores[i].time == 0.0f || value >= scores[i].time)
                {
                    scores.Insert(i, data);
                    scores.RemoveAt(totalPositions);
                    SaveHighscores(id);
                    return data;
                }
            }
        }

        return null;
    }

    public void SaveHighscores(string group)
    {
        HighscoreDataHelper jsonData;
        jsonData.groups = highScoreGroups;

        string json = JsonUtility.ToJson(jsonData);
        PlayerPrefs.SetString(highscoreKey, json);
    }
}

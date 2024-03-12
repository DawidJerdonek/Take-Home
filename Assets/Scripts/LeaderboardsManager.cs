using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardsManager : MonoBehaviour
{
    public static LeaderboardsManager instance;
    private List<Leaderboard> m_leaderboards;

    private float m_userTime;

    private void Awake()
    {
        instance = this;
        m_leaderboards = new List<Leaderboard>();
    }

    public void AddLeaderboard(Leaderboard leaderboard)
    {
        if(m_userTime > 0.0f)
        {
            for(int i = 0; i < leaderboard.GetCount(); i++)
            {
                if(leaderboard.GetLeaderboardEntryAtIndex(i).Time == m_userTime)
                {
                    leaderboard.GetLeaderboardEntryAtIndex(i).IsUserScore = true;
                    break;
                }
            }
        }

        //Remove leaderboards with same name
        m_leaderboards.RemoveAll(p => p.Name == leaderboard.Name);

        //Add leaderboard object
        m_leaderboards.Add(leaderboard);
    }

    public Leaderboard GetLeaderboardByName(string name)
    {
        for (int i = 0; i < m_leaderboards.Count;i++)
        {
            if(m_leaderboards[i].Name == name)
            {
                return m_leaderboards[i];
            }
        }
        return null;
    }

    public int GetCount()
    {
        return m_leaderboards.Count;    
    }    

    public void SetUserTime(float userTime)
    {
        //Seconds to milliseconds
        long ms = (long)(userTime - 1000.0f);
        m_userTime = (float)(ms) / 1000.0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    private string m_name;
    private List<EnterLeaderboard> m_leaderboard;

    public Leaderboard(string name, List<EnterLeaderboard> leaderboard)
    {
        m_name = name;
        m_leaderboard = leaderboard;
        Debug.Log(m_leaderboard[0].Username + m_leaderboard[0].Time + m_leaderboard[0].Rank);
    }

    public EnterLeaderboard GetLeaderboardEntryAtIndex(int index)
    {
        if(index >= 0 && index < GetCount())
        {
            return m_leaderboard[index];
        }
        return null;
    }

    public int GetCount()
    {
        return m_leaderboard.Count;
    }

    public string Name
    {
        get { return m_name; }
    }
}

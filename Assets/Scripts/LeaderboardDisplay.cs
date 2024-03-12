using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardDisplay : MonoBehaviour
{
    [SerializeField] private LeaderboardRanking[] leaderboardRankings;
    public static LeaderboardDisplay instance;

    public void SetLeaderboardData(string leaderboardId)
    {
        EnterLeaderboard leaderboardEntry;
        ResetLeaderboardData();
        Leaderboard leaderboard = LeaderboardsManager.instance.GetLeaderboardByName(leaderboardId);
        
        if (leaderboard != null)
        {
            for (int i = 0; i < leaderboard.GetCount(); i++)
            {
                leaderboardEntry = leaderboard.GetLeaderboardEntryAtIndex(i);
                if(leaderboardEntry != null && i < leaderboardRankings.Length)
                {
                    leaderboardRankings[i].Set(leaderboardEntry);
                }
            }
        }
       
    }

    private void ResetLeaderboardData()
    {
        foreach (LeaderboardRanking hsr in leaderboardRankings)
            hsr.Reset();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLeaderboard
{
    private string m_nickname;
    private float m_time;
    private int m_rank;
    private bool m_isUserScore;

    public EnterLeaderboard(string nickname, float time, int rank)
    {
        m_nickname = nickname;
        m_time = time;
        m_rank = rank;
    }

    public string Username
    {
        get { return m_nickname; }
        set { m_nickname = value; }
    }

    public float Time
    {
        get { return m_time; }
        set { m_time = value; }
    }

    public int Rank
    {
        get { return m_rank; }
        set { m_rank = value; }
    }

    public bool IsUserScore
    { 
        get { return m_isUserScore; }
        set { m_isUserScore = value; }
    }
}

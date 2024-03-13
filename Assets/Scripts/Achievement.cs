using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement
{
    private string m_id;
    private string m_title;
    private string m_description;
    private bool m_awarded;

    public Achievement(string id, string title, string description, string status)
    {
        m_id = id;
        m_title = title;
        m_description = description;
        m_awarded = status == "AWARDED";
    }

    public string GetStatusString()
    {
        if(m_awarded)
        {
            return "Earned";
        }
        return "";
    }

    public void AwardAchievement()
    {
        m_awarded = true;
        NetworkManager.sharedInstance.AwardAchievementReq(this);
    }

    public string Id
    {
        get { return m_id; }
    }
      
    public string Title 
    { 
        get { return m_title; } 
    }

    public string Description 
    { 
        get { return m_description; } 
    }

    public bool Awarded 
    { 
        get { return m_awarded; } 
    }
}

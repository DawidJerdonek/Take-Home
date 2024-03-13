using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    private List<Achievement> m_achievements;

    private void Awake()
    {
        instance = this;
    }

    public Achievement GetAchievementById(string id)
    {
        if(m_achievements != null)
        {
            for(int i = 0; i < m_achievements.Count; i++)
            {
                if(m_achievements[i].Id == id)
                {
                    return m_achievements[i];
                }
            }
        }
        return null;
    }

    public Achievement GetAchievementByIndex(int index)
    {
        if (m_achievements != null)
        {
            if (index >= 0 && index < GetCount())
            {
                return m_achievements[index];
            }
        }
        return null;
    }

    public int GetCount()
    {
        return m_achievements.Count;
    }

    public void SetAchievements(ref List<Achievement> achievements)
    {
        m_achievements = achievements;
    }
}


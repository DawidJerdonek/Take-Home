using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager instance;

    private List<Statistics> m_statistics;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Statistics GetStatisticByName(string name)
    {
        if (m_statistics != null)
        {
            for(int i = 0; i < m_statistics.Count; i++)
            {
                if (m_statistics[i].Name == name)
                {
                    return m_statistics[i];
                }
            }
        }
        return null;
    }

    public Statistics GetStatisticByIndex(int index)
    {
        if(m_statistics != null)
        {
            if(index>= 0 && index< GetCount())
            {
                return m_statistics[index];
            }
        }
        return null;
    }

    public int GetCount()
    {
        return m_statistics.Count;
    }
    public void SetStatistics(ref List<Statistics> statistics)
    {
        m_statistics = statistics;
    }

    public Dictionary<string,object> GetIncrementsDictionary()
    {
        if(m_statistics != null)
        {
            Dictionary<string,object> data = new Dictionary<string,object>();

            for(int i = 0; i < m_statistics.Count; i++)
            {
                data.Add(m_statistics[i].Name, m_statistics[i].Increment);
            }
            return data;
        }
        return null;
    }
}

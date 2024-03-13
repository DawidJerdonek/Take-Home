using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics
{
    private string m_name;
    private string m_description;
    private long m_value;
    private long m_increment;

    public Statistics(string name, string description, long value)
    {
        m_name = name;
        m_description = description;
        m_value = value;
        m_increment = 0;
    }

    public void IncrementValue(int amount = 1)
    {
        m_increment += amount;
        m_value += amount;
    }
    public void IncrementByValue(int amount)
    {
        m_increment += amount;
        m_value += amount;
    }
    

    public string Name
    { 
        get { return m_name; } 
    }
    public string Description 
    { 
        get { return m_description; } 
    }
    public long Value
    { 
        get { return m_value; } 
    }
    public long Increment
    { 
        get { return m_increment; } 
    }
}

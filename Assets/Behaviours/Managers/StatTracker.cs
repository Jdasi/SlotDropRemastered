﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutStats
{
    public int score_earned;
    public int score_deposited;
    public int kills;
    public int deaths;
}

public class UpgradeTimestamp
{
    public string name;
    public float time;
}

public class StatTracker : MonoBehaviour
{
    public bool debug_log_enabled = true;

    public Dictionary<string, LoadoutStats> loadout_stats = new Dictionary<string, LoadoutStats>();
    public List<UpgradeTimestamp> upgrade_timestamps = new List<UpgradeTimestamp>();
    public int num_titans { get; private set; }

    private float awake_time;


    public void Init(List<string> _loadout_names)
    {
        foreach (string loadout_name in _loadout_names)
        {
            loadout_stats.Add(loadout_name, new LoadoutStats());
        }
    }


    public void LogDeath(string _loadout_name)
    {
        if (!loadout_stats.ContainsKey(_loadout_name))
            return;

        LoadoutStats stats = loadout_stats[_loadout_name];
        ++stats.deaths;

        if (debug_log_enabled)
        {
            Debug.Log("Loadout: " + _loadout_name + " died. " +
                "[Total Deaths for Loadout: " + stats.deaths.ToString() + "]");
        }
    }


    public void LogKill(string _loadout_name)
    {
        if (!loadout_stats.ContainsKey(_loadout_name))
            return;

        LoadoutStats stats = loadout_stats[_loadout_name];
        ++stats.kills;

        if (debug_log_enabled)
        {
            Debug.Log("Loadout: " + _loadout_name + " earned a kill. " +
                "[Total Kills for Loadout: " + stats.kills.ToString() + "]");
        }
    }


    public void LogScoreIncrease(string _loadout_name, int _amount)
    {
        if (!loadout_stats.ContainsKey(_loadout_name))
            return;

        LoadoutStats stats = loadout_stats[_loadout_name];
        stats.score_earned += _amount;

        if (debug_log_enabled)
        {
            Debug.Log("Loadout: " + _loadout_name + " earned " + _amount.ToString() + " score. " +
                "[Total Earned Score for Loadout: " + stats.score_earned.ToString() + "]");
        }
    }


    public void LogScoreDeposited(string _loadout_name, int _amount)
    {
        if (!loadout_stats.ContainsKey(_loadout_name))
            return;

        LoadoutStats stats = loadout_stats[_loadout_name];
        stats.score_deposited += _amount;

        if (debug_log_enabled)
        {
            Debug.Log("Loadout: " + _loadout_name + " deposited " + _amount.ToString() + " score. " +
                "[Total Deposited Score for Loadout: " + stats.score_deposited.ToString() + "]");
        }
    }


    public void LogPCUpgrade(string _upgrade_name, float _time)
    {
        UpgradeTimestamp timestamp = new UpgradeTimestamp();

        timestamp.name = _upgrade_name;
        timestamp.time = _time - awake_time;

        upgrade_timestamps.Add(timestamp);

        if (debug_log_enabled)
        {
            Debug.Log("PC Upgraded to: " + _upgrade_name + " at " + _time.ToString());
        }
    }


    public void LogTitanAchieved()
    {
        ++num_titans;

        if (debug_log_enabled)
        {
            Debug.Log("Titan Achieved. " + "[Total Titans this Session: " + num_titans.ToString() + "]");
        }
    }


    void Awake()
    {
        awake_time = Time.time;
    }

}
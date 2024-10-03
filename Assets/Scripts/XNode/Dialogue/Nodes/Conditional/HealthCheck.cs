﻿using UnityEngine;

/// <summary>
/// A Health check conditional that checks the players health.
/// If it is lower than or equal to our value then it is true, otherwise it is true.
/// Written by: Sean
/// </summary>
public class HealthCheck : ConditionalNodeBase
{
    public int healthToCheck;
    public override bool Condition()
    {
        int health = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityHealth>().GetEntityHealth;

        if (health <= healthToCheck)
            return true;
        else
            return false;
    }

    public override string GetNodeType { get { return "HealthCheck"; } }
}
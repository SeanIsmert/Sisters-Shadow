using UnityEngine;

/// <summary>
/// A Health check conditional that checks the players health.
/// If it is lower than or equal to our value then it is true, otherwise it is true.
/// Written by: Sean
/// </summary>
public class AmmoCheck : ConditionalNodeBase
{
    public int ammoToCheck;
    public override bool Condition()
    {
        int ammo = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityHealth>().GetEntityHealth;

        return ammo <= ammoToCheck;
    }

    public override string PortOnCondtion()
    {
        return Condition() ? "isTrue" : "isFalse";
    }

    public override string GetNodeType { get { return "AmmoCheck"; } }
}
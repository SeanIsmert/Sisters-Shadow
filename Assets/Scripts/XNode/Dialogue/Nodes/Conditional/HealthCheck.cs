using UnityEngine;

public class HealthCheck : ConditionalNodeBase
{
    public override bool Condition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return false;

        //if (PlayerData.instance.health > 2)
        //    return true;

        return false;
    }

    public override string GetNodeType { get { return "HealthCheck"; } }
}
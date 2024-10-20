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
        uint ammoTotal = 0;
        foreach (var token in InventoryManager.Instance.GetInventoryItems()) // Iterate through each item in the player's inventory
        {
            if (!token.GetBaseItem.stackable) // skip items that arn't stackable items
                continue;

            ammoTotal += token.GetItemAmount;
        }

        if (ammoTotal > ammoToCheck) // Return ture if you surpass amount
            return true;

        return false; // Return false otherwise
    }

    public override string PortOnCondtion()
    {
        return Condition() ? "isTrue" : "isFalse";
    }

    public override string GetNodeType { get { return "AmmoCheck"; } }
}
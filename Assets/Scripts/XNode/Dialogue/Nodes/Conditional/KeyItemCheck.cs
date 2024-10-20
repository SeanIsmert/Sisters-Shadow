public class KeyItemCheck : ConditionalNodeBase
{
    public string keyToCheck;
    public override bool Condition()
    {
        foreach (var token in InventoryManager.Instance.GetInventoryItems()) // Iterate through each item in the player's inventory
        {
            if (!token.GetBaseItem.keyItem) // skip items that arn't key items
                continue;

            if (token.GetKeyID == keyToCheck) // Match ID and key
            {
                return true; // Return true if a match is found
            }
        }

        return false; // Return false otherwise
    }

    public override string PortOnCondtion()
    {
        return Condition() ? "isTrue" : "isFalse";
    }

    public override string GetNodeType { get { return "KeyItemCheck"; } }
}

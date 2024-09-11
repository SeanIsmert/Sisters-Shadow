using UnityEngine;
using XNodeEditor;
using System;
using XNode;

[CreateAssetMenu]
public class DialogueGraph : NodeGraph 
{
	public DialogueNodeBase current;
}

[CustomNodeGraphEditor(typeof(DialogueGraph))]
public class DialogueGraphEditor : NodeGraphEditor
{
    public override string GetNodeMenuName(Type type)
    {
        if (type.IsSubclassOf(typeof(CoreNodeBase)) || type.IsSubclassOf(typeof(DialogueNodeBase)) || type.IsSubclassOf(typeof(ConditionalNodeBase)))
        {
            return base.GetNodeMenuName(type);        
        }

        return null;
    }
}
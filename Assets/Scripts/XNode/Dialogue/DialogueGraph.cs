using UnityEngine;
using System;
using XNode;

#if UNITY_EDITOR
using XNodeEditor;
#endif

[CreateAssetMenu]
public class DialogueGraph : NodeGraph 
{
	//public DialogueNodeBase dialogue;
    public CoreNodeBase current;
}

#if UNITY_EDITOR
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
#endif
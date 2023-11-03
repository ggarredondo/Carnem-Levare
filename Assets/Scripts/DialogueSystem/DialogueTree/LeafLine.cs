using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TreeUtilities;

namespace DialogueTreeUtilities
{
    [NodeRelevance(typeof(DialogueTree))]
    public class LeafLine : LeafNode, IHaveText
    {
        [HideInInspector] public string text;

        public DialogueData data;

        public DialogueData Data => data;

        public string Text => text;

        public override void Clone(Node node)
        {
            base.Clone(node);
            data.Clone(((LeafLine)node).Data);
        }
    }
}

using UnityEngine;

namespace TreeUtilities
{
    public abstract class Node : ScriptableObject
    {
        [HideInInspector] public string currentName;
        [HideInInspector] public Color backgroundColor;
        [HideInInspector] public Color textColor;
        [HideInInspector] public int ID;
        [HideInInspector] public bool selected;
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;

        public ref readonly int GetID { get => ref ID; }

        public int InitializeID(int actualID)
        {
            ID = actualID + 1;
            int newID = ID;

            if (this is IHaveChildren n)
                newID = n.InitializeChildrenID();

            return newID;
        }

        public void Initialize()
        {
            selected = false;

            if (this is IHaveChildren n)
                n.InitializeChildren();
        }

        public virtual void Clone(Node node)
        {
            currentName = node.currentName;
            backgroundColor = node.backgroundColor;
            textColor = node.textColor;
            position = node.position;
        }
    }
}

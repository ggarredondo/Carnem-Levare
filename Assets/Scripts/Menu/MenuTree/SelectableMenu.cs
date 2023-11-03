using TreeUtilities;
using UnityEngine;

namespace MenuTreeUtilities
{
    [NodeRelevance(typeof(MenuTree))]
    public class SelectableMenu : CompositeNode, ICanSelect
    {
        [SerializeField] private int selectedChild;

        public int GetSelectedChild()
        {
            return selectedChild;
        }

        public void MoveLeftChild()
        {
            selectedChild = Mod(selectedChild - 1, children.Count);
        }

        public void MoveRightChild()
        {
            selectedChild = Mod(selectedChild + 1, children.Count);
        }

        public void SelectChild(int child)
        {
            selectedChild = child;
        }

        private int Mod(int a, int b)
        {
            return (a % b + b) % b;
        }
    }
}

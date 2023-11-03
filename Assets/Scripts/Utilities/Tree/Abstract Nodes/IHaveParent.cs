namespace TreeUtilities
{
    public interface IHaveParent
    {
        public void SetParent(Node parent);

        public Node GetParent();
    }
}

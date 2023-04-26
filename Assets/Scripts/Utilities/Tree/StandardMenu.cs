public class StandardMenu : CompositeNode, IChildrenIterate
{
    public void GoToChildren()
    {
        ChangeState();
        children[actualChildren].ChangeState();
    }

    protected override void OnNotSelected()
    {
        selected = false;
    }

    protected override void OnSelected()
    {
        selected = true;
    }
}

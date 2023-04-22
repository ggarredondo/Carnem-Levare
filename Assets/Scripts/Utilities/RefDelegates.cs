
namespace RefDelegates
{
    public delegate void ActionRef<T>(ref T obj);
    public delegate void ActionRef<T1, T2>(ref T1 obj1, ref T2 obj2);

    public delegate void ActionIn<T>(in T obj);
    public delegate void ActionIn<T1, T2>(in T1 obj1, in T2 obj2);
}

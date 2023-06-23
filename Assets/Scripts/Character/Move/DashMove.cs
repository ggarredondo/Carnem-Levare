using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Move/DashMove")]
public class DashMove : Move
{
    [Header("Dash Values")]
    [SerializeField] private bool invulnerability = false;

    protected override void UpdateStringData()
    {
        stringData?.Clear();
        stringData?.Add(""); stringData?.Add(moveName);
        stringData?.Add("Start Up"); stringData?.Add((int)RelativeStartUp + " ms");
        stringData?.Add("Active"); stringData?.Add((int)RelativeActive + " ms");
        stringData?.Add("Recovery"); stringData?.Add((int)RelativeRecovery + " ms");
    }

    public override void InitMove(in CharacterStats stats) {}
    public override void ActivateMove(in CharacterStats stats) => stats.NOHURT_FLAG = invulnerability;
    public override void DeactivateMove(in CharacterStats stats) => stats.NOHURT_FLAG = false;
    public override void EndMove(in CharacterStats stats) {}
}

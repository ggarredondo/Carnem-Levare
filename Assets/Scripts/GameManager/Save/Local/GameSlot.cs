
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSlot : SaveSlot
{
    [Header("Player")]
    public List<int> selectedMoves;
    public List<bool> newMoves;
    public List<Move> moves;

    [Header("Enemy")]
    public string enemyResourcesFolder; 
    public List<string> enemyPrefabNames;

    public new object Clone()
    {
        GameSlot nuevo = (GameSlot) MemberwiseClone();

        nuevo.selectedMoves = new List<int>(selectedMoves);
        nuevo.newMoves = new List<bool>(newMoves);
        nuevo.moves = new List<Move>(moves);

        nuevo.enemyPrefabNames = new List<string>(enemyPrefabNames);

        return nuevo;
    }
}

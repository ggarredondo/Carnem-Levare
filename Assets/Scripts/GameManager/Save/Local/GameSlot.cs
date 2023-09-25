
using System.Collections.Generic;

[System.Serializable]
public class GameSlot : SaveSlot
{
    public List<int> selectedMoves;
    public List<bool> newMoves;
    public List<Move> moves;

    public new object Clone()
    {
        GameSlot nuevo = (GameSlot) MemberwiseClone();

        nuevo.selectedMoves = new List<int>(selectedMoves);
        nuevo.newMoves = new List<bool>(newMoves);
        nuevo.moves = new List<Move>(moves);

        return nuevo;
    }
}

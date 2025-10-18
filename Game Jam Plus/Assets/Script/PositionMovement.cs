using UnityEngine;

[System.Serializable]
public class PositionMovement
{
    [SerializeField] private TypePlayerEnum m_typePlayer;
    [SerializeField] private int m_pieceForce;
    public TypePlayerEnum  TypePlayer => m_typePlayer;
    public int PieceForce => m_pieceForce;

    public void CreateNewMovement(TypePlayerEnum type, int force)
    {
        m_typePlayer = type;
        m_pieceForce = force;
    }
}

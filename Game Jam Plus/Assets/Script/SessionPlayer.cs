using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionPlayer
{
    [SerializeField] private TypePlayerEnum m_typePlayer;
    [SerializeField] private List<PlayerPieceElement> m_pieces = new List<PlayerPieceElement>();
    [SerializeField] private int m_maxForce;

    public TypePlayerEnum TypePlayer => m_typePlayer;
    public int GetMaxForceSessionPlayer()
    {
        var forceMax = 0;
        foreach(var piece in m_pieces)
        {
            if(piece.PieceForce > forceMax && piece.gameObject.activeSelf)
            {
                m_maxForce = piece.PieceForce;
            }
        }
        return m_maxForce;
    }
}

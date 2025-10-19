using System.Collections.Generic;
using UnityEngine;

public class MapPosition : MonoBehaviour
{
    [SerializeField] private List<PositionMovement> m_movements = new List<PositionMovement>();
    [SerializeField] private SpriteRenderer m_childrenRenderer;
    private void Awake()
    {
        PositionMovement newPosition = new PositionMovement();
        newPosition.CreateNewMovement(TypePlayerEnum.None, -1);
        m_movements.Add(newPosition);
    }

    public PositionMovement GetLastMovement()
    {
        if (m_movements.Count == 0)
            return null;
        return m_movements[m_movements.Count - 1];
    }

    public bool TryInsertMovement(PlayerPieceElement newPieceElement)
    {
        if(newPieceElement.PieceForce > GetLastMovement().PieceForce)
        {
            PositionMovement newPosition = new PositionMovement();
            newPosition.CreateNewMovement(newPieceElement.TypePlayer, newPieceElement.PieceForce);
            m_childrenRenderer.sprite = newPieceElement.ElementSprite;
            m_childrenRenderer.color = newPieceElement.ElementColor;
            m_childrenRenderer.gameObject.transform.localScale = newPieceElement.gameObject.transform.lossyScale;
            newPieceElement.DesactivePiece();
            m_movements.Add(newPosition);
            return true;
        }
        return false;
    }
}

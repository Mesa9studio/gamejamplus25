using UnityEngine;


public class PlayerPieceElement : MonoBehaviour
{
    [SerializeField] private TypePlayerEnum m_typePlayer;
    [SerializeField] private int m_pieceForce;
    [SerializeField] private Sprite m_elementSprite;
    [SerializeField] private Color m_elementColor;
    public TypePlayerEnum TypePlayer => m_typePlayer;
    public int PieceForce => m_pieceForce;
    public Sprite ElementSprite => m_elementSprite;
    public Color ElementColor => m_elementColor;

    public PlayerPieceElement Select()
    {
        Debug.Log("Select Position name " + gameObject.name + ": " + m_typePlayer + ", " + m_pieceForce);
        return this;
    }
    public PlayerPieceElement Deselect()
    {
        Debug.Log("Deselect Position name " + gameObject.name + ": " + m_typePlayer + ", " + m_pieceForce);
        return this;
    }

    public void DesactivePiece()
    {
        this.gameObject.SetActive(false);

    }
}

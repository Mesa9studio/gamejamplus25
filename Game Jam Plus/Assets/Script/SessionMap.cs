using System.Collections.Generic;
using UnityEngine;

public class SessionMap : MonoBehaviour
{
    [SerializeField] private List<MapPosition> m_sessionMap = new List<MapPosition>();
    [SerializeField] private List<PlayerPieceElement> m_playerLeft;
    [SerializeField] private List<PlayerPieceElement> m_playerRight;
    [SerializeField] private PlayerPieceElement m_currentObjectClick;
    [SerializeField] private List<TypePlayerEnum> m_playersGame;
    [SerializeField] private TypePlayerEnum m_currentPlayer;
    private void Start()
    {
        ChooseFirstPlayer();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
            // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
            if (hitInfo && hitInfo.collider.CompareTag("PlayerElement"))
            {
                SelectElement(hitInfo);
            }

            if (hitInfo && hitInfo.collider.CompareTag("MapElement"))
            {
                SelectMapElement(hitInfo);
            }
        }
    }

    private void ChooseFirstPlayer()
    {
        var index = Random.Range(0, m_playersGame.Count);
        m_currentPlayer = m_playersGame[index];
    }
    private void SelectMapElement(RaycastHit2D hitInfo)
    {
        if (m_currentObjectClick == null)
            return;

        var elementClicked = hitInfo.collider.GetComponent<MapPosition>();
        var isValidMovement = elementClicked.TryInsertMovement(m_currentObjectClick);
        Debug.Log("Movimento validado? " + isValidMovement);
        if (isValidMovement)
        {
            m_currentObjectClick = null;
            ShowCurrentBoardGame();
            ChangeCurrentPlayer();
        }
    }

    private void ChangeCurrentPlayer()
    {
        var index = m_playersGame.IndexOf(m_currentPlayer);

        m_currentPlayer = index == 0 ? m_playersGame[1] : m_playersGame[0];
    }
    private void SelectElement(RaycastHit2D hitInfo)
    {
        var elementClicked = hitInfo.collider.GetComponent<PlayerPieceElement>();
        if (m_currentObjectClick == null)
        {
            if (elementClicked.TypePlayer == m_currentPlayer)
            {
                m_currentObjectClick = elementClicked?.Select();
            }
        }
        else
        {
            if (elementClicked == m_currentObjectClick)
            {
                m_currentObjectClick.Deselect();
                m_currentObjectClick = null;
            }
            else
            {
                if(elementClicked.TypePlayer == m_currentPlayer)
                {
                    m_currentObjectClick.Deselect();
                    m_currentObjectClick = elementClicked;
                    m_currentObjectClick.Select();
                }
            }
        }
    }
    private void ShowCurrentBoardGame()
    {
        Debug.Log("Show Current Board Game");
        foreach (var pos in m_sessionMap)
        {
            Debug.Log("Position name " + pos.name + ": " + pos.GetLastMovement().TypePlayer + ", " + pos.GetLastMovement().PieceForce);
        }
    }
}

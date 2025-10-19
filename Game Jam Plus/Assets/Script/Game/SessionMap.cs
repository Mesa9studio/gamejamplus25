using System.Collections.Generic;
using UnityEngine;

public class SessionMap : MonoBehaviour
{
    [SerializeField] private List<MapPosition> m_sessionMap = new List<MapPosition>();
    [SerializeField] private List<SessionPlayer> m_players = new List<SessionPlayer>();
    [SerializeField] private PlayerPieceElement m_currentObjectClick;
    [SerializeField] private List<TypePlayerEnum> m_playersGame;
    [SerializeField] private TypePlayerEnum m_currentPlayer;
    [SerializeField] private TypePlayerEnum m_winnerPlayer;
    [SerializeField] private int m_minimumMapForce;
    [SerializeField] private GameObject m_indicatorCurrentObjectClick;
    [SerializeField] private float m_indicatorOffset;
    [SerializeField] private GameCanvasUI m_gameCanvasUI;
    [SerializeField] private GameObject m_groupPlayerOne;
    [SerializeField] private GameObject m_groupPlayerTwo;


    private bool fimDeJogo;
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
            if (hitInfo && hitInfo.collider.CompareTag("PlayerElement") && !fimDeJogo)
            {
                SelectElement(hitInfo);
            }

            if (hitInfo && hitInfo.collider.CompareTag("MapElement") && !fimDeJogo)
            {
                SelectMapElement(hitInfo);
            }
        }
    }
    #region Gameplay
    private void ChooseFirstPlayer()
    {
        var index = Random.Range(0, m_playersGame.Count);
        m_currentPlayer = m_playersGame[index];
        m_gameCanvasUI.ShowCurrentPlayerTurn(m_currentPlayer);
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
            SetIndicatorPosition(false);
            var isGameVictory = CheckVictory();
            var isGameDraw = false;
            if (!isGameVictory)
            {
                var isGameFull = CheckDraw();
                if (isGameFull)
                {
                    SessionPlayer nextPlayer = null;
                    foreach (var player in m_players)
                    {
                        if (player.TypePlayer != m_currentPlayer)
                        {
                            nextPlayer = player;
                        }
                    }
                    isGameDraw = (isGameFull && nextPlayer?.GetMaxForceSessionPlayer() > m_minimumMapForce) ? false : true;
                    if (isGameDraw)
                    {
                        fimDeJogo = true;
                    }
                }
            }
            else
            {
                fimDeJogo = true;
                m_winnerPlayer = m_currentPlayer;
            }
            Debug.Log("Game has a victory?" + isGameVictory);
            Debug.Log("Game has a draw?" + isGameDraw);
            m_currentObjectClick = null;
            if (!fimDeJogo)
            {
                ChangeCurrentPlayer();
            }
            else
            {
                m_groupPlayerOne.SetActive(false);
                m_groupPlayerTwo.SetActive(false);
                m_gameCanvasUI.ShowResult(m_winnerPlayer);
                if (m_winnerPlayer != TypePlayerEnum.None)
                {
                    Debug.Log("O jogo foi vencido por: " + m_winnerPlayer);
                }
                else
                {
                    Debug.Log("O jogo foi empate");
                }
            }
        }
    }

    private void ChangeCurrentPlayer()
    {
        var index = m_playersGame.IndexOf(m_currentPlayer);
        m_currentPlayer = index == 0 ? m_playersGame[1] : m_playersGame[0];
        m_gameCanvasUI.ShowCurrentPlayerTurn(m_currentPlayer);
    }
    private void SelectElement(RaycastHit2D hitInfo)
    {
        var elementClicked = hitInfo.collider.GetComponent<PlayerPieceElement>();
        if (m_currentObjectClick == null)
        {
            if (elementClicked.TypePlayer == m_currentPlayer)
            {
                m_currentObjectClick = elementClicked?.Select();
                SetIndicatorPosition(true);
            }
        }
        else
        {
            if (elementClicked == m_currentObjectClick)
            {
                m_currentObjectClick.Deselect();
                m_currentObjectClick = null;
                SetIndicatorPosition(false);
            }
            else
            {
                if (elementClicked.TypePlayer == m_currentPlayer)
                {
                    m_currentObjectClick.Deselect();
                    m_currentObjectClick = elementClicked;
                    SetIndicatorPosition(true);
                    m_currentObjectClick.Select();
                }
            }
        }
    }

    private void SetIndicatorPosition(bool isActive)
    {
        if(m_currentObjectClick != null)
        {
            var defaultPosition = m_currentObjectClick.transform.position;
            m_indicatorCurrentObjectClick.transform.position = new Vector3(defaultPosition.x, defaultPosition.y + m_indicatorOffset, defaultPosition.z);
        }
        m_indicatorCurrentObjectClick.SetActive(isActive);
    }
    #endregion
    #region Check victory or draw
    private bool CheckDraw()
    {
        var isBoardFull = true;
        m_minimumMapForce = 3;
        foreach (var pos in m_sessionMap)
        {
            if (pos.GetLastMovement().TypePlayer == TypePlayerEnum.None)
            {
                isBoardFull = false;
            }
            if(pos.GetLastMovement().PieceForce < m_minimumMapForce)
            {
                m_minimumMapForce = pos.GetLastMovement().PieceForce;
            }
        }
        return isBoardFull;
    }
    private bool CheckVictory()
    {
        var isGameDone = false;
        isGameDone = CheckRow();

        if (!isGameDone)
        {
            isGameDone = CheckCollum();
        }

        if (!isGameDone)
        {
            isGameDone = CheckDiagonal();
        }
        return isGameDone;
    }

    private bool CheckRow()
    {
        for (int i = 0; i < 3; i++)
        {
            var firstPosition = m_sessionMap[3 * i + 0].GetLastMovement();
            var secondPosition = m_sessionMap[3 * i + 1].GetLastMovement();
            var thirdPosition = m_sessionMap[3 * i + 2].GetLastMovement();
            if (firstPosition.TypePlayer == secondPosition.TypePlayer &&
                firstPosition.TypePlayer == thirdPosition.TypePlayer &&
                firstPosition.TypePlayer == m_currentPlayer)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckCollum()
    {
        for (int i = 0; i < 3; i++)
        {
            var firstPosition = m_sessionMap[3 * 0 + i].GetLastMovement();
            var secondPosition = m_sessionMap[3 * 1 + i].GetLastMovement();
            var thirdPosition = m_sessionMap[3 * 2 + i].GetLastMovement();
            if (firstPosition.TypePlayer == secondPosition.TypePlayer &&
                firstPosition.TypePlayer == thirdPosition.TypePlayer &&
                firstPosition.TypePlayer == m_currentPlayer)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckDiagonal()
    {
        var firstPosition = m_sessionMap[0].GetLastMovement();
        var secondPosition = m_sessionMap[4].GetLastMovement();
        var thirdPosition = m_sessionMap[8].GetLastMovement();
        if (firstPosition.TypePlayer == secondPosition.TypePlayer &&
            firstPosition.TypePlayer == thirdPosition.TypePlayer &&
            firstPosition.TypePlayer == m_currentPlayer)
        {
            return true;
        }

        firstPosition = m_sessionMap[2].GetLastMovement();
        secondPosition = m_sessionMap[4].GetLastMovement();
        thirdPosition = m_sessionMap[6].GetLastMovement();

        if (firstPosition.TypePlayer == secondPosition.TypePlayer &&
            firstPosition.TypePlayer == thirdPosition.TypePlayer &&
            firstPosition.TypePlayer == m_currentPlayer)
        {
            return true;
        }
        return false;
    }
    #endregion

    private void ShowCurrentBoardGame()
    {
        Debug.Log("Show Current Board Game");
        foreach (var pos in m_sessionMap)
        {
            Debug.Log("Position name " + pos.name + ": " + pos.GetLastMovement().TypePlayer + ", " + pos.GetLastMovement().PieceForce);
        }
    }
}

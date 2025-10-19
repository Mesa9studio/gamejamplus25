using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameCanvasUI : MonoBehaviour
{
    private const string MENU_SCENE = "Menu";
    private const string GAME_SCENE = "GamePlay";
    private const string DRAW_TITLE_MESSAGE = "Empate";
    private const string VICTORY_TITLE_MESSAGE = "Vitoria";
    private const string DRAW_SECOND_MESSAGE = "";
    private const string VICTORY_SECOND_MESSAGE = "O Vencedor foi o: ";
    private const string TURN_MESSAGE = "Vez: ";
    [SerializeField] private GameObject m_gameFinishObject;
    [SerializeField] private TextMeshProUGUI m_titleMessage;
    [SerializeField] private TextMeshProUGUI m_secondMessage;
    [SerializeField] private TextMeshProUGUI m_playerTurn;

    public void ShowResult(TypePlayerEnum winner)
    {
        m_gameFinishObject.SetActive(true);
        m_playerTurn.gameObject.SetActive(false);
        if (winner == TypePlayerEnum.None)
        {
            m_titleMessage.text = DRAW_TITLE_MESSAGE;
            m_secondMessage.text = DRAW_SECOND_MESSAGE;
        }
        else
        {
            m_titleMessage.text = VICTORY_TITLE_MESSAGE;
            m_secondMessage.text = VICTORY_SECOND_MESSAGE + winner;
        }
    }

    public void ShowCurrentPlayerTurn(TypePlayerEnum currentPlayer)
    {
        m_playerTurn.text = TURN_MESSAGE + currentPlayer;
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene(MENU_SCENE);
    }

    public void Rematch()
    {
        SceneManager.LoadScene(GAME_SCENE);
    }
}

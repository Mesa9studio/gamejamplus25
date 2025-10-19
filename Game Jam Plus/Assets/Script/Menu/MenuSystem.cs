using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_menuObject;
    [SerializeField] private GameObject m_tutorialObject;
    [SerializeField] private GameObject m_creditsObject;
    [SerializeField] private int m_tutorialCountPages;
    [SerializeField] private Sprite m_FinalAdvanceTutorial;
    [SerializeField] private List<Sprite> m_tutorialInfoSteps;
    [SerializeField] private Image m_imageInfoTutorial;
    [SerializeField] private Image m_advanceInfoTutorial;

    private const string GAME_SCENE_NAME = "GamePlay";
    public void StartButton()
    {
        m_menuObject.SetActive(false);
        m_tutorialObject.SetActive(true);
    }

    public void CreditsButton()
    {
        m_menuObject.SetActive(false);
        m_creditsObject.SetActive(true);
    }

    public void BackMenuButton()
    {
        m_creditsObject.SetActive(false);
        m_menuObject.SetActive(true);
    }
    public void ToggleMusic()
    {

    }

    public void AdvanceTutorial()
    {
        m_tutorialCountPages++;
        Debug.Log("m_tutorialCountPages < m_tutorialInfoSteps.Count: " + m_tutorialCountPages + ", " + m_tutorialInfoSteps);
        if (m_tutorialCountPages < m_tutorialInfoSteps.Count)
        {
            m_imageInfoTutorial.sprite = m_tutorialInfoSteps[m_tutorialCountPages];
            if (m_tutorialCountPages == m_tutorialInfoSteps.Count - 1)
            {
                m_advanceInfoTutorial.sprite = m_FinalAdvanceTutorial;
            }
        }
        else
        {
            SceneManager.LoadScene(GAME_SCENE_NAME);
            Debug.Log("Fim do Tutorial");
        }
    }
}

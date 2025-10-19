using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] private AnimationClip m_openingAnimation;

    private void Start()
    {
        StartCoroutine(AberturaAnimacao());
    }
    private IEnumerator AberturaAnimacao()
    {

        yield return new WaitForSecondsRealtime(m_openingAnimation.length);
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}

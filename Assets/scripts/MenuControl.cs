using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuControl : MonoBehaviour
{
    #region SERIALIZED FIELDS
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private Button startBtn;
    [SerializeField]
    private Button quitBtn;
    [SerializeField]
    private AudioSource sfxSource;
    #endregion

    #region MONO BEHAVIOURS
    void Start()
    {
        startBtn.onClick.AddListener(StartGame);
        quitBtn.onClick.AddListener(QuitGame);
    }
    #endregion

    #region PRIVATE METHODS
    void StartGame()
    {
        sfxSource.PlayOneShot(sfxSource.clip);
        SceneManager.LoadScene(1);
    }

    void QuitGame()
    {
        sfxSource.PlayOneShot(sfxSource.clip);
        Application.Quit();
    }
    #endregion
}

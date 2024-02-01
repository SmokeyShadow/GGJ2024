using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    #region STATIC FIELDS
    private static UIManager instance;
    #endregion

    #region SERIALIZED FIELDS
    [SerializeField]
    private Text text;
    [SerializeField]
    private Button quitBtn1;
    [SerializeField]
    private Button quitBtn2;
    [SerializeField]
    private Button restartBtn;
    [SerializeField]
    private Button resumeBtn;
    #endregion

    #region PROPERTIES
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<UIManager>();
            return instance;
        }
    }
    #endregion

    #region MONO BEHAVIOURS
    private void Start()
    {
        quitBtn1.onClick.AddListener(GameOver);
        quitBtn2.onClick.AddListener(Quit);
        restartBtn.onClick.AddListener(Restart);
    }
    #endregion

    #region PUBLIC METHODS
    public void SetText(string txt)
    {
        text.gameObject.SetActive(true);
        text.text = txt;
    }

    public void SetText(string txt , float showTime , float afterTime)
    {
        text.text = txt;
        StopAllCoroutines();
        StartCoroutine(ShowTextRoutine(showTime , afterTime));
    }

    public void GameOver()
    {
        SoundPlayer.Instance.PlaySound(SoundPlayer.SoundClip.UI_Click);
        Time.timeScale = 0;
        resumeBtn.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(true);
        quitBtn2.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SoundPlayer.Instance.PlaySound(SoundPlayer.SoundClip.UI_Click);
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void Resume()
    {
        SoundPlayer.Instance.PlaySound(SoundPlayer.SoundClip.UI_Click);
        Time.timeScale = 1;
        resumeBtn.gameObject.SetActive(false);
        restartBtn.gameObject.SetActive(false);
        quitBtn2.gameObject.SetActive(false);
    }

    public void Quit()
    {
        SoundPlayer.Instance.PlaySound(SoundPlayer.SoundClip.UI_Click);
        SceneManager.LoadScene(0);
    }
    #endregion

    #region COROUTINES
    IEnumerator ShowTextRoutine(float showTime, float afterTime)
    {
        yield return new WaitForSecondsRealtime(afterTime);
        text.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(showTime);
        text.gameObject.SetActive(false);
    }
    #endregion
}

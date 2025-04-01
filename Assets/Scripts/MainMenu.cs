using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClickClip;//sfx2, sfx3;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonSound()
    {
        PlayClip(buttonClickClip);
    }

    private void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private IEnumerator WaitAndLoadScene(int LoadScene)
    {
        PlayButtonSound();

        // Wait for the sound to finish before loading the scene
        yield return new WaitForSeconds(0.25f);

        SceneManager.LoadScene(LoadScene);
    }

    public void NewGame(int level)
    {
        Time.timeScale = 1f;
        StartCoroutine(WaitAndLoadScene(level));
    }

    public void StartOverLvl1()
    {
        Time.timeScale = 1f;
        StartCoroutine(WaitAndLoadScene(1));
    }

    public void PauseGame()
    {
        PlayButtonSound();
        Time.timeScale = 0f;
        Timer.Instance.StopTimer();
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        PlayButtonSound();
        Timer.Instance.ResumeTimer();
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        //PlayButtonSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next level exists
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(WaitAndLoadScene(nextSceneIndex));
        }
    }

    public void LoadMainMenuWithoutWait()
    {
        Time.timeScale = 1f;
        PlayButtonSound();
        SceneManager.LoadScene(0);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(WaitAndLoadScene(0));
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        StartCoroutine(WaitAndExit());
    }

    private IEnumerator WaitAndExit()
    {
        PlayButtonSound();
        yield return new WaitForSeconds(0.4f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
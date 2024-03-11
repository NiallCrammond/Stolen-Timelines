using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

     public Animator animator;
    public static LevelManager instance;
    public string[] sceneNames;


 

    public void loadMainMenu()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(loadLevel(sceneNames[0], 1));
    }

 public void loadGameLevel()
    {
        StartCoroutine(loadLevel(sceneNames[1], 1));
    }


    public IEnumerator loadLevel(string sceneID, float transitionTime)

    {
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID);
        asyncLoad.allowSceneActivation = false;

        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        asyncLoad.allowSceneActivation = true;

        while(!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.LoadScene(sceneID);
    }
}

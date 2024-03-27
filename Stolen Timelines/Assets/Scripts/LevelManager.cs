using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public Animator[] animator;
    public static LevelManager instance;
    public string[] sceneNames;

    private bool isReady = true;


    private GameController gc;
    private void Awake()
    {
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }



    public void loadMainMenu()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(loadLevel(sceneNames[0], animator[0], "Start", 1));
    }

    public void loadGameLevel()
    {
        StartCoroutine(loadLevel(sceneNames[1], animator[0], "Start", 5));
    }

    public void loadHub()
    {
        StartCoroutine(loadLevel(sceneNames[2], animator[0], "Start", 1));
    }

    public void loadInstruction()
    {


        //StartCoroutine(loadLevel(sceneNames[3], animator[0], "Start", 1));
        gc.level = SceneManager.LoadSceneAsync(sceneNames[1]);
        gc.level.allowSceneActivation = false;
        SceneManager.LoadScene(sceneNames[3]);
    }


    public IEnumerator loadLevel(string sceneID, Animator anim, string trigger, float transitionTime)

    {
        if (isReady)
        {
            isReady = false;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID);
             asyncLoad.allowSceneActivation = false;
            anim.SetTrigger(trigger);
            yield return new WaitForSeconds(transitionTime);

            asyncLoad.allowSceneActivation = true;

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            isReady = true;
            SceneManager.LoadScene(sceneID);

        }
    }

}

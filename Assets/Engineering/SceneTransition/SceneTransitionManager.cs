using DG.Tweening;
using ScriptableObjectDependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{

    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private Material fadeBlackMaterial;


    [SerializeField] NullEvent onSceneExitStart;
    [SerializeField] NullEvent onSceneExitEnd;
    [SerializeField] NullEvent onSceneEnterStart;
    [SerializeField] NullEvent onSceneEnterEnd;

    public static SceneTransitionManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null) {
            Debug.Log("More than 1 SceneTransitionManager in scene!");
            Destroy(this.gameObject);
            return;
        } 

        Instance = this;
        DontDestroyOnLoad(gameObject);

        fadeBlackMaterial.SetFloat("_Blackness", 1f);
        currentTransition = DOTween.Sequence()
                .AppendInterval(0.5f)
                .AppendCallback(() => onSceneEnterStart.Raise(null))
                .Append(DOTween.To(() => fadeBlackMaterial.GetFloat("_Blackness"), x => fadeBlackMaterial.SetFloat("_Blackness", x), 0f, fadeDuration))
                .AppendCallback(() => onSceneEnterEnd.Raise(null))
                .OnKill(() => currentTransition = null);
    }


    Sequence currentTransition;

    void SceneTransition(string sceneName) {
        onSceneExitStart.Raise(null);


        onSceneExitStart.Raise(null);
        currentTransition = DOTween.Sequence()
            .Append(DOTween.To(() => fadeBlackMaterial.GetFloat("_Blackness"), x => fadeBlackMaterial.SetFloat("_Blackness", x), 1f, fadeDuration))
            .AppendCallback(() => onSceneExitEnd.Raise(null))
            .AppendInterval(0.75f)
            .AppendCallback(() => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName))
            .AppendInterval(0.75f)
            .AppendCallback(() => onSceneEnterStart.Raise(null))
            .Append(DOTween.To(() => fadeBlackMaterial.GetFloat("_Blackness"), x => fadeBlackMaterial.SetFloat("_Blackness", x), 0f, fadeDuration))
            .AppendCallback(() => onSceneEnterEnd.Raise(null))
            .OnKill(() => currentTransition = null);


    }

    public void Transition(string sceneName) {
        if (currentTransition.IsActive()) {
            Debug.Log("A transition is already in progress!");
            return;
        }

        SceneTransition(sceneName);
    }
}

using DG.Tweening;
using Febucci.UI;
using ScriptableObjectDependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CreditsAchievementManager : MonoBehaviour
{
    [SerializeField] Sprite goodImage;
    [SerializeField] Sprite badImage;

    [SerializeField] TextMeshProUGUI timeText;

    [SerializeField] Achievement[] achievements;

    [SerializeField] BoolReference noShot;
    [SerializeField] BoolReference speedrun;
    [SerializeField] BoolReference allCheered;
    [SerializeField] BoolReference noCasualties;

    [SerializeField] FloatReference gameTime;
    // Start is called before the first frame update
    void Start()
    {
        SetCredits();
        achievementBox.localScale = Vector3.zero;

    }

    [SerializeField] RectTransform achievementBox;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TypewriterByCharacter typewriter;

    [SerializeField] PlayerInput clickToRestart;

    [SerializeField] RectTransform restartText;
    Sequence restartBob;
    public void OnSceneEnterEnd() {
        restartBob = DOTween.Sequence()
            .Append(restartText.DOScale(Vector3.one * 1.1f, 2f).SetEase(Ease.InOutSine))
            .Append(restartText.DOScale(Vector3.one, 2f).SetEase(Ease.InOutSine))
            .SetLoops(-1);

        DOTween.Sequence()
            .AppendCallback(() => typewriter.ShowText("So it is done."))
            .AppendInterval(6f)
            .AppendCallback(() => typewriter.ShowText("The only holiday that remains is Christmas."))
            .AppendInterval(7f)
            .AppendCallback(() => typewriter.ShowText("With the eradication of all other holidays from the world..."))
            .AppendInterval(10f)
            .AppendCallback(() => typewriter.ShowText("What else is there for santa to do...?"))
            .AppendInterval(8f)
            .Append(dialogueText.DOColor(Color.clear, 1f))
            .AppendCallback(() => dialogueText.gameObject.SetActive(false))
            .Append(achievementBox.DOScale(Vector3.one, 2f).SetEase(Ease.OutSine))
            .AppendCallback(() => clickToRestart.enabled = true);

    }

    public void Restart() {
        clickToRestart.enabled = false;
        SceneTransitionManager.Instance.Transition("MelonJamScene");
    }

    [SerializeField] float timeLimit = 1200f;
    [SerializeField] float timeFloor = 600f;
    [Serializable]
    struct Achievement {
        public Image image;
        public TextMeshProUGUI name;
    }

    void SetCredits() {
        bool[] targets = new bool[] { noShot.Value, speedrun.Value, allCheered.Value, noCasualties.Value };
        for (int i = 0; i < achievements.Length; i++) {
            if (targets[i]) {
                achievements[i].image.sprite = goodImage;
                achievements[i].name.color = Color.green;
            }
            else {
                achievements[i].image.sprite = badImage;
                achievements[i].name.color = Color.red;
            }
        }


        string outTime = "";
        int hours = Mathf.FloorToInt(gameTime.Value / 3600);
        int minutes = Mathf.FloorToInt((gameTime.Value - hours * 3600) / 60);
        int seconds = Mathf.FloorToInt(gameTime.Value - hours * 3600 - minutes * 60);
        if (gameTime.Value > 3600) {
            outTime += hours.ToString();
            if (hours == 1) {
                outTime += " hour ";
            }
            else {
                outTime += " hours ";
            }
        }
        if (gameTime.Value > 60f) {
            outTime += minutes.ToString();
            if (minutes == 1) {
                outTime += " minute ";
            }
            else {
                outTime += " minutes ";
            }
        }
        if (gameTime.Value > 0) {
            outTime += seconds.ToString();
            if (seconds == 1) {
                outTime += " second ";
            }
            else {
                outTime += " second ";
            }
        }

        timeText.text = outTime;
        float timeDiff = timeLimit - gameTime.Value;
        timeText.color = Color.Lerp(Color.red, Color.green, timeDiff / (timeLimit - timeFloor));
    }
}

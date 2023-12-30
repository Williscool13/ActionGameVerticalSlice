using DG.Tweening;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private BoolVariable noShoot;
    [SerializeField] private BoolVariable underTenMinutes;
    [SerializeField] private BoolVariable cheeredAllEnemies;
    [SerializeField] private BoolVariable noCasualties;

    [SerializeField] private FloatVariable gameTime;

    [SerializeField] private int enemyCount = 0;

    [SerializeField] private GameObject convertParent;
    [SerializeField] private TextMeshProUGUI convertCount;

    int currentEnemyCount = 0;

    float timer;
    Vector3 basePopupPos;

    public static AchievementManager Instance;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this.gameObject);
            Debug.LogError("Multiple Achievement Managers in scene");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetStats();

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnSceneChanged(Scene oldScene, Scene newScene) {
        if (newScene.name == "MelonJamScene") { ResetStats(); }
    }

    void ResetStats() {
        currentEnemyCount = 0;
        convertCount.text = currentEnemyCount.ToString() + "/" + enemyCount; 

        noShoot.Value = true;
        underTenMinutes.Value = true;
        cheeredAllEnemies.Value = false;
        noCasualties.Value = true;
        basePopupPos = achievementPopupPanel.position;
        timer = 0.0f;
        gameTime.Value = 0.0f;
        Debug.Log("Reset Stats");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        gameTime.Value = timer;
        
        if (underTenMinutes.Value == false) { return; }
        if (timer > 600.0f) {
            underTenMinutes.Value = false;
            achievementArray[1] = 0;
            OnAchievement(1);
        }
    }


    public void OnShoot() {
        if (noShoot.Value == false) { return; }
        noShoot.Value = false;
        achievementArray[0] = 0;
        OnAchievement(0);
    }

    public void OnEnemyCheered() {
        if (cheeredAllEnemies.Value == true) { return; }
        currentEnemyCount++;
        convertCount.text = currentEnemyCount.ToString() + "/" + enemyCount;
        if (currentEnemyCount >= enemyCount) { convertCount.color = new Color(124f / 255f, 255f / 255f, 124f / 255f); } 
        else { convertCount.color = new Color(255f / 255f, 100f / 255f, 100f / 255f); }
        if (currentEnemyCount >= enemyCount) {
            cheeredAllEnemies.Value = true;
            achievementArray[2] = 1;
            OnAchievement(2);
        }
    }

    public void OnChristmasCasualty() {
        if (noCasualties.Value == false) { return; }
        noCasualties.Value = false;
        achievementArray[3] = 0;
        OnAchievement(3);
    }

    public void OnSceneEnterEnd() {
        if (SceneManager.GetActiveScene().name == "MelonJamScene") {
            convertParent.SetActive(true);
        }
    }

    [Title("Achievement Popup")]
    [SerializeField] Canvas achievementCanvas;
    [SerializeField] RectTransform achievementPopupPanel;
    [SerializeField] TextMeshProUGUI achievementTitle;
    [SerializeField] TextMeshProUGUI achievementText;
    [SerializeField] Image achievementImage;

    [SerializeField] float popupDistance = 100.0f;
    [SerializeField] float popupDuration = 1.0f;

    [SerializeField] Sprite positiveImage;
    [SerializeField] Sprite negativeImage;

    Sequence achievementPopup;


    // 1 noshoot
    // 2 under 10 minutes
    // 3 cheered all enemies
    // 4 no casualties
    int[] achievementArray = new int[4] { 1, 1, 0, 1 };
    void OnAchievement(int achievementIndex) {
        string baseText = achievementIndex switch {
            0 => "Pacifist",
            1 => "Speedrunner",
            2 => "Cheerful",
            3 => "Disciplined",
            _ => "Error",
        };
        if (achievementArray[achievementIndex] == 1) {
            achievementTitle.text = "Achievement Get!";
            achievementTitle.color = Color.green;
            achievementText.text = baseText;
            achievementImage.sprite = positiveImage;
            
        } else {
            achievementTitle.text = "Achievement Failed!";
            achievementTitle.color = Color.red;
            achievementText.text = baseText;
            achievementImage.sprite = negativeImage;
        }
        
        if (achievementPopup.IsActive()) {
            achievementPopup.Kill();
        }

        achievementPopupPanel.position = basePopupPos; 

        achievementPopup = DOTween.Sequence()
            .Append(achievementPopupPanel.DOMoveY(basePopupPos.y - popupDistance, 1.0f))
            .AppendInterval(popupDuration)
            .Append(achievementPopupPanel.DOMoveY(basePopupPos.y, 1.0f))
            .AppendCallback(() => achievementPopupPanel.position = basePopupPos)
            .OnKill(() => achievementPopup = null);

    }


}

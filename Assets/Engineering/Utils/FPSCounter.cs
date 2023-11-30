using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public static FPSCounter Instance;
    
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] bool editorAndDebugOnly = true;
    [SerializeField] bool singleton = true;
    
    float averageFramerate;
    private void Awake() {
        if (editorAndDebugOnly) {
            if (!Application.isEditor && !Debug.isDebugBuild) {
                Destroy(this.gameObject);
            }
        }

        if (!singleton) return;

        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(transform.gameObject);
        }
    }
    void Update()
    {
        float fps = 1f / Time.deltaTime;
        averageFramerate += (fps - averageFramerate) * 0.01f;
        fpsText.text = averageFramerate.ToString("F0") + " fps";
    }
}

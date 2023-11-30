using UnityEngine;
public class FramerateSetter : MonoBehaviour
{
    [SerializeField] int vysncCount = 1;
    [SerializeField] bool screenRefreshRateTarget = true;
    [SerializeField] float targetFrameRate = 60f;
    void Start() { 
        QualitySettings.vSyncCount = this.vysncCount;
        if (this.screenRefreshRateTarget) Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        else Application.targetFrameRate = (int)this.targetFrameRate;
    } 
}

using UnityEngine;

public class Game : MonoBehaviour
{
    private void Start()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled // TODO: Move to Game.cs Script.
        Application.targetFrameRate = 60;
#endif
    }

}

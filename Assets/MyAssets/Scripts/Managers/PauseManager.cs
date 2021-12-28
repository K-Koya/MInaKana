using UnityEngine;

/// <summary>
/// ポーズ処理を実施
/// </summary>
public class PauseManager : Singleton<PauseManager>
{
    [SerializeField, Tooltip("ポーズメニューボタン名")]
    private string _pauseButtonName = "Cancel";

    /// <summary>
    /// IPauseAndResumeのOnPauseを定義する
    /// </summary>
    public delegate void OnPause();
    /// <summary>
    /// IPauseAndResumeのOnResumeを定義する
    /// </summary>
    public delegate void OnResume();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/// <summary>
/// ポーズ処理対象にするコンポーネントに継承させる
/// </summary>
public interface IPauseAndResume
{
    /// <summary>
    /// ポーズになった瞬間に実施する処理
    /// </summary>
    public void OnPause();

    /// <summary>
    /// ポーズ解除と同時に実施する処理
    /// </summary>
    public void OnResume();
}

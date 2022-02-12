using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンの状態を制御するコンポーネント
/// </summary>
public class MySceneManager : Singleton<MySceneManager>
{
    #region シーン遷移機能
    [Header("BuildSettingで登録したScene名を以下に登録する")]
    [SerializeField, Tooltip("Scene名 : タイトル")]
    string _SceneNameTitle = "Title";

    [SerializeField, Tooltip("Scene名 : 自然エリア")]
    string _SceneNameGrassField = "GrassField";

    /// <summary> Scene遷移でかけるエフェクトを操作するAnimator </summary>
    Animator _EffectAnimator = default;

    [Header("Animatorで登録したエフェクト用Animation名を以下に登録する")]
    [SerializeField, Tooltip("Animation名 : 円形切り抜きの暗転をする")]
    string _AnimNameCircleBlackIning = "CircleBlackIning";

    [SerializeField, Tooltip("Animation名 : 暗転から円形切り抜きしつつ明転する")]
    string _AnimNameCircleBlackOuting = "CircleBlackOuting";
    #endregion

    #region ポーズ処理機能
    /// <summary>
    /// IPauseAndResumeのOnPauseを定義する
    /// </summary>
    public delegate void OnPause();
    /// <summary>
    /// IPauseAndResumeのOnResumeを定義する
    /// </summary>
    public delegate void OnResume();
    #endregion

    #region プロパティ
    /// <summary> Scene名 : タイトル </summary>
    public string SceneNameTitle { get => _SceneNameTitle; }
    /// <summary> Scene名 : 自然エリア </summary>
    public string SceneNameGrassField { get => _SceneNameGrassField; }
    #endregion

    protected override void Awake()
    {
        IsDontDestroyOnLoad = true;
        base.Awake();
    }

    void Start()
    {
        _EffectAnimator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// 任意のエフェクトをかけながらScene遷移
    /// </summary>
    /// <param name="sceneName">遷移先Scene名</param>
    /// <param name="delay">遷移に要する時間</param>
    /// <param name="type">エフェクトの種類</param>
    public void SceneChange(string sceneName, float delay = 0f, LoadSceneEffectType type = LoadSceneEffectType.None)
    {
        //Scene遷移エフェクトを要求別に指定
        switch (type)
        {
            case LoadSceneEffectType.CircleBlack:
                SceneManager.sceneLoaded += OnLoadedNext;
                _EffectAnimator.Play(_AnimNameCircleBlackIning);
                break;
            default: break;
        }

        //Scene遷移をdelay分遅延実行
        StartCoroutine(DelayLoadScene(sceneName, delay));
    }

    /// <summary> 次のSceneが読み込まれた時に実行するメソッド </summary>
    void OnLoadedNext(Scene scene, LoadSceneMode mode)
    {
        _EffectAnimator.Play(_AnimNameCircleBlackOuting);
    }

    /// <summary>Scene遷移コルーチン</summary>
    /// <param name="sceneName">遷移先Scene名</param>
    /// <param name="delay">遅延時間</param>
    IEnumerator DelayLoadScene(string sceneName, float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        //Scene遷移
        SceneManager.LoadScene(sceneName);
    }
}

/// <summary>
/// Sceneをまたぐ際にはさむエフェクトの種類
/// </summary>
public enum LoadSceneEffectType
{
    /// <summary> 何もしない </summary>
    None,
    /// <summary> 円形の切り抜きをしながら暗転フェード </summary>
    CircleBlack,
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

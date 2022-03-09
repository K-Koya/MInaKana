using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary> タイトル画面を制御するコンポーネント </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary> Timelineカットを制御するコンポーネント </summary>
    PlayableDirector _PD = default;

    /// <summary> BGM再生用のAudioSource </summary>
    AudioSource _BGNSpeaker = default;

    [Header("BGM用AudioClipのアサイン要求")]
    [SerializeField, Tooltip("タイトルBGMをアサイン")]
    AudioClip _AudioTitle = default;


    [Header("Timeline用アサイン要求")]
    [SerializeField, Tooltip("タイムライン開始時に、非アクティブ化するオブジェクト")]
    GameObject[] onStartDisableObjects = default;

    [SerializeField, Tooltip("タイムライン開始時に、アクティブ化するオブジェクト")]
    GameObject[] onStartEnableObjects = default;

    [SerializeField, Tooltip("タイムライン終了時に、非アクティブ化するオブジェクト")]
    GameObject[] onEndDisableObjects = default;

    [SerializeField, Tooltip("タイムライン終了時に、アクティブ化するオブジェクト")]
    GameObject[] onEndEnableObjects = default;

    [Header("Animator用アサイン要求")]
    [SerializeField, Tooltip("1P用スタートボタンのアニメーター")]
    Animator _StartOrderAnim1 = default;

    [SerializeField, Tooltip("2P用スタートボタンのアニメーター")]
    Animator _StartOrderAnim2 = default;

    [SerializeField, Tooltip("Animation名 : 押下要求")]
    string _AnimNameStartPushOrder = "StartPushOrder";

    [SerializeField, Tooltip("Animation名 : 押下中サイン")]
    string _AnimNameStartPushNow = "StartPushNow";

    /// <summary>1Pスタート用ボタン入力状態</summary>
    bool _IsPush1P = false;

    /// <summary>2Pスタート用ボタン入力状態</summary>
    bool _IsPush2P = false;

    // Start is called before the first frame update
    void Start()
    {
        _PD = FindObjectOfType<PlayableDirector>();
        _BGNSpeaker = GetComponent<AudioSource>();

        //カット再生・終了時に実行したいメソッドを定義
        _PD.played += OnStart;
        _PD.stopped += OnEnd;

        _BGNSpeaker.clip = _AudioTitle;
        _BGNSpeaker.Play();
    }

    // Update is called once per frame
    void Update()
    {
        bool push1P = InputAssistant.GetJump(1);
        bool push2P = InputAssistant.GetJump(2);

        //入力情報が更新されたらアニメーションを変更
        if(_IsPush1P != push1P)
        {
            if (push1P) _StartOrderAnim1.Play(_AnimNameStartPushNow);
            else _StartOrderAnim1.Play(_AnimNameStartPushOrder);
            _IsPush1P = push1P;
        }
        if(_IsPush2P != push2P)
        {
            if (push2P) _StartOrderAnim2.Play(_AnimNameStartPushNow);
            else _StartOrderAnim2.Play(_AnimNameStartPushOrder);
            _IsPush2P = push2P;
        }

        //入力があればデモバトルシーンへ丸い切り抜きエフェクトをかけて遷移
        if (push1P && push2P)
        {
            MySceneManager.I.SceneChange(MySceneManager.I.SceneNameGrassField, 1f, LoadSceneEffectType.CircleBlack);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// カット再生時に実行するメソッド
    /// </summary>
    /// <param name="pd">該当のPlayableDirector</param>
    void OnStart(PlayableDirector pd)
    {
        if (_PD != pd) return;

        Array.ForEach(onStartDisableObjects, o => o.SetActive(false));
        Array.ForEach(onStartEnableObjects, o => o.SetActive(true));
    }

    /// <summary>
    /// カット終了時に実行するメソッド
    /// </summary>
    /// <param name="pd">該当のPlayableDirector</param>
    void OnEnd(PlayableDirector pd)
    {
        if (_PD != pd) return;

        Array.ForEach(onEndDisableObjects, o => o.SetActive(false));
        Array.ForEach(onEndEnableObjects, o => o.SetActive(true));
    }
}

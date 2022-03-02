using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 戦闘の状況
/// </summary>
public enum BattleSituation : byte
{
    /// <summary> 戦闘中でない </summary>
    NotOnBattle,
    /// <summary> 導入 </summary>
    Introduction,
    /// <summary> 戦闘中 </summary>
    OnBattle,
    /// <summary> 戦闘の合間のイベント中 </summary>
    Interval,
    /// <summary> プレイヤー勝ち </summary>
    PlayerWin,
    /// <summary> プレイヤー負け </summary>
    PlayerLose
}

/// <summary>
/// 戦闘を制御するコンポーネント
/// </summary>
public class BattleManager : MonoBehaviour
{
    #region メンバ
    /// <summary> 各キャラクターのRapidAccumulationがこの値に達すると行動できる </summary>
    const short TURN_BORDER = 500;

    /// <summary> 戦闘の状況 </summary>
    static BattleSituation _Situation = BattleSituation.Introduction;

    /// <summary> 各戦闘中のキャラクターのステータスへのアクセッサ </summary>
    List<CharacterStatus> _BattleCharacters = default;

    /// <summary> 現在の行動者のステータス </summary>
    CharacterStatus _TurnOwner = default;

    /// <summary> 次の行動者を見つけて指示させるイテレーター </summary>
    IEnumerator _TurnInstructerIterator = default;

    /// <summary> Timelineカットを制御するコンポーネント </summary>
    PlayableDirector _PD = default;

    [SerializeField, Tooltip("タイムライン開始時に、非アクティブ化するオブジェクト")]
    GameObject[] onStartDisableObjects = default;

    [SerializeField, Tooltip("タイムライン開始時に、アクティブ化するオブジェクト")]
    GameObject[] onStartEnableObjects = default;

    [SerializeField, Tooltip("タイムライン終了時に、非アクティブ化するオブジェクト")]
    GameObject[] onEndDisableObjects = default;

    [SerializeField, Tooltip("タイムライン終了時に、アクティブ化するオブジェクト")]
    GameObject[] onEndEnableObjects = default;

    [SerializeField, Tooltip("戦闘開始時のカット")]
    PlayableAsset _CutForIntroduction = default;

    [SerializeField, Tooltip("戦闘勝利時のカット")]
    PlayableAsset _CutForWin = default;

    [SerializeField, Tooltip("戦闘敗北時のカット")]
    PlayableAsset _CutForLose = default;
    #endregion

    #region プロパティ
    /// <summary> 各戦闘中のキャラクターのうち、戦闘可能な者のアクセッサ </summary>
    private List<CharacterStatus> ActiveCharacters { get => _BattleCharacters.Where(bc => !bc.IsDefeated).ToList(); }
    /// <summary> 戦闘の状況 </summary>
    public static BattleSituation Situation { get => _Situation; set => _Situation = value; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _Situation = BattleSituation.Introduction;

        _PD = FindObjectOfType<PlayableDirector>();
        //戦闘に参加中のキャラクター全員のステータスを取得
        _BattleCharacters = FindObjectsOfType<CharacterStatus>().ToList();

        //カット再生・終了時に実行したいメソッドを定義
        _PD.played += OnStart;
        _PD.stopped += OnEnd;

        //戦闘開始カットを再生
        _PD.Play(_CutForIntroduction);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_Situation)
        {
            case BattleSituation.OnBattle:
                if (!_TurnOwner || !_TurnOwner.IsMyTurn)
                {
                    //イテレーター未定義なら定義、定義済みなら次のyieldまで実行し、最後まで実行済みなら始めから
                    if(_TurnInstructerIterator == null) _TurnInstructerIterator = TurnInstructer();
                    else _TurnInstructerIterator.MoveNext();
                    CheckSettlementToResult();
                }
                break;
            case BattleSituation.PlayerWin:
                if (InputAssistant.GetDownMenu)
                {
                    _TurnInstructerIterator = null;
                    MySceneManager.I.SceneChange(MySceneManager.I.SceneNameTitle, 1f, LoadSceneEffectType.CircleBlack);
                }
                break;
            case BattleSituation.PlayerLose:
                if (InputAssistant.GetDownMenu)
                {
                    _TurnInstructerIterator = null;
                    MySceneManager.I.SceneChange(MySceneManager.I.SceneNameTitle, 1f, LoadSceneEffectType.CircleBlack);
                }
                break;
            default: break;
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

        //バトル導入時なら、オブジェクトの有効化をする
        switch (_Situation)
        {
            case BattleSituation.Introduction:
                Array.ForEach(onEndDisableObjects, o => o.SetActive(false));
                Array.ForEach(onEndEnableObjects, o => o.SetActive(true));
                _Situation = BattleSituation.OnBattle;
                break;
            case BattleSituation.PlayerWin:
            case BattleSituation.PlayerLose:

                break;
            default: break;
        }
    }

    /// <summary>
    /// 次の行動者を見つけて指示する
    /// </summary>
    IEnumerator TurnInstructer()
    {
        yield return null;

        //敏捷蓄積値順にソート
        _BattleCharacters = _BattleCharacters.OrderByDescending(b => b.RapidAccumulation).ToList();

        //次のターンの行動者を保管
        _TurnOwner = ActiveCharacters.Where(b => b.RapidAccumulation >= TURN_BORDER).FirstOrDefault();

        //誰かのRapidAccumulationがTURN_BORDERを超えるまで、全員敏捷値を加算
        while (!_TurnOwner)
        {
            _BattleCharacters.ForEach(b => b.RapidAccumulation += b.Rapid);
            _TurnOwner = ActiveCharacters.Where(b => b.RapidAccumulation >= TURN_BORDER).FirstOrDefault();

            yield return null;
        }

        //あなたのターンです
        _TurnOwner.IsMyTurn = true;
        _TurnOwner.RapidAccumulation -= TURN_BORDER;

        Debug.Log(_TurnOwner.Name + " のターンです。");

        //このイテレーターを止める
        _TurnInstructerIterator = null;
    }

    /// <summary>
    /// 決着がついたかを判定し、ついたなら戦闘後処理
    /// </summary>
    void CheckSettlementToResult()
    {
        //プレイヤーが一人も残っていない場合ゲームオーバー
        if (ActiveCharacters.OfType<PlayerStatus>().ToList().Count < 1)
        {
            _Situation = BattleSituation.PlayerLose;
            _PD.Play(_CutForLose);
            GUIPlayersInputNavigation.OrderReset();
        }
        //プレイヤーが残っていて敵が一体も残っていない場合勝利
        else if (ActiveCharacters.OfType<EnemyStatus>().ToList().Count < 1)
        {
            _Situation = BattleSituation.PlayerWin;
            _PD.Play(_CutForWin);
            GUIPlayersInputNavigation.OrderReset();
        }
    }
}

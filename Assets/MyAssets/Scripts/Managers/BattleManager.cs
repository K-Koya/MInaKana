using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

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
    static BattleSituation _Situation = BattleSituation.OnBattle;

    /// <summary> 各戦闘中のキャラクターのステータスへのアクセッサ </summary>
    List<CharacterStatus> _BattleCharacters = default;

    /// <summary> 現在の行動者のステータス </summary>
    CharacterStatus _TurnOwner = default;
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
        //戦闘に参加中のキャラクター全員のステータスを取得
        _BattleCharacters = FindObjectsOfType<CharacterStatus>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        //戦闘中のみ実行
        if (_Situation != BattleSituation.OnBattle) return;

        if (!_TurnOwner || !_TurnOwner.IsMyTurn)
        {
            CheckSettlementToResult();
            TurnInstructer();
        }
    }

    /// <summary>
    /// 次の行動者を見つけて指示する
    /// </summary>
    void TurnInstructer()
    {
        //敏捷蓄積値順にソート
        _BattleCharacters = _BattleCharacters.OrderByDescending(b => b.RapidAccumulation).ToList();

        //次のターンの行動者を保管
        _TurnOwner = ActiveCharacters.Where(b => b.RapidAccumulation >= TURN_BORDER).FirstOrDefault();

        //誰かのRapidAccumulationがTURN_BORDERを超えるまで、全員敏捷値を加算
        while (!_TurnOwner)
        {
            _BattleCharacters.ForEach(b => b.RapidAccumulation += b.Rapid);
            _TurnOwner = ActiveCharacters.Where(b => b.RapidAccumulation >= TURN_BORDER).FirstOrDefault();
        }

        //あなたのターンです
        _TurnOwner.IsMyTurn = true;
        _TurnOwner.RapidAccumulation -= TURN_BORDER;

        Debug.Log(_TurnOwner.Name + " のターンです。");
    }

    /// <summary>
    /// 決着がついたかを判定し、ついたなら戦闘後処理
    /// </summary>
    void CheckSettlementToResult()
    {
        //プレイヤーが一人も残っていない場合ゲームオーバー
        if (ActiveCharacters.OfType<PlayerStatus>().ToList().Count < 1) _Situation = BattleSituation.PlayerLose;
        //プレイヤーが残っていて敵が一体も残っていない場合勝利
        else if (ActiveCharacters.OfType<EnemyStatus>().ToList().Count < 1) _Situation = BattleSituation.PlayerWin;
    }
}

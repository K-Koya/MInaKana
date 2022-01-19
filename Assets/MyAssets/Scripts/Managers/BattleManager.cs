using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// 戦闘を制御するコンポーネント
/// </summary>
public class BattleManager : MonoBehaviour
{
    /// <summary> 各キャラクターのRapidAccumulationがこの値に達すると行動できる </summary>
    const short TURN_BORDER = 500;

    /// <summary> 各戦闘中のキャラクターのステータスへのアクセッサ </summary>
    CharacterStatus[] _BattleCharacters = default;

    /// <summary> 現在の行動者のステータス </summary>
    CharacterStatus _TurnOwner = default;

    // Start is called before the first frame update
    void Start()
    {
        //戦闘に参加中のキャラクター全員のステータスを取得
        _BattleCharacters = FindObjectsOfType<CharacterStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_TurnOwner || !_TurnOwner.IsMyTurn) TurnInstructer();
    }

    /// <summary>
    /// 次の行動者を見つけて指示する
    /// </summary>
    void TurnInstructer()
    {
        //次のターンの行動者を保管
        _TurnOwner = _BattleCharacters.Where(b => !(b.RapidAccumulation < TURN_BORDER)).FirstOrDefault();

        //誰かのRapidAccumulationがTURN_BORDERを超えるまで、全員敏捷値を加算
        while (!_TurnOwner)
        {
            Array.ForEach(_BattleCharacters, b => b.RapidAccumulation += b.Rapid);
            _TurnOwner = _BattleCharacters.OrderByDescending(b => b.RapidAccumulation).Where(b => !(b.RapidAccumulation < TURN_BORDER)).FirstOrDefault();
        }

        //あなたのターンです
        _TurnOwner.IsMyTurn = true;
        _TurnOwner.RapidAccumulation -= TURN_BORDER;

        Debug.Log(_TurnOwner.Name + " のターンです。");
    }
}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 同オブジェクトのキャラクターの戦闘時の操作を受け付ける
/// </summary>
[RequireComponent(typeof(CharacterStatus))]
public abstract class BattleOperator : MonoBehaviour
{
    #region メンバー
    /// <summary> 戦闘時の初期位置 </summary>
    protected Vector3 _BasePosition = default;

    /// <summary> 同オブジェクトのキャラクターの能力値 </summary>
    protected CharacterStatus _Status = default;

    /// <summary> プレイヤー側のステータス </summary>
    protected PlayerStatus[] _Players = default;

    /// <summary> 敵側のステータス </summary>
    protected EnemyStatus[] _Enemies = default;

    /// <summary> 各コマンド実行時に、施行する相手 </summary>
    protected List<CharacterStatus> _CommandTargets = new List<CharacterStatus>();

    /// <summary> 実行中のコマンドの流れ </summary>
    protected Coroutine _RunningCommand = default;
    #endregion


    #region プロパティ
    /// <summary> 戦闘時の初期位置 </summary>
    public Vector3 BasePosition { get => _BasePosition; set => _BasePosition = value; }
    /// <summary> プレイヤー側のステータス </summary>
    public PlayerStatus[] Players { get => _Players; }
    /// <summary> 敵側のステータス </summary>
    public EnemyStatus[] Enemies { get => _Enemies; }
    #endregion

    protected virtual void Awake()
    {
        _Status = GetComponent<CharacterStatus>();
        _Players = FindObjectsOfType<PlayerStatus>();
        _Enemies = FindObjectsOfType<EnemyStatus>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //初期位置を原点に
        _BasePosition = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_Status.IsMyTurn)
        {
            OperateCommand();
        }
    }

    /// <summary>
    /// 行動を実施
    /// </summary>
    protected abstract void OperateCommand();
}

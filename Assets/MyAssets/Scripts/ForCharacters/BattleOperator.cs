using System.Linq;
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

    /// <summary> プレイヤー側の戦闘用コンポーネント </summary>
    protected BattleOperatorForPlayer[] _Players = default;

    /// <summary> 敵側の戦闘用コンポーネント </summary>
    protected BattleOperatorForEnemy[] _Enemies = default;

    /// <summary> プレイヤー側のステータス </summary>
    //protected PlayerStatus[] _Players = default;

    /// <summary> 敵側のステータス </summary>
    //protected EnemyStatus[] _Enemies = default;

    /// <summary> 各コマンド実行時に、施行する相手 </summary>
    protected List<CharacterStatus> _CommandTargets = new List<CharacterStatus>();

    /// <summary> 実行中のコマンドの流れ </summary>
    protected Coroutine _RunningCommand = default;
    #endregion


    #region プロパティ
    /// <summary> キャラクター名 </summary>
    public string Name { get => _Status.Name; }
    /// <summary> 戦闘時の初期位置 </summary>
    public Vector3 BasePosition { get => _BasePosition; set => _BasePosition = value; }
    /// <summary> キャラクターの頭の位置 </summary>
    public Vector3 HeadPoint { get => _Status.HeadPoint; }
    /// <summary> true : 戦えなくなった </summary>
    public bool IsDefeated { get => _Status.IsDefeated; }
    /// <summary> プレイヤー側のステータス </summary>
    public BattleOperatorForPlayer[] Players { get => _Players; }
    /// <summary> プレイヤー側のうち、この場で行動できる者のステータス </summary>
    public BattleOperatorForPlayer[] ActivePlayers { get => _Players.Where(p => !p.IsDefeated).ToArray(); }
    /// <summary> 敵側のステータス </summary>
    public BattleOperatorForEnemy[] Enemies { get => _Enemies; }
    /// <summary> 敵側のうち、この場で行動できる者のステータス </summary>
    public BattleOperatorForEnemy[] ActiveEnemies { get => _Enemies.Where(p => !p.IsDefeated).ToArray(); }
    #endregion

    /// <summary> ダメージを受けた時のダメージ表示をさせる処理 </summary>
    /// <param name="damage"> 受けたダメージ </param>
    /// <param name="position"> 表示位置 </param>
    protected delegate void VD(int damage, Vector3 position);
    /// <summary> ダメージを受けた時のダメージ表示をさせる処理 </summary>
    protected VD VisualizeDamage;


    protected virtual void Awake()
    {
        _Status = GetComponent<CharacterStatus>();
        _Players = FindObjectsOfType<BattleOperatorForPlayer>();
        _Enemies = FindObjectsOfType<BattleOperatorForEnemy>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //初期位置を原点に
        _BasePosition = transform.position;

        //ダメージ表示処理メソッドを委譲
        GUIVisualizeChangeLife _VisualizeChangeLife = GetComponentInChildren<GUIVisualizeChangeLife>();
        VisualizeDamage = _VisualizeChangeLife.Damage;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_Status.IsMyTurn)
        {
            OperateCommand();
        }
        else
        {
            _RunningCommand = null;
            OperateCounter();
        }
    }

    /// <summary>
    /// 自キャラにダメージを受けさせ、その値を表示する
    /// </summary>
    /// <param name="attack"></param>
    /// <param name="ratio"></param>
    public void GaveDamage(int attack, float ratio)
    {
        //ダメージ計算し、減少
        int damage = _Status.GaveDamage(attack, ratio);
        //ダメージ表示
        VisualizeDamage(damage, transform.position);
    }
    

    

    /// <summary>
    /// 自分のターンの行動を実施
    /// </summary>
    protected abstract void OperateCommand();

    /// <summary>
    /// 他の相手のターンの行動を実施
    /// </summary>
    protected virtual void OperateCounter() { }
}

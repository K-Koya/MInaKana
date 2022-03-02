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
    [SerializeField, Tooltip("行動対象を指す矢印オブジェクト")]
    GameObject _TargetArrow = default;

    /// <summary> true : 行動対象にされている </summary>
    bool _IsTargeted = false;

    /// <summary> 戦闘時の初期位置 </summary>
    protected Vector3 _BasePosition = default;

    /// <summary> 同オブジェクトのキャラクターの能力値 </summary>
    protected CharacterStatus _Status = default;

    /// <summary> プレイヤー側の戦闘用コンポーネント </summary>
    protected BattleOperatorForPlayer[] _Players = default;

    /// <summary> 敵側の戦闘用コンポーネント </summary>
    protected BattleOperatorForEnemy[] _Enemies = default;

    /// <summary> 各コマンド実行時に、施行する相手 </summary>
    protected List<CharacterStatus> _CommandTargets = new List<CharacterStatus>();

    /// <summary> 実行中のコマンドの流れ </summary>
    protected Coroutine _RunningCommand = default;

    /// <summary> 該当キャラクターの当たり判定 </summary>
    protected Collider _Collider = default;
    #endregion

    #region Animator用
    [Header("以下にAnimator中のアニメーションの名前を登録")]
    [SerializeField, Tooltip("キャラクターのモーションを制御するアニメーター")]
    protected Animator _Animator = default;

    [SerializeField, Tooltip("パラメーター名 : やられ")]
    protected string _AnimParamDefeated = "OnDefeat";

    [SerializeField, Tooltip("パラメーター名 : 接地")]
    protected string _AnimParamGrounded = "IsGrounded";

    [SerializeField, Tooltip("アニメーション名 : 待機中")]
    protected string _AnimNameStay = "OnStay";

    [SerializeField, Tooltip("アニメーション名 : 走行")]
    protected string _AnimNameRun = "OnRun";

    [SerializeField, Tooltip("アニメーション名 : 被ダメージ")]
    protected string _AnimNameDamage = "OnDamage";
    #endregion

    #region プロパティ
    /// <summary> true : 行動対象にされている (主に行動対象選択の時に矢印を表示させるために利用) </summary>
    public bool IsTargeted { set => _IsTargeted = value; }
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
        _Collider = GetComponent<Collider>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //初期位置を原点に
        _BasePosition = transform.position;

        //ダメージ表示処理メソッドを委譲
        GUIVisualizeChangeLife _VisualizeChangeLife = GetComponentInChildren<GUIVisualizeChangeLife>();
        VisualizeDamage = _VisualizeChangeLife.Damage;

        //ターゲット矢印を非表示
        _TargetArrow.SetActive(false);

        if (!_Animator) _Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_Status.IsMyTurn)
        {
            OperateCommand();

            //反撃等でやられたらターン終了
            if (_Status.IsDefeated) _Status.IsMyTurn = false;
        }
        else
        {
            _RunningCommand = null;
            OperateCounter();
        }

        //ターゲット矢印の表示処理
        _TargetArrow.SetActive(_IsTargeted);
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

        //ダメージリアクション
        _Animator.Play(_AnimNameDamage);

        //やられたらやられモーションを促す
        if(_Status.IsDefeated) _Animator.SetTrigger(_AnimParamDefeated);
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

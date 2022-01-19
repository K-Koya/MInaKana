using UnityEngine;

/// <summary>
/// 同オブジェクトのキャラクターの戦闘時の操作を受け付ける
/// </summary>
[RequireComponent(typeof(CharacterStatus))]
public abstract class BattleOperator : MonoBehaviour
{
    /// <summary> 戦闘時の初期位置 </summary>
    protected Vector3 _BasePosition = default;

    /// <summary> キャラクターの能力値 </summary>
    protected CharacterStatus _Status = default;

    /// <summary> 実行中のコマンドの流れ </summary>
    protected Coroutine _RunningCommand = default;

    /// <summary> 戦闘時の初期位置 </summary>
    public Vector3 BasePosition { get => _BasePosition; set => _BasePosition = value; }

    protected virtual void Awake()
    {
        _Status = GetComponent<CharacterStatus>();
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
    /// <returns> true : 行動終了 </returns>
    protected abstract void OperateCommand();
}

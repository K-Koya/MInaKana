using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵キャラクターの行動を制御する
/// </summary>
public abstract class BattleOperatorForEnemy : BattleOperator
{
    /// <summary> true : 攻撃中 </summary>
    protected bool _IsAttacking = false;

    /// <summary> true : 反撃された </summary>
    protected bool _IsCounterattacked = false;

    /// <summary> 攻撃の際の威力倍率 </summary>
    protected float _AttackRatio = 1.0f;


    /// <summary> ターゲットを決めるルール </summary>
    public enum TargetSelectRule
    {
        /// <summary> 完全にランダム </summary>
        AtRandom,
        /// <summary> 誰も狙わない </summary>
        Harmless
    }
    [SerializeField, Tooltip("ターゲットを決めるルール")]
    protected TargetSelectRule _HowToSelect = default;



    /// <summary> true : 反撃された 値を受け取った後値がfalseになる </summary>
    protected bool TriggerCounterattacked { get { bool b = _IsCounterattacked; _IsCounterattacked = false; return b; } }


    /// <summary> プレイヤーからのカウンターを受け付ける </summary>
    /// <param name="other">ほかのトリガーコライダー</param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        //攻撃中でないなら即抜ける
        if (!_IsAttacking) return;

        //プレイヤーとの接触
        if (other.gameObject.CompareTag(TagNameManager.I.TagNamePlayer))
        {
            //接触点
            Vector3 hitpos = other.ClosestPoint(transform.position);

            BattleOperatorForPlayer player = other.GetComponent<BattleOperatorForPlayer>();

            //頭上から接触なら踏みつけられた判定
            if ((hitpos - _Status.HeadPoint).y > 0)
            {
                _IsCounterattacked = true;
                GaveDamage(player.AttackStatus, 0.2f);
                player.DoTrample(0.75f);
            }
            //踏まれずに接触
            else
            {
                player.GaveDamage(_Status.Attack, _AttackRatio);
            }
        }
    }
}

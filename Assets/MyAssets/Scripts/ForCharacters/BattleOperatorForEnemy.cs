using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵キャラクターの行動を制御する
/// </summary>
public abstract class BattleOperatorForEnemy : BattleOperator
{
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


    protected override void Start()
    {
        base.Start();

        //敵は基本的に攻撃時にカウンターを受けるためにしか使わない
        _Collider.enabled = false;
    }

    /// <summary> プレイヤーからのカウンターを受け付ける </summary>
    /// <param name="other">ほかのトリガーコライダー</param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        //攻撃中でないなら即抜ける
        if (_RunningCommand == null) return;

        //プレイヤーとの接触
        if (other.gameObject.CompareTag(TagNameManager.I.TagNamePlayer))
        {
            //接触点
            Vector3 hitpos = other.ClosestPoint(transform.position);

            BattleOperatorForPlayer player = other.GetComponent<BattleOperatorForPlayer>();

            //頭上から接触なら踏みつけられた判定
            if ((hitpos - _Status.HeadPoint * 0.9f).y > 0)
            {
                _IsCounterattacked = true;
                GaveDamage(player.AttackStatus, 0.5f);
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

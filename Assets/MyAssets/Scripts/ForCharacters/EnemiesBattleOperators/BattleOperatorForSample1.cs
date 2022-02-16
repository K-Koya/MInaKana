using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary> 
/// サンプル敵1の行動設定コンポーネント
/// </summary>
public class BattleOperatorForSample1 : BattleOperatorForEnemy
{
    // Start is called before the first frame update
    //protected override void Start()
    //{
        
    //}

    //// Update is called once per frame
    //protected override void Update()
    //{
        
    //}

    /// <summary> 行動設定 </summary>
    protected override void OperateCommand()
    {
        if (_RunningCommand == null)
        {
            switch (_HowToSelect)
            {
                case TargetSelectRule.AtRandom:
                    //どちらかをランダムで攻撃
                    _RunningCommand = StartCoroutine(BodyAttack(ActivePlayers[Random.Range(0, ActivePlayers.Length)]));
                    break;
            }
        }
    }

    /// <summary> 体当たり攻撃 </summary>
    /// <param name="target"> 一人の標的 </param>
    IEnumerator BodyAttack(BattleOperator target)
    {
        //ジャンプ回避表示
        GUIPlayersInputNavigation.JumpOrder(0, true, true);

        _IsAttacking = true;
        float waitTime = Random.Range(0.1f, 1f);
        bool isGaveCounter = false;

        //ターゲット正面に移動して一定時間待機
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(target.BasePosition + (target.transform.forward * 3f), 1f).SetEase(Ease.Linear));
        sequence.AppendInterval(waitTime);
        sequence.Play();
        yield return sequence.WaitForCompletion();

        //体当たり攻撃
        _IsCounterattacked = false;
        _AttackRatio = 0.4f;
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(target.BasePosition - (target.transform.forward * 5f), 0.8f).SetEase(Ease.Linear));
        sequence.Play().OnUpdate(() =>
        {
            //カウンターを受けたら、DOTweenを切る
            if (TriggerCounterattacked)
            {
                isGaveCounter = true;
                transform.DOKill();

            }
        });
        yield return sequence.WaitForCompletion();

        //カウンターを受けたか否かで対応した方法で原点再起
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(_BasePosition - (transform.forward * 5f), 0.05f).SetEase(Ease.INTERNAL_Zero));
        sequence.Append(transform.DOMove(_BasePosition, 0.2f).SetEase(Ease.Linear));
        sequence.Play();

        yield return sequence.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);

        //ジャンプ回避非表示
        GUIPlayersInputNavigation.JumpOrder();

        _IsAttacking = false;
        _Status.IsMyTurn = false;
    }
}

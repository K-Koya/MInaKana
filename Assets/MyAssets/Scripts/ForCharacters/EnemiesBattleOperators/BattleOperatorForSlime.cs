using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary> 
/// サンプル敵1の行動設定コンポーネント
/// </summary>
public class BattleOperatorForSlime : BattleOperatorForEnemy
{
    /// <summary> 行動設定 </summary>
    protected override void OperateCommand()
    {
        if (_RunningCommand == null)
        {
            //自分が攻撃対象にできるプレイヤーの数
            int numberOfActivePlayer = ActivePlayers.Length;
            //行動選択のための数値
            int attackKindRatio = (int)(Random.value * 100);

            //攻撃対象の決め方で分岐
            switch (_HowToSelect)
            {
                case TargetSelectRule.AtRandom:

                    switch (numberOfActivePlayer)
                    {
                        //プレイヤーが一人だけ
                        case 1:
                            _RunningCommand = StartCoroutine(BodyAttack(ActivePlayers[0]));
                            break;

                        //プレイヤーが二人
                        case 2:
                            //ランダムで攻撃を選択
                            if (attackKindRatio > 50)
                            {
                                //どちらかをランダムで攻撃
                                _RunningCommand = StartCoroutine(BodyAttack(ActivePlayers[Random.Range(0, 2)]));
                            }
                            else
                            {
                                //どちらかをランダムで攻撃
                                BattleOperator target = ActivePlayers[0];
                                BattleOperator other = ActivePlayers[1];
                                if(Random.Range(0, 2) > 0)
                                {
                                    target = ActivePlayers[1];
                                    other = ActivePlayers[0];
                                }
                                _RunningCommand = StartCoroutine(SwitchingBodyAttack(target, other));
                            }
                            break;
                    }

                    break;
            }
        }
    }

    /// <summary> 体当たり攻撃 </summary>
    /// <param name="target"> 一人の標的 </param>
    IEnumerator BodyAttack(BattleOperator target)
    {
        Debug.Log("BodyAttack");
        //ジャンプ回避表示
        GUIPlayersInputNavigation.JumpOrder(0, true, true);

        //反撃判定のためコライダー起動
        _Collider.enabled = true;

        float waitTime = Random.Range(0.1f, 1f);
        bool isGaveCounter = false;

        //ターゲット正面に移動して一定時間待機
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(target.BasePosition + (target.transform.forward * 3f), 1f).SetEase(Ease.Linear).OnStart(() => _Animator.Play(_AnimNameRun)));
        sequence.AppendInterval(waitTime).OnStart(() => _Animator.Play(_AnimNameStay));
        sequence.Play();
        yield return sequence.WaitForCompletion();

        //体当たり攻撃
        _IsCounterattacked = false;
        _AttackRatio = 0.75f;
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(target.BasePosition - (target.transform.forward * 5f), 0.8f).SetEase(Ease.Linear).OnStart(() => _Animator.Play(_AnimNameRun)));
        sequence.Play().OnUpdate(() =>
        {
            //カウンターを受けたら、DOTweenを切る
            if (_IsCounterattacked)
            {
                isGaveCounter = true;
                sequence.Kill();
                _Collider.enabled = false;
            }
        });
        yield return sequence.WaitForCompletion();

        //ベース位置へ再起 反撃を受けたか否かで分岐
        sequence = DOTween.Sequence();
        if (isGaveCounter)
        {
            sequence.SetDelay(0.5f);
        }
        else
        {
            sequence.Append(transform.DOMove(_BasePosition - (transform.forward * 5f), 0.05f).SetEase(Ease.INTERNAL_Zero));
        }
        sequence.Append(transform.DOMove(_BasePosition, 0.5f).SetEase(Ease.Linear));
        sequence.Play();

        yield return sequence.WaitForCompletion();

        _Animator.Play(_AnimNameStay);

        yield return new WaitForSeconds(0.5f);

        //ジャンプ回避非表示
        GUIPlayersInputNavigation.JumpOrder();

        //反撃判定のためコライダー停止
        _Collider.enabled = false;

        _Status.IsMyTurn = false;
    }

    /// <summary>
    /// こちらに近づいてから対象を選んで体当たり攻撃
    /// </summary>
    /// <param name="target">攻撃対象</param>
    /// <param name="other">非攻撃対象</param>
    /// <returns></returns>
    IEnumerator SwitchingBodyAttack(BattleOperator target, BattleOperator other)
    {
        Debug.Log("SwitchingBodyAttack");

        //ジャンプ回避表示
        GUIPlayersInputNavigation.JumpOrder(0, true, true);

        //反撃判定のためコライダー起動
        _Collider.enabled = true;


        float waitTime = Random.Range(0.1f, 1f);
        bool isGaveCounter = false;

        //中間点を経由して体当たり
        Vector3 wayPoint = Vector3.Lerp(target.BasePosition, other.BasePosition, 0.5f) + target.transform.forward * 2f;
        Vector3 originFoward = transform.forward;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(wayPoint, 1f).SetEase(Ease.Linear).OnStart(() => _Animator.Play(_AnimNameRun)));
        sequence.Append(transform.DOLookAt(target.BasePosition, 0.5f).OnStart(() => _Animator.Play(_AnimNameStay)));
        sequence.Append(transform.DOLookAt(wayPoint + originFoward, 0.5f));
        sequence.Append(transform.DOMove(target.BasePosition, 1f).SetEase(Ease.Linear).OnStart(() => _Animator.Play(_AnimNameRun)));
        sequence.Append(transform.DOMove(target.BasePosition - (target.transform.forward * 5f), 0.8f).SetEase(Ease.Linear));
        sequence.Play().OnUpdate(() => 
        {
            //カウンターを受けたら、DOTweenを切る
            if (_IsCounterattacked)
            {
                isGaveCounter = true;
                sequence.Kill();
                _Collider.enabled = false;
            }
        });
        yield return sequence.WaitForCompletion();

        //ベース位置へ再起 反撃を受けたか否かで分岐
        sequence = DOTween.Sequence();
        if (isGaveCounter)
        {
            sequence.SetDelay(0.5f);
        }
        else
        {
            sequence.Append(transform.DOMove(_BasePosition - (transform.forward * 5f), 0.05f).SetEase(Ease.INTERNAL_Zero));
        }
        sequence.Append(transform.DOMove(_BasePosition, 0.5f).SetEase(Ease.Linear));
        sequence.Play();

        yield return sequence.WaitForCompletion();

        _Animator.Play(_AnimNameStay);

        yield return new WaitForSeconds(0.5f);

        //ジャンプ回避非表示
        GUIPlayersInputNavigation.JumpOrder();

        //反撃判定のためコライダー停止
        _Collider.enabled = false;

        _Status.IsMyTurn = false;
    }
}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// プレイヤーキャラクターの戦闘時の操作
/// </summary>
public class BattleOperatorForPlayer : BattleOperator
{
    /// <summary> 判定表示 </summary>
    private GUIEvaluation _Evaluation = default;

    /// <summary> 今の入力判定 </summary>
    private InputEvaluation _NowEvaluation = InputEvaluation.Initial;

    /// <summary> 入力結果 </summary>
    private InputEvaluation _InputResult = InputEvaluation.Initial;

    protected override void Start()
    {
        base.Start();
        _Evaluation = FindObjectOfType<GUIEvaluation>();
    }

    /// <summary>
    /// コマンド操作を受け付けてアクションを起こす
    /// </summary>
    /// <returns> true : 一連のアクションが終了した </returns>
    protected override void OperateCommand()
    {
        if(_RunningCommand == null)
        {
            EnemyStatus enemy = FindObjectsOfType<EnemyStatus>().FirstOrDefault();
            _RunningCommand = StartCoroutine(JumpAttack(enemy));
        }
    }

    /// <summary>
    /// 入力判定
    /// </summary>
    /// <param name="allTime"> 入力受付総時間 </param>
    /// <param name="excellent"> Excellentの受付時間 </param>
    /// <param name="great"> Greatの受付時間 </param>
    /// <param name="good"> Goodの受付時間 </param>
    /// <param name="ok"> OKの受付時間 </param>
    IEnumerator InputEvaluater(float allTime, float excellent = 0f, float great = 0f, float good = 0f, float ok = 0f)
    {
        if (allTime <= 0f) yield break;

        //Miss判定の継続時間
        float miss = Mathf.Max(allTime - excellent - great - good - ok, 0f);
        //Miss判定
        if (miss > 0f)
        {
            _NowEvaluation = InputEvaluation.Miss;
            yield return new WaitForSeconds(miss);
        }
        //OK判定
        if (ok > 0f)
        {
            _NowEvaluation = InputEvaluation.OK;
            yield return new WaitForSeconds(ok);
        }
        //Good判定
        if (good > 0f)
        {
            _NowEvaluation = InputEvaluation.Good;
            yield return new WaitForSeconds(good);
        }
        //Great判定
        if (great > 0f)
        {
            _NowEvaluation = InputEvaluation.Great;
            yield return new WaitForSeconds(great);
        }
        //Excellent判定
        if (excellent > 0f)
        {
            _NowEvaluation = InputEvaluation.Excellent;
            yield return new WaitForSeconds(excellent);
        }
        //以降、失敗
        _NowEvaluation = InputEvaluation.Miss;
    }

    /// <summary>
    /// ジャンプ攻撃用シーケンス
    /// </summary>
    /// <param name="target">攻撃対象</param>
    IEnumerator JumpAttack(EnemyStatus target)
    {
        //攻撃前に少しのインターバル
        yield return new WaitForSeconds(0.5f);

        //ジャンプ攻撃を仕掛ける際の踏み切る位置を計算
        Vector3 jumpPoint = Vector3.Lerp(target.transform.position, transform.position, 0.5f);

        //入力状態初期化
        _InputResult = InputEvaluation.Initial;

        //助走して敵頭上にジャンプTween開始
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(jumpPoint, 0.5f).SetEase(Ease.Linear));
        sequence.Append(transform.DOJump(target.HeadPoint, 5.0f, 1, 1.0f).SetEase(Ease.Linear));
        sequence.Play().OnUpdate(() => 
        {
            //入力状態が初期値でボタン入力があれば、タイミングの良し悪しを判定し入力状態に反映
            if (InputAssistant.GetDownJump(_Status.Number) && _InputResult == InputEvaluation.Initial) _InputResult = _NowEvaluation;
        });
        yield return StartCoroutine(InputEvaluater(1.5f, 0f, 0f, 0.1f, 0.2f));

        //判定表示
        _Evaluation.DoAnimation(_InputResult, target.HeadPoint);

        //入力判定がGood
        if (_InputResult > InputEvaluation.OK)
        {
            //ダメージ発生
            target.GaveDamage(_Status.Attack, 0.5f);

            //入力状態初期化
            _InputResult = InputEvaluation.Initial;
            //2回目のジャンプで入力を再評価
            sequence = transform.DOJump(target.HeadPoint, 3.0f, 1, 0.5f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                if (InputAssistant.GetDownJump(_Status.Number) && _InputResult == InputEvaluation.Initial) _InputResult = _NowEvaluation;
            });
            yield return StartCoroutine(InputEvaluater(0.5f, 0.1f, 0.2f));

            //判定表示
            _Evaluation.DoAnimation(_InputResult, target.HeadPoint);

            //入力判定がExcellent
            if (_InputResult == InputEvaluation.Excellent)
            {
                //ダメージ発生
                target.GaveDamage(_Status.Attack, 1f);

                sequence = DOTween.Sequence().Append(transform.DOJump(target.transform.position + (Vector3.forward * 5f), 2.0f, 1, 0.5f).SetEase(Ease.Linear));
                sequence.Append(transform.DOMove(_BasePosition + Vector3.back * 3f, 0.05f).SetEase(Ease.INTERNAL_Zero));
                sequence.Append(transform.DOMove(_BasePosition, 0.3f).SetEase(Ease.Linear));
                sequence.Play();
            }
            //入力判定がGreat
            else if (_InputResult == InputEvaluation.Great)
            {
                //ダメージ発生
                target.GaveDamage(_Status.Attack, 0.8f);

                sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 2.0f, 1, 0.5f).SetEase(Ease.Linear));
                sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear));
                sequence.Play();
            }
            //入力判定がMiss
            else
            {
                //ダメージ発生
                target.GaveDamage(_Status.Attack, 0.4f);

                sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 1.5f, 1, 0.5f).SetEase(Ease.Linear));
                sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear));
                sequence.Play();
            }
        }
        //入力判定がOK
        else if(_InputResult == InputEvaluation.OK)
        {
            //ダメージ発生
            target.GaveDamage(_Status.Attack, 0.25f);

            sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 1.5f, 1, 0.5f).SetEase(Ease.Linear));
            sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear));
            sequence.Play();
        }
        //入力判定がMiss
        else
        {
            //ダメージ発生
            target.GaveDamage(_Status.Attack, 0.1f);

            sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 1.0f, 2, 1.0f).SetEase(Ease.OutCubic));
            sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear));
            sequence.Play();
        }

        yield return sequence.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);

        _Status.IsMyTurn = false;
    }
}

/// <summary>
/// コマンド選択最上位
/// </summary>
public enum FirstMenu : byte
{
    /// <summary> 一人で攻撃 </summary>
    Solo,
    /// <summary> 二人で攻撃 </summary>
    Twins,
    /// <summary> アイテム使用 </summary>
    Item,
    /// <summary> 逃げる </summary>
    Run,
    /// <summary> 今は行動しない </summary>
    Pass
}

/// <summary>
/// 入力判定
/// </summary>
public enum InputEvaluation : byte
{
    /// <summary> 初期値 </summary>
    Initial,
    /// <summary> 失敗 </summary>
    Miss,
    /// <summary> OK. </summary>
    OK,
    /// <summary> Good! </summary>
    Good,
    /// <summary> Great!! </summary>
    Great,
    /// <summary> Excellent!!! </summary>
    Excellent
}

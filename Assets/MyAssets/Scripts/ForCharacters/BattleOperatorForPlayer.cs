using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �v���C���[�L�����N�^�[�̐퓬���̑���
/// </summary>
public class BattleOperatorForPlayer : BattleOperator
{
    /// <summary> ����\�� </summary>
    private GUIEvaluation _Evaluation = default;

    /// <summary> ���̓��͔��� </summary>
    private InputEvaluation _NowEvaluation = InputEvaluation.Initial;

    /// <summary> ���͌��� </summary>
    private InputEvaluation _InputResult = InputEvaluation.Initial;

    protected override void Start()
    {
        base.Start();
        _Evaluation = FindObjectOfType<GUIEvaluation>();
    }

    /// <summary>
    /// �R�}���h������󂯕t���ăA�N�V�������N����
    /// </summary>
    /// <returns> true : ��A�̃A�N�V�������I������ </returns>
    protected override void OperateCommand()
    {
        if(_RunningCommand == null)
        {
            EnemyStatus enemy = FindObjectsOfType<EnemyStatus>().FirstOrDefault();
            _RunningCommand = StartCoroutine(JumpAttack(enemy));
        }
    }

    /// <summary>
    /// ���͔���
    /// </summary>
    /// <param name="allTime"> ���͎�t������ </param>
    /// <param name="excellent"> Excellent�̎�t���� </param>
    /// <param name="great"> Great�̎�t���� </param>
    /// <param name="good"> Good�̎�t���� </param>
    /// <param name="ok"> OK�̎�t���� </param>
    IEnumerator InputEvaluater(float allTime, float excellent = 0f, float great = 0f, float good = 0f, float ok = 0f)
    {
        if (allTime <= 0f) yield break;

        //Miss����̌p������
        float miss = Mathf.Max(allTime - excellent - great - good - ok, 0f);
        //Miss����
        if (miss > 0f)
        {
            _NowEvaluation = InputEvaluation.Miss;
            yield return new WaitForSeconds(miss);
        }
        //OK����
        if (ok > 0f)
        {
            _NowEvaluation = InputEvaluation.OK;
            yield return new WaitForSeconds(ok);
        }
        //Good����
        if (good > 0f)
        {
            _NowEvaluation = InputEvaluation.Good;
            yield return new WaitForSeconds(good);
        }
        //Great����
        if (great > 0f)
        {
            _NowEvaluation = InputEvaluation.Great;
            yield return new WaitForSeconds(great);
        }
        //Excellent����
        if (excellent > 0f)
        {
            _NowEvaluation = InputEvaluation.Excellent;
            yield return new WaitForSeconds(excellent);
        }
        //�ȍ~�A���s
        _NowEvaluation = InputEvaluation.Miss;
    }

    /// <summary>
    /// �W�����v�U���p�V�[�P���X
    /// </summary>
    /// <param name="target">�U���Ώ�</param>
    IEnumerator JumpAttack(EnemyStatus target)
    {
        //�U���O�ɏ����̃C���^�[�o��
        yield return new WaitForSeconds(0.5f);

        //�W�����v�U�����d�|����ۂ̓��ݐ؂�ʒu���v�Z
        Vector3 jumpPoint = Vector3.Lerp(target.transform.position, transform.position, 0.5f);

        //���͏�ԏ�����
        _InputResult = InputEvaluation.Initial;

        //�������ēG����ɃW�����vTween�J�n
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(jumpPoint, 0.5f).SetEase(Ease.Linear));
        sequence.Append(transform.DOJump(target.HeadPoint, 5.0f, 1, 1.0f).SetEase(Ease.Linear));
        sequence.Play().OnUpdate(() => 
        {
            //���͏�Ԃ������l�Ń{�^�����͂�����΁A�^�C�~���O�̗ǂ������𔻒肵���͏�Ԃɔ��f
            if (InputAssistant.GetDownJump(_Status.Number) && _InputResult == InputEvaluation.Initial) _InputResult = _NowEvaluation;
        });
        yield return StartCoroutine(InputEvaluater(1.5f, 0f, 0f, 0.1f, 0.2f));

        //����\��
        _Evaluation.DoAnimation(_InputResult, target.HeadPoint);

        //���͔��肪Good
        if (_InputResult > InputEvaluation.OK)
        {
            //�_���[�W����
            target.GaveDamage(_Status.Attack, 0.5f);

            //���͏�ԏ�����
            _InputResult = InputEvaluation.Initial;
            //2��ڂ̃W�����v�œ��͂��ĕ]��
            sequence = transform.DOJump(target.HeadPoint, 3.0f, 1, 0.5f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                if (InputAssistant.GetDownJump(_Status.Number) && _InputResult == InputEvaluation.Initial) _InputResult = _NowEvaluation;
            });
            yield return StartCoroutine(InputEvaluater(0.5f, 0.1f, 0.2f));

            //����\��
            _Evaluation.DoAnimation(_InputResult, target.HeadPoint);

            //���͔��肪Excellent
            if (_InputResult == InputEvaluation.Excellent)
            {
                //�_���[�W����
                target.GaveDamage(_Status.Attack, 1f);

                sequence = DOTween.Sequence().Append(transform.DOJump(target.transform.position + (Vector3.forward * 5f), 2.0f, 1, 0.5f).SetEase(Ease.Linear));
                sequence.Append(transform.DOMove(_BasePosition + Vector3.back * 3f, 0.05f).SetEase(Ease.INTERNAL_Zero));
                sequence.Append(transform.DOMove(_BasePosition, 0.3f).SetEase(Ease.Linear));
                sequence.Play();
            }
            //���͔��肪Great
            else if (_InputResult == InputEvaluation.Great)
            {
                //�_���[�W����
                target.GaveDamage(_Status.Attack, 0.8f);

                sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 2.0f, 1, 0.5f).SetEase(Ease.Linear));
                sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear));
                sequence.Play();
            }
            //���͔��肪Miss
            else
            {
                //�_���[�W����
                target.GaveDamage(_Status.Attack, 0.4f);

                sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 1.5f, 1, 0.5f).SetEase(Ease.Linear));
                sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear));
                sequence.Play();
            }
        }
        //���͔��肪OK
        else if(_InputResult == InputEvaluation.OK)
        {
            //�_���[�W����
            target.GaveDamage(_Status.Attack, 0.25f);

            sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 1.5f, 1, 0.5f).SetEase(Ease.Linear));
            sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear));
            sequence.Play();
        }
        //���͔��肪Miss
        else
        {
            //�_���[�W����
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
/// �R�}���h�I���ŏ��
/// </summary>
public enum FirstMenu : byte
{
    /// <summary> ��l�ōU�� </summary>
    Solo,
    /// <summary> ��l�ōU�� </summary>
    Twins,
    /// <summary> �A�C�e���g�p </summary>
    Item,
    /// <summary> ������ </summary>
    Run,
    /// <summary> ���͍s�����Ȃ� </summary>
    Pass
}

/// <summary>
/// ���͔���
/// </summary>
public enum InputEvaluation : byte
{
    /// <summary> �����l </summary>
    Initial,
    /// <summary> ���s </summary>
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

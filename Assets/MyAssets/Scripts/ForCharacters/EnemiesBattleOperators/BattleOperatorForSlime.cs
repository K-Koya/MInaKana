using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary> 
/// �T���v���G1�̍s���ݒ�R���|�[�l���g
/// </summary>
public class BattleOperatorForSlime : BattleOperatorForEnemy
{
    /// <summary> �s���ݒ� </summary>
    protected override void OperateCommand()
    {
        if (_RunningCommand == null)
        {
            //�������U���Ώۂɂł���v���C���[�̐�
            int numberOfActivePlayer = ActivePlayers.Length;
            //�s���I���̂��߂̐��l
            int attackKindRatio = (int)(Random.value * 100);

            //�U���Ώۂ̌��ߕ��ŕ���
            switch (_HowToSelect)
            {
                case TargetSelectRule.AtRandom:

                    switch (numberOfActivePlayer)
                    {
                        //�v���C���[����l����
                        case 1:
                            _RunningCommand = StartCoroutine(BodyAttack(ActivePlayers[0]));
                            break;

                        //�v���C���[����l
                        case 2:
                            //�����_���ōU����I��
                            if (attackKindRatio > 50)
                            {
                                //�ǂ��炩�������_���ōU��
                                _RunningCommand = StartCoroutine(BodyAttack(ActivePlayers[Random.Range(0, 2)]));
                            }
                            else
                            {
                                //�ǂ��炩�������_���ōU��
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

    /// <summary> �̓�����U�� </summary>
    /// <param name="target"> ��l�̕W�I </param>
    IEnumerator BodyAttack(BattleOperator target)
    {
        Debug.Log("BodyAttack");
        //�W�����v���\��
        GUIPlayersInputNavigation.JumpOrder(0, true, true);

        //��������̂��߃R���C�_�[�N��
        _Collider.enabled = true;

        float waitTime = Random.Range(0.1f, 1f);
        bool isGaveCounter = false;

        //�^�[�Q�b�g���ʂɈړ����Ĉ�莞�ԑҋ@
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(target.BasePosition + (target.transform.forward * 3f), 1f).SetEase(Ease.Linear).OnStart(() => _Animator.Play(_AnimNameRun)));
        sequence.AppendInterval(waitTime).OnStart(() => _Animator.Play(_AnimNameStay));
        sequence.Play();
        yield return sequence.WaitForCompletion();

        //�̓�����U��
        _IsCounterattacked = false;
        _AttackRatio = 0.75f;
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(target.BasePosition - (target.transform.forward * 5f), 0.8f).SetEase(Ease.Linear).OnStart(() => _Animator.Play(_AnimNameRun)));
        sequence.Play().OnUpdate(() =>
        {
            //�J�E���^�[���󂯂���ADOTween��؂�
            if (_IsCounterattacked)
            {
                isGaveCounter = true;
                sequence.Kill();
                _Collider.enabled = false;
            }
        });
        yield return sequence.WaitForCompletion();

        //�x�[�X�ʒu�֍ċN �������󂯂����ۂ��ŕ���
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

        //�W�����v����\��
        GUIPlayersInputNavigation.JumpOrder();

        //��������̂��߃R���C�_�[��~
        _Collider.enabled = false;

        _Status.IsMyTurn = false;
    }

    /// <summary>
    /// ������ɋ߂Â��Ă���Ώۂ�I��ő̓�����U��
    /// </summary>
    /// <param name="target">�U���Ώ�</param>
    /// <param name="other">��U���Ώ�</param>
    /// <returns></returns>
    IEnumerator SwitchingBodyAttack(BattleOperator target, BattleOperator other)
    {
        Debug.Log("SwitchingBodyAttack");

        //�W�����v���\��
        GUIPlayersInputNavigation.JumpOrder(0, true, true);

        //��������̂��߃R���C�_�[�N��
        _Collider.enabled = true;


        float waitTime = Random.Range(0.1f, 1f);
        bool isGaveCounter = false;

        //���ԓ_���o�R���đ̓�����
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
            //�J�E���^�[���󂯂���ADOTween��؂�
            if (_IsCounterattacked)
            {
                isGaveCounter = true;
                sequence.Kill();
                _Collider.enabled = false;
            }
        });
        yield return sequence.WaitForCompletion();

        //�x�[�X�ʒu�֍ċN �������󂯂����ۂ��ŕ���
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

        //�W�����v����\��
        GUIPlayersInputNavigation.JumpOrder();

        //��������̂��߃R���C�_�[��~
        _Collider.enabled = false;

        _Status.IsMyTurn = false;
    }
}

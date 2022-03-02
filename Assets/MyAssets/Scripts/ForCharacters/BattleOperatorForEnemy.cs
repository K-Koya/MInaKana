using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�L�����N�^�[�̍s���𐧌䂷��
/// </summary>
public abstract class BattleOperatorForEnemy : BattleOperator
{
    /// <summary> true : �������ꂽ </summary>
    protected bool _IsCounterattacked = false;

    /// <summary> �U���̍ۂ̈З͔{�� </summary>
    protected float _AttackRatio = 1.0f;

    /// <summary> �^�[�Q�b�g�����߂郋�[�� </summary>
    public enum TargetSelectRule
    {
        /// <summary> ���S�Ƀ����_�� </summary>
        AtRandom,
        /// <summary> �N���_��Ȃ� </summary>
        Harmless
    }
    [SerializeField, Tooltip("�^�[�Q�b�g�����߂郋�[��")]
    protected TargetSelectRule _HowToSelect = default;


    protected override void Start()
    {
        base.Start();

        //�G�͊�{�I�ɍU�����ɃJ�E���^�[���󂯂邽�߂ɂ����g��Ȃ�
        _Collider.enabled = false;
    }

    /// <summary> �v���C���[����̃J�E���^�[���󂯕t���� </summary>
    /// <param name="other">�ق��̃g���K�[�R���C�_�[</param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        //�U�����łȂ��Ȃ瑦������
        if (_RunningCommand == null) return;

        //�v���C���[�Ƃ̐ڐG
        if (other.gameObject.CompareTag(TagNameManager.I.TagNamePlayer))
        {
            //�ڐG�_
            Vector3 hitpos = other.ClosestPoint(transform.position);

            BattleOperatorForPlayer player = other.GetComponent<BattleOperatorForPlayer>();

            //���ォ��ڐG�Ȃ瓥�݂���ꂽ����
            if ((hitpos - _Status.HeadPoint * 0.9f).y > 0)
            {
                _IsCounterattacked = true;
                GaveDamage(player.AttackStatus, 0.5f);
                player.DoTrample(0.75f);
            }
            //���܂ꂸ�ɐڐG
            else
            {
                player.GaveDamage(_Status.Attack, _AttackRatio);
            }
        }
    }
}

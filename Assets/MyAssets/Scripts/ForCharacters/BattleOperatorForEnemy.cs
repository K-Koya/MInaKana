using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�L�����N�^�[�̍s���𐧌䂷��
/// </summary>
public abstract class BattleOperatorForEnemy : BattleOperator
{
    /// <summary> true : �U���� </summary>
    protected bool _IsAttacking = false;

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



    /// <summary> true : �������ꂽ �l���󂯎������l��false�ɂȂ� </summary>
    protected bool TriggerCounterattacked { get { bool b = _IsCounterattacked; _IsCounterattacked = false; return b; } }


    /// <summary> �v���C���[����̃J�E���^�[���󂯕t���� </summary>
    /// <param name="other">�ق��̃g���K�[�R���C�_�[</param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        //�U�����łȂ��Ȃ瑦������
        if (!_IsAttacking) return;

        //�v���C���[�Ƃ̐ڐG
        if (other.gameObject.CompareTag(TagNameManager.I.TagNamePlayer))
        {
            //�ڐG�_
            Vector3 hitpos = other.ClosestPoint(transform.position);

            BattleOperatorForPlayer player = other.GetComponent<BattleOperatorForPlayer>();

            //���ォ��ڐG�Ȃ瓥�݂���ꂽ����
            if ((hitpos - _Status.HeadPoint).y > 0)
            {
                _IsCounterattacked = true;
                GaveDamage(player.AttackStatus, 0.2f);
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

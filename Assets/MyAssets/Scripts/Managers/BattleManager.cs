using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �퓬�̏�
/// </summary>
public enum BattleSituation : byte
{
    /// <summary> �퓬���łȂ� </summary>
    NotOnBattle,
    /// <summary> ���� </summary>
    Introduction,
    /// <summary> �퓬�� </summary>
    OnBattle,
    /// <summary> �퓬�̍��Ԃ̃C�x���g�� </summary>
    Interval,
    /// <summary> �v���C���[���� </summary>
    PlayerWin,
    /// <summary> �v���C���[���� </summary>
    PlayerLose
}

/// <summary>
/// �퓬�𐧌䂷��R���|�[�l���g
/// </summary>
public class BattleManager : MonoBehaviour
{
    #region �����o
    /// <summary> �e�L�����N�^�[��RapidAccumulation�����̒l�ɒB����ƍs���ł��� </summary>
    const short TURN_BORDER = 500;

    /// <summary> �퓬�̏� </summary>
    static BattleSituation _Situation = BattleSituation.OnBattle;

    /// <summary> �e�퓬���̃L�����N�^�[�̃X�e�[�^�X�ւ̃A�N�Z�b�T </summary>
    List<CharacterStatus> _BattleCharacters = default;

    /// <summary> ���݂̍s���҂̃X�e�[�^�X </summary>
    CharacterStatus _TurnOwner = default;
    #endregion

    #region �v���p�e�B
    /// <summary> �e�퓬���̃L�����N�^�[�̂����A�퓬�\�Ȏ҂̃A�N�Z�b�T </summary>
    private List<CharacterStatus> ActiveCharacters { get => _BattleCharacters.Where(bc => !bc.IsDefeated).ToList(); }
    /// <summary> �퓬�̏� </summary>
    public static BattleSituation Situation { get => _Situation; set => _Situation = value; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //�퓬�ɎQ�����̃L�����N�^�[�S���̃X�e�[�^�X���擾
        _BattleCharacters = FindObjectsOfType<CharacterStatus>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        //�퓬���̂ݎ��s
        if (_Situation != BattleSituation.OnBattle) return;

        if (!_TurnOwner || !_TurnOwner.IsMyTurn)
        {
            CheckSettlementToResult();
            TurnInstructer();
        }
    }

    /// <summary>
    /// ���̍s���҂������Ďw������
    /// </summary>
    void TurnInstructer()
    {
        //�q���~�ϒl���Ƀ\�[�g
        _BattleCharacters = _BattleCharacters.OrderByDescending(b => b.RapidAccumulation).ToList();

        //���̃^�[���̍s���҂�ۊ�
        _TurnOwner = ActiveCharacters.Where(b => b.RapidAccumulation >= TURN_BORDER).FirstOrDefault();

        //�N����RapidAccumulation��TURN_BORDER�𒴂���܂ŁA�S���q���l�����Z
        while (!_TurnOwner)
        {
            _BattleCharacters.ForEach(b => b.RapidAccumulation += b.Rapid);
            _TurnOwner = ActiveCharacters.Where(b => b.RapidAccumulation >= TURN_BORDER).FirstOrDefault();
        }

        //���Ȃ��̃^�[���ł�
        _TurnOwner.IsMyTurn = true;
        _TurnOwner.RapidAccumulation -= TURN_BORDER;

        Debug.Log(_TurnOwner.Name + " �̃^�[���ł��B");
    }

    /// <summary>
    /// �������������𔻒肵�A�����Ȃ�퓬�㏈��
    /// </summary>
    void CheckSettlementToResult()
    {
        //�v���C���[����l���c���Ă��Ȃ��ꍇ�Q�[���I�[�o�[
        if (ActiveCharacters.OfType<PlayerStatus>().ToList().Count < 1) _Situation = BattleSituation.PlayerLose;
        //�v���C���[���c���Ă��ēG����̂��c���Ă��Ȃ��ꍇ����
        else if (ActiveCharacters.OfType<EnemyStatus>().ToList().Count < 1) _Situation = BattleSituation.PlayerWin;
    }
}

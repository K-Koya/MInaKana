using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
    static BattleSituation _Situation = BattleSituation.Introduction;

    /// <summary> �e�퓬���̃L�����N�^�[�̃X�e�[�^�X�ւ̃A�N�Z�b�T </summary>
    List<CharacterStatus> _BattleCharacters = default;

    /// <summary> ���݂̍s���҂̃X�e�[�^�X </summary>
    CharacterStatus _TurnOwner = default;

    /// <summary> Timeline�J�b�g�𐧌䂷��R���|�[�l���g </summary>
    PlayableDirector _PD = default;

    [SerializeField, Tooltip("�^�C�����C���J�n���ɁA��A�N�e�B�u������I�u�W�F�N�g")]
    GameObject[] onStartDisableObjects = default;

    [SerializeField, Tooltip("�^�C�����C���J�n���ɁA�A�N�e�B�u������I�u�W�F�N�g")]
    GameObject[] onStartEnableObjects = default;

    [SerializeField, Tooltip("�^�C�����C���I�����ɁA��A�N�e�B�u������I�u�W�F�N�g")]
    GameObject[] onEndDisableObjects = default;

    [SerializeField, Tooltip("�^�C�����C���I�����ɁA�A�N�e�B�u������I�u�W�F�N�g")]
    GameObject[] onEndEnableObjects = default;

    [SerializeField, Tooltip("�퓬�J�n���̃J�b�g")]
    PlayableAsset _CutForIntroduction = default;

    [SerializeField, Tooltip("�퓬�������̃J�b�g")]
    PlayableAsset _CutForWin = default;

    [SerializeField, Tooltip("�퓬�s�k���̃J�b�g")]
    PlayableAsset _CutForLose = default;
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
        _PD = FindObjectOfType<PlayableDirector>();
        //�퓬�ɎQ�����̃L�����N�^�[�S���̃X�e�[�^�X���擾
        _BattleCharacters = FindObjectsOfType<CharacterStatus>().ToList();

        //�J�b�g�Đ��E�I�����Ɏ��s���������\�b�h���`
        _PD.played += OnStart;
        _PD.stopped += OnEnd;

        //�퓬�J�n�J�b�g���Đ�
        _PD.Play(_CutForIntroduction);
    }

    // Update is called once per frame
    void Update()
    {
        //�퓬���̂ݎ��s
        if (_Situation != BattleSituation.OnBattle) return;

        if (!_TurnOwner || !_TurnOwner.IsMyTurn)
        {
            TurnInstructer();
            CheckSettlementToResult();
        }
    }

    /// <summary>
    /// �J�b�g�Đ����Ɏ��s���郁�\�b�h
    /// </summary>
    /// <param name="pd">�Y����PlayableDirector</param>
    void OnStart(PlayableDirector pd)
    {
        if (_PD != pd) return;

        Array.ForEach(onStartDisableObjects, o => o.SetActive(false));
        Array.ForEach(onStartEnableObjects, o => o.SetActive(true));
    }

    /// <summary>
    /// �J�b�g�I�����Ɏ��s���郁�\�b�h
    /// </summary>
    /// <param name="pd">�Y����PlayableDirector</param>
    void OnEnd(PlayableDirector pd)
    {
        if (_PD != pd) return;

        Array.ForEach(onEndDisableObjects, o => o.SetActive(false));
        Array.ForEach(onEndEnableObjects, o => o.SetActive(true));
        if (_Situation == BattleSituation.Introduction) _Situation = BattleSituation.OnBattle;
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
        if (ActiveCharacters.OfType<PlayerStatus>().ToList().Count < 1)
        {
            _TurnOwner = null;
            GUIPlayersInputNavigation.OrderReset();
            _Situation = BattleSituation.PlayerLose;
            _PD.Play(_CutForLose);
        }
        //�v���C���[���c���Ă��ēG����̂��c���Ă��Ȃ��ꍇ����
        else if (ActiveCharacters.OfType<EnemyStatus>().ToList().Count < 1)
        {
            _TurnOwner = null;
            GUIPlayersInputNavigation.OrderReset();
            _Situation = BattleSituation.PlayerWin;
            _PD.Play(_CutForWin);
        }
    }
}

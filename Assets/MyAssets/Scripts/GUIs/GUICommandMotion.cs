using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �v���C���[�̃^�[���̍ۂ̃R�}���h�I�����r�W���A����
/// </summary>
public class GUICommandMotion : MonoBehaviour
{
    /// <summary> �v���C���[�p�̐퓬���̐���R���|�[�l���g </summary>
    BattleOperatorForPlayer _BattleOperator = default;

    /// <summary> �O�̃t���[���őI�����Ă����R�}���h </summary>
    int _SelectedBeforeFrame = 0;

    /// <summary> �ŏ��ɏo�Ă��郁�j���[�̊Y�����Ԃɂ�����R�}���h�J�[�h�̈ʒu(1�Ԗڂ̂��̂͐^�񒆂ɁA�ȍ~�����v���) </summary>
    Vector3[] _CommandCardPositions = default;

    /// <summary> �ŏ��ɏo�Ă��郁�j���[�̊Y�����Ԃɂ�����R�}���h�J�[�h�̕`�揇 </summary>
    int[] _CommandCardWriteOrder = default;

    [SerializeField, Tooltip("�ŏ��ɏo�Ă��郁�j���[�̃R�}���h�J�[�h�I�u�W�F�N�g�̈ʒu�B\n 1�Ԗڂ̂��̂͐^�񒆂ɁA�ȍ~�����v���ɃA�T�C������")]
    RectTransform[] _FirstMenuTransforms = default;

    [SerializeField, Tooltip("��Ԗڂɏo�Ă���R�}���h���X�g���j���[�ŁA\n������\��������R���|�[�l���g�Q���A�T�C������")]
    GUIShowCommandName[] _SecondMenuTexts = default;


    // Start is called before the first frame update
    void Start()
    {
        _BattleOperator = GetComponentInParent<BattleOperatorForPlayer>();

        //�J�[�h�̏����ʒu���A���ׂ���W�ʒu��ۊ�
        _CommandCardPositions = _FirstMenuTransforms.Select(cc => cc.localPosition).ToArray();
        _CommandCardWriteOrder = _FirstMenuTransforms.Select(cc => cc.GetSiblingIndex()).ToArray();

        //�R�}���h�J�[�h���A�N�e�B�u��
        Array.ForEach(_FirstMenuTransforms, f => f.gameObject.SetActive(false));
        //�R�}���h���X�g���A�N�e�B�u��
        Array.ForEach(_SecondMenuTexts, f => f.gameObject.SetActive(false));
    }

    // Update is called once per frame
    void Update()
    {
        //�����̃^�[���łȂ�
        if (!_BattleOperator.IsMyTurn) return;
        //�R�}���h���s���ł���
        if (_BattleOperator.IsRunningCommand)
        {
            //�R�}���h�J�[�h���A�N�e�B�u��
            Array.ForEach(_FirstMenuTransforms, f => f.gameObject.SetActive(false));
            //�R�}���h���X�g���A�N�e�B�u��
            Array.ForEach(_SecondMenuTexts, f => f.gameObject.SetActive(false));
            return;
        }

        if(_BattleOperator.SecondMenu == null)
        {
            //�R�}���h�J�[�h���A�N�e�B�u��
            Array.ForEach(_FirstMenuTransforms, f => f.gameObject.SetActive(true));
            //�R�}���h���X�g���A�N�e�B�u��
            Array.ForEach(_SecondMenuTexts, f => f.gameObject.SetActive(false));

            //�R�}���h���ύX����Ă���΁A�J�[�h���ړ�������
            if ((int)_BattleOperator.FirstMenu != _SelectedBeforeFrame)
            {
                CardMove();
                _SelectedBeforeFrame = (int)_BattleOperator.FirstMenu;
            }
        }
        else
        {
            //�R�}���h�J�[�h���A�N�e�B�u��
            Array.ForEach(_FirstMenuTransforms, f => f.gameObject.SetActive(false));
            //�R�}���h���X�g���A�N�e�B�u��
            Array.ForEach(_SecondMenuTexts, f => f.gameObject.SetActive(true));

            //�R�}���h�����肵�Ă��Ȃ�
            if (_BattleOperator.Candidate == null)
            {
                //�R�}���h���ύX����Ă���΁A���X�g�\�����X�V
                if (_BattleOperator.SecondMenuIndex + 100 != _SelectedBeforeFrame)
                {
                    CommandListViewer();
                    _SelectedBeforeFrame = _BattleOperator.SecondMenuIndex + 100;
                }
            }
            //�R�}���h�����肵�āA�^�[�Q�b�g��I��
            else
            {
                //�^�[�Q�b�g���ύX����Ă���΁A���X�g�\�����X�V
                if (_BattleOperator.TargetIndex + 1000 != _SelectedBeforeFrame)
                {
                    TargetListViewer();
                    _SelectedBeforeFrame = _BattleOperator.TargetIndex + 1000;
                }
            }
        }

    }

    /// <summary> �I�𒆂̃R�}���h�J�[�h������ɗ���悤�Ɋe�J�[�h���ړ������� </summary>
    void CardMove()
    {
        //�������Vecter3�z���index�����炷��
        int offset = (int)_BattleOperator.FirstMenu;

        //Tween�������Ȃ�����
        int numberOfCommand = _CommandCardPositions.Length;
        for (int i = 0; i < numberOfCommand; i++)
        {
            int index = (i + offset) % numberOfCommand;
            _FirstMenuTransforms[i].DOKill();
            _FirstMenuTransforms[i].DOLocalMove(_CommandCardPositions[index], 0.1f).SetEase(Ease.Linear);
            _FirstMenuTransforms[i].SetSiblingIndex(_CommandCardWriteOrder[index]);
        }
    }

    /// <summary> �Y�������ނ̃R�}���h�ꗗ�𑀍삷�� </summary>
    void CommandListViewer()
    {
        //���X�g�������擾
        int numberOfCommand = _BattleOperator.SecondMenu.Count;

        //�I���R�}���h���^�񒆂ɗ���悤�Ƀ��X�g�ɔ��f
        int halfLength = _SecondMenuTexts.Length / 2;
        for (int i = 0; i < _SecondMenuTexts.Length; i++)
        {
            //���X�g���̃e�L�X�g�ɕ\���AOutOfRange����ꍇ�͋�
            int index = _BattleOperator.SecondMenuIndex + i - halfLength;
            if (index < 0 || numberOfCommand - 1 < index)
            {
                _SecondMenuTexts[i].Show("-", "-");
            }
            else
            {
                if (index > 0)
                {
                    //�P�ʎw��
                    string unit = "SP:";
                    if (_BattleOperator.FirstMenu == FirstMenu.Item) unit = " �~";
                    CommandBase cb = _BattleOperator.SecondMenu[index];
                    _SecondMenuTexts[i].Show(cb.Name, unit + cb.Value);
                }
                //�߂�R�}���h
                else _SecondMenuTexts[i].Show("Back", "");
            }
        }
    }

    /// <summary> �^�[�Q�b�g�ꗗ�𑀍삷�� </summary>
    void TargetListViewer()
    {
        //�^�[�Q�b�g���X�g���擾
        int numberOfTarget = 0;
        CharacterStatus[] targetStatus = default;
        switch (_BattleOperator.Candidate.Target)
        {
            case TargetType.OneEnemy:
            case TargetType.OneByOneEnemies:
                targetStatus = _BattleOperator.Enemies;
                numberOfTarget = targetStatus.Length + 1;
                break;
            case TargetType.AllEnemies:
                targetStatus = _BattleOperator.Enemies;
                numberOfTarget = 2;
                break;
            case TargetType.Allies:
                targetStatus = _BattleOperator.Players;
                numberOfTarget = targetStatus.Length + 1;
                break;
            default: break;
        }

        //�I���R�}���h���^�񒆂ɗ���悤�Ƀ��X�g�ɔ��f
        int halfLength = _SecondMenuTexts.Length / 2;
        for (int i = 0; i < _SecondMenuTexts.Length; i++)
        {
            //���X�g���̃e�L�X�g�ɕ\���AOutOfRange����ꍇ�͋�
            int index = _BattleOperator.TargetIndex + i - halfLength;
            if (index < 0 || numberOfTarget - 1 < index)
            {
                _SecondMenuTexts[i].Show("-", "");
            }
            else
            {
                if (index > 0) _SecondMenuTexts[i].Show(targetStatus[index - 1].Name, "");
                //�߂�R�}���h
                else _SecondMenuTexts[i].Show("Back", "");
            }
        }
    }
}

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/// <summary> �R�}���h�I�����j���[�ŏ�� </summary>
public enum FirstMenu : byte
{
    /// <summary> �\���A�^�b�N�F��l�ōU�� </summary>
    Solo = 0,
    /// <summary> �c�C���Y�A�^�b�N�F��l�ōU�� </summary>
    Twins = 1,
    /// <summary> �A�C�e���g�p </summary>
    Item = 2,
    /// <summary> ���͍s�����Ȃ� </summary>
    Pass = 3,
    /// <summary> ������ </summary>
    Leave = 4
}

/// <summary> ���͔��� </summary>
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

/// <summary> �R�}���h���ʂ̑Ώ� </summary>
public enum TargetType : byte
{
    /// <summary> ���� </summary>
    Own,
    /// <summary> ���� </summary>
    Allies,
    /// <summary> �����S�� </summary>
    AllAllies,
    /// <summary> �G�P�� </summary>
    OneEnemy,
    /// <summary> �G�P�̎��X </summary>
    OneByOneEnemies,
    /// <summary> �G�S�� </summary>
    AllEnemies
}



/// <summary>
/// �v���C���[�L�����N�^�[�̐퓬���̑���
/// </summary>
[RequireComponent(typeof(HumanoidAnimatorAssistant))]
public class BattleOperatorForPlayer : BattleOperator
{
    #region Animator�p
    [SerializeField, Tooltip("�A�j���[�V������ : ���")]
    protected string _AnimNameBackward = "OnBackward";

    [SerializeField, Tooltip("�A�j���[�V������ : �W�����v")]
    protected string _AnimNameJump = "OnJump";

    [SerializeField, Tooltip("�A�j���[�V������ : ���݂��W�����v")]
    protected string _AnimNameStepJump = "OnStepJump";

    [SerializeField, Tooltip("�A�j���[�V������ : ����W�����v")]
    protected string _AnimNameDodgeJump = "OnDodgeJump";

    [SerializeField, Tooltip("�A�j���[�V������ : ��𓥂݂��W�����v")]
    protected string _AnimNameDodgeStepJump = "OnDodgeStepJum";

    [SerializeField, Tooltip("�p�����[�^�[�� : �R�}���h���s��")]
    protected string _AnimParamCommandRunning = "OnCommandRunning";

    [SerializeField, Tooltip("�p�����[�^�[�� : �퓬�v�l��")]
    protected string _AnimParamThinking = "OnThinking";

    [SerializeField, Tooltip("�p�����[�^�[�� : �퓬�R�}���h����")]
    protected string _AnimParamCommandCorrect = "OnCommandCorrect";

    [SerializeField, Tooltip("�p�����[�^�[�� : ���݂����s")]
    protected string _AnimParamTrampleFailed = "TrampleFailed";

    [SerializeField, Tooltip("�p�����[�^�[�� : ���݂����s��R�P��")]
    protected string _AnimParamAfterTrampleFailed = "OnAfterTrampleFailed";
    #endregion

    #region �����o�[�ϐ�
    /// <summary>true : �����̃^�[�������čŏ��̃t���[�� </summary>
    bool _IsInitMyTurn = true;

    [Header("�h���N���X�p�����[�^�[")]
    [SerializeField, Tooltip("Humanoid���f���p�A�j���[�^�[�⏕�R���|�[�l���g")]
    HumanoidAnimatorAssistant _HumAnimator = default;

    [SerializeField, Tooltip("�ŏ�ʃR�}���h�J�[�h���j���[�őI�𒆂̂���")]
    FirstMenu _FirstMenu = FirstMenu.Solo;

    [SerializeField, Tooltip("��Ԗڂ̃R�}���h���j���[�̑I�����R�}���h���X�g")]
    List<CommandBase> _SecondMenu = default;

    [SerializeField, Tooltip("��Ԗڂ̃R�}���h���j���[�̑I��ԍ�")]
    byte _SecondMenuIndex = 0;

    [SerializeField, Tooltip("�I�����R�}���h")]
    CommandBase _Candidate = default;

    [SerializeField, Tooltip("�R�}���h�����{����Ώۂ̑I��ԍ�")]
    byte _TargetIndex = 0;

    /// <summary> ����\�� </summary>
    GUIEvaluation _Evaluation = default;

    /// <summary> ���̓��͔��� </summary>
    InputEvaluation _NowEvaluation = InputEvaluation.Initial;

    /// <summary> ���͌��� </summary>
    InputEvaluation _InputResult = InputEvaluation.Initial;

    /// <summary> �\���A�^�b�N(��l�ōU��)�R�}���h�̃��X�g </summary>
    List<CommandBase> _SoloAttacks = new List<CommandBase>();

    /// <summary> �c�C���Y�A�^�b�N(��l�ōU��)�R�}���h�̃��X�g </summary>
    List<CommandBase> _TwinsAttacks = new List<CommandBase>();

    /// <summary> �A�C�e���g�p�R�}���h�̃��X�g </summary>
    List<CommandBase> _Items = new List<CommandBase>();

    /// <summary> ������R�}���h </summary>
    CommandBase _LeaveCommand = default;

    /// <summary> ���͍s�������摗�肷��R�}���h�̃��X�g </summary>
    List<CommandBase> _PassCommands = new List<CommandBase>();

    /// <summary> �L�����N�^�[��Rigidbody </summary>
    Rigidbody _RB = default;
    #endregion

    #region �v���p�e�B
    /// <summary> �ŏ�ʃR�}���h�J�[�h���j���[�őI�𒆂̂��� </summary>
    public FirstMenu FirstMenu { get => _FirstMenu; }
    /// <summary> ��Ԗڂ̃R�}���h���j���[�̑I�����R�}���h���X�g </summary>
    public List<CommandBase> SecondMenu { get => _SecondMenu; }
    /// <summary> �I�����R�}���h </summary>
    public CommandBase Candidate { get => _Candidate; }
    /// <summary> true : �����̃^�[���ł��� </summary>
    public bool IsMyTurn { get => _Status.IsMyTurn; }
    /// <summary> true : �R�}���h���s���ł��� </summary>
    public bool IsRunningCommand { get => _RunningCommand != null; }
    /// <summary> ��Ԗڂ̃R�}���h���j���[�̑I��ԍ� </summary>
    public int SecondMenuIndex { get => _SecondMenuIndex; }
    /// <summary> �R�}���h�����{����Ώۂ̑I��ԍ� </summary>
    public int TargetIndex { get => _TargetIndex; }
    /// <summary> �L�����N�^�[�̍U���� </summary>
    public int AttackStatus { get => _Status.Attack; }
    #endregion

    protected override void Start()
    {
        base.Start();
        _Evaluation = FindObjectOfType<GUIEvaluation>();

        _IsInitMyTurn = true;

        _RB = GetComponent<Rigidbody>();
        _HumAnimator = GetComponent<HumanoidAnimatorAssistant>();

        #region �e�R�}���h����������Run���\�b�h�̕R�Â�(�������A�擪 index = 0 �� Back�R�}���h�p��null���w��)

        /* �\���A�^�b�N */
        _SoloAttacks.Add(null);
        _SoloAttacks.Add(new Command_Attack_Jump(JumpAttack));

        /* �c�C���Y�A�^�b�N */
        _TwinsAttacks.Add(null);

        /* �A�C�e�� */
        _Items.Add(null);

        /* �l�q�� */
        _PassCommands.Add(null);
        _PassCommands.Add(new Command_Pass_Abandon(PassAbandon));

        #endregion
    }

    protected override void Update()
    {
        base.Update();

        //�ڒn�t���O�ݒ�
        if(_RunningCommand == null)
        {
            _RB.isKinematic = false;
        }
        else
        {
            _RB.isKinematic = true;
        }

        //Animator�ɐڒn�t���O��ݒ�
        _Animator.SetBool(_AnimParamGrounded, (_Status as PlayerStatus).IsGrounded);
        //Animator�ɃR�}���h���s�t���O��ݒ�
        _Animator.SetBool(_AnimParamCommandRunning, _RunningCommand != null);
    }

    /// <summary>
    /// �R�}���h������󂯕t���ăA�N�V�������N����
    /// </summary>
    protected override void OperateCommand()
    {
        //�R�}���h���m�肵�Ď��s���łȂ�
        if (_RunningCommand == null)
        {
            //�����̃^�[�������߂ĖK�ꂽ�ꍇ�Ɏ��s
            if(_IsInitMyTurn)
            {
                //�ҋ@�����[�V����
                _HumAnimator.PlaySmooth(_AnimParamThinking);

                //������̓i�r
                GUIPlayersInputNavigation.CorrectOrder(_Status.Number, true);
                GUIPlayersInputNavigation.CursorHorizontalOrder(_Status.Number, true);
                _IsInitMyTurn = false;
            }

            //FirstMenu��I��
            if (_SecondMenu == null)
            {
                //�I��
                int enu = (int)_FirstMenu;
                if (InputAssistant.GetDownRight(_Status.Number)) enu += 1;
                else if (InputAssistant.GetDownLeft(_Status.Number)) enu -= 1;
                _FirstMenu = (FirstMenu)Mathf.Repeat(enu, 5);

                //�I���m��
                if (InputAssistant.GetDownJump(_Status.Number))
                {
                    _SecondMenuIndex = 0;

                    //�i�r�Q�[�V�����N��
                    GUIPlayersInputNavigation.CursorVerticalOrder(_Status.Number, true);
                    GUIPlayersInputNavigation.BackOrder(_Status.Number, true);

                    //�R�}���h����
                    _HumAnimator.PlaySmooth(_AnimParamCommandCorrect);

                    switch (_FirstMenu)
                    {
                        case FirstMenu.Solo:
                            _SecondMenu = _SoloAttacks;
                            _SecondMenu.ForEach(sm => { if(sm != null) sm.SetUsable(true, _Status.SPCurrent); });
                            break;
                        case FirstMenu.Twins:
                            _SecondMenu = _TwinsAttacks;
                            _SecondMenu.ForEach(sm => { if (sm != null) sm.SetUsable(true, _Status.SPCurrent); });
                            break;
                        case FirstMenu.Item:
                            _SecondMenu = _Items;
                            break;
                        case FirstMenu.Pass:
                            _SecondMenu = _PassCommands;
                            _SecondMenu.ForEach(sm => { if (sm != null) sm.SetUsable(true, _Status.SPCurrent); });
                            break;
                        case FirstMenu.Leave:
                            _SecondMenu = new List<CommandBase>() { _LeaveCommand };
                            break;
                        default: break;
                    }
                }
            }
            //_SecondMenu��I��
            else if (_Candidate == null)
            {
                //�I��
                if (InputAssistant.GetDownDown(_Status.Number)) _SecondMenuIndex += 1;
                else if (InputAssistant.GetDownUp(_Status.Number)) _SecondMenuIndex -= 1;
                _SecondMenuIndex = (byte)Mathf.Repeat(_SecondMenuIndex, _SecondMenu.Count);

                //�I���m��
                if (InputAssistant.GetDownJump(_Status.Number))
                {
                    //�L�����Z������
                    if (_SecondMenuIndex == 0)
                    {
                        _Candidate = null;
                        _SecondMenu = null;

                        //�i�r�Q�[�V������Ԃ��P�O�ɖ߂�
                        GUIPlayersInputNavigation.CursorHorizontalOrder(_Status.Number, true);
                        GUIPlayersInputNavigation.BackOrder();
                    }
                    else
                    {
                        //�R�}���h����
                        _HumAnimator.PlaySmooth(_AnimParamCommandCorrect);

                        _Candidate = _SecondMenu[_SecondMenuIndex];
                        //�g�p�s�R�}���h�܂���SP�s���Ȃ疳������
                        if (!_Candidate.IsUsable)
                        {
                            _Candidate = null;
                        }
                    }
                }
                //�o�b�N(�L�����Z��)����
                if (InputAssistant.GetDownAttack(_Status.Number))
                {
                    _Candidate = null;
                    _SecondMenu = null;

                    //�i�r�Q�[�V������Ԃ��P�O�ɖ߂�
                    GUIPlayersInputNavigation.CursorHorizontalOrder(_Status.Number, true);
                    GUIPlayersInputNavigation.BackOrder();
                }
            }
            //�R�}���h���s�����I��
            else
            {
                //�ő�I��
                int maxIndex = 0;
                switch (_Candidate.Target)
                {
                    case TargetType.OneEnemy:
                    case TargetType.OneByOneEnemies:
                        maxIndex = _Enemies.Length + 1;
                        break;
                    case TargetType.AllEnemies:
                    case TargetType.Own:
                    case TargetType.AllAllies:
                        maxIndex = 2;
                        break;
                    case TargetType.Allies:
                        maxIndex =_Players.Length + 1;
                        break;
                    default: break;
                }

                //�I��
                if (InputAssistant.GetDownDown(_Status.Number)) _TargetIndex += 1;
                else if (InputAssistant.GetDownUp(_Status.Number)) _TargetIndex -= 1;
                _TargetIndex = (byte)Mathf.Repeat(_TargetIndex, maxIndex);

                //�I���m��
                if (InputAssistant.GetDownJump(_Status.Number))
                {
                    //�L�����Z������
                    if (_TargetIndex == 0) _Candidate = null;
                    //�^�[�Q�b�g�����߂āA�R�}���h�����s
                    else
                    {
                        //�e����̓i�r����
                        GUIPlayersInputNavigation.CorrectOrder();
                        GUIPlayersInputNavigation.CursorVerticalOrder();
                        GUIPlayersInputNavigation.BackOrder();

                        //SP���� or �A�C�e�����}�C�i�X
                        if (_FirstMenu == FirstMenu.Item) _Candidate.Value -= 1;
                        else _Status.SPCurrent -= _Candidate.Value;

                        //�퓬�̑ҋ@��Ԃ�
                        _HumAnimator.PlaySmooth(_AnimNameStay);

                        switch (_Candidate.Target)
                        {
                            case TargetType.OneEnemy:
                                _RunningCommand = StartCoroutine(_Candidate.Run(ActiveEnemies[_TargetIndex - 1]));
                                break;
                            case TargetType.AllEnemies:
                                _RunningCommand = StartCoroutine(_Candidate.Run(ActiveEnemies));
                                break;
                            case TargetType.OneByOneEnemies:
                                //�G�X�e�[�^�X�ꗗ�ɑ΂��A�I�������G���擪�ɗ���悤�ɓ���ւ�
                                List<BattleOperatorForEnemy> list = ActiveEnemies.ToList();
                                list.Insert(0, ActiveEnemies[_TargetIndex]);
                                list.RemoveAt(_TargetIndex);
                                _RunningCommand = StartCoroutine(_Candidate.Run(list.ToArray()));
                                break;
                            case TargetType.Own:
                                _RunningCommand = StartCoroutine(_Candidate.Run(this));
                                break;
                            case TargetType.Allies:
                                _RunningCommand = StartCoroutine(_Candidate.Run(_Players[_TargetIndex - 1]));
                                break;
                            case TargetType.AllAllies:
                                _RunningCommand = StartCoroutine(_Candidate.Run(_Players));
                                break;
                            default: break;
                        }

                        //���^�[���̍s���I���I���E���^�[���ɔ�����
                        _IsInitMyTurn = true;
                    }
                }
                //�o�b�N(�L�����Z��)����
                if (InputAssistant.GetDownAttack(_Status.Number))
                {
                    _Candidate = null;
                }
            }
        }
        else
        {
            _SecondMenu = null;
            _SecondMenuIndex = 0;
            _Candidate = null;
            _TargetIndex = 0;
        }
    }

    /// <summary>
    /// ����̍s���ŃJ�E���^�[�s�����󂯕t����
    /// </summary>
    protected override void OperateCounter()
    {
        if (_Status.IsDefeated) return;

        //�W�����v���
        if (InputAssistant.GetDownJump(_Status.Number))
        {
            (_Status as PlayerStatus).DoJump(1f);
            _HumAnimator.PlaySmooth(_AnimNameDodgeJump);
        }
    }

    /// <summary>
    /// �W�����v�U���̃J�E���^�[�������̓��݂��W�����v
    /// </summary>
    public void DoTrample(float powerRatio)
    {
        (_Status as PlayerStatus).DoJump(powerRatio, true);
        _HumAnimator.PlaySmooth(_AnimNameDodgeStepJump);
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


    #region �R�}���h����ɗ��p���郁�\�b�h�Q(CommandBase�h���N���X��Action�ɑ�����ė��p)


    /// <summary>
    /// �W�����v�U���p�V�[�P���X
    /// </summary>
    /// <param name="target">�U���Ώ�</param>
    IEnumerator JumpAttack(params BattleOperator[] targets)
    {
        //�Ώۂ�1�̂ɍi��
        BattleOperator target = targets[0];

        //�W�����v���̓i�r
        GUIPlayersInputNavigation.JumpOrder(_Status.Number, true, true);

        //�U���O�ɏ����̃C���^�[�o��
        yield return new WaitForSeconds(0.5f);

        //�W�����v�U�����d�|����ۂ̓��ݐ؂�ʒu���v�Z
        Vector3 jumpPoint = Vector3.Lerp(target.transform.position, transform.position, 0.5f);

        //���͏�ԏ�����
        _InputResult = InputEvaluation.Initial;

        //�������ēG����ɃW�����vTween�J�n
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(jumpPoint, 0.5f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameRun)));
        sequence.Append(transform.DOJump(target.HeadPoint, 3.0f, 1, 0.7f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameJump)));
        sequence.Play().OnUpdate(() => 
        {
            //���͏�Ԃ������l�Ń{�^�����͂�����΁A�^�C�~���O�̗ǂ������𔻒肵���͏�Ԃɔ��f
            if (InputAssistant.GetDownJump(_Status.Number) && _InputResult == InputEvaluation.Initial) _InputResult = _NowEvaluation;
        });
        yield return StartCoroutine(InputEvaluater(1.2f, 0f, 0f, 0.1f, 0.2f));

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
            sequence = transform.DOJump(target.HeadPoint, 2.0f, 1, 0.5f).SetEase(Ease.Linear).OnStart(() => 
            _HumAnimator.PlaySmooth(_AnimNameStepJump)).OnUpdate(() =>
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

                sequence = DOTween.Sequence().Append(transform.DOJump(target.transform.position + (Vector3.forward * 5f), 2.0f, 1, 0.5f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameJump)));
                sequence.Append(transform.DOMove(_BasePosition + Vector3.back * 3f, 0.05f).SetEase(Ease.INTERNAL_Zero));
                sequence.Append(transform.DOMove(_BasePosition, 0.3f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameRun)));
                sequence.Play();
            }
            //���͔��肪Great
            else if (_InputResult == InputEvaluation.Great)
            {
                //�_���[�W����
                target.GaveDamage(_Status.Attack, 0.8f);

                sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 2.0f, 1, 0.5f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameJump)));
                sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameBackward)));
                sequence.Play();
            }
            //���͔��肪Miss
            else
            {
                //�_���[�W����
                target.GaveDamage(_Status.Attack, 0.4f);

                sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 1.5f, 1, 0.5f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameJump)));
                sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameBackward)));
                sequence.Play();
            }
        }
        //���͔��肪OK
        else if(_InputResult == InputEvaluation.OK)
        {
            //�_���[�W����
            target.GaveDamage(_Status.Attack, 0.25f);

            sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 1.5f, 1, 0.5f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameJump)));
            sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameBackward)));
            sequence.Play();
        }
        //���͔��肪Miss
        else
        {
            //�_���[�W����
            target.GaveDamage(_Status.Attack, 0.1f);

            sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 1.0f, 2, 1.0f).SetEase(Ease.OutCubic).OnStart(() => _HumAnimator.PlaySmooth(_AnimParamTrampleFailed)));
            sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameBackward)));
            sequence.Play();
        }

        yield return sequence.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);

        //�W�����v���̓i�r����
        GUIPlayersInputNavigation.JumpOrder();

        //�ҋ@��Ԃ�
        _HumAnimator.PlaySmooth(_AnimNameStay);

        _Status.IsMyTurn = false;
    }

    /// <summary>
    /// ���������Ƀp�X����V�[�P���X
    /// </summary>
    /// <param name="targets">�Ώ�</param>
    /// <returns></returns>
    IEnumerator PassAbandon(params BattleOperator[] targets)
    {
        yield return new WaitForSeconds(1.0f);
        _Status.IsMyTurn = false;
    }

    #endregion
}



#region �R�}���h�p�N���X
/// <summary> �S�R�}���h���N���X </summary>
public abstract class CommandBase
{
    #region �����o�[
    /// <summary> �R�}���h�� </summary>
    protected string _Name = "Name";

    /// <summary> �R�}���h���ʔ͈� </summary>
    protected TargetType _TargetType = TargetType.OneEnemy;

    /// <summary> MP��g�p�񐔂�\�����l </summary>
    protected short _ConsumeValue = 0;

    /// <summary> �R�}���h���� </summary>
    protected string _Explain = "";

    /// <summary> �l���ς݂ł��� </summary>
    protected bool _IsAcquired = false;

    /// <summary> �g�p�\�ł��� </summary>
    protected bool _IsUsable = false;
    #endregion

    #region �v���p�e�B
    /// <summary> �R�}���h�� </summary>
    public string Name { get => _Name; }
    /// <summary> �R�}���h���ʔ͈� </summary>
    public TargetType Target { get => _TargetType; }
    /// <summary> �A�C�e���p : �A�C�e���c��� </summary>
    public short Value { get => _ConsumeValue; set => _ConsumeValue = value; }
    /// <summary> �R�}���h���� </summary>
    public string Explain { get => _Explain; }
    /// <summary> true : ���x���A�b�v��V�i���I�Ƃ��Ŏ擾���g����悤�ɂȂ��Ă��� </summary>
    public bool IsAcquired { get => _IsAcquired; }
    /// <summary> false : ��Ԉُ�ȂǂŎg�p�s�ɂȂ��Ă��� </summary>
    public bool IsUsable { get => _IsUsable; }
    #endregion

    /// <summary> �R�}���h���s���ɑ��点�铮��̈Ϗ����\�b�h </summary>
    public delegate IEnumerator CommandCorotine(params BattleOperator[] targets);
    /// <summary> �R�}���h���s���ɑ��点�铮��̈Ϗ������o�[(��ʂ�BattleOperatorForPlayer�N���X�ɂĕR�Â�) </summary>
    public CommandCorotine Run;

    /// <summary> �g�p�s�t���O�𒲐߂��� </summary>
    /// <param name="isUsable"> false : �g�p�s�\��Ԉُ�ɂȂ��Ă��� </param>
    /// <param name="CurrentSP"> ���݂�SP�l </param>
    public void SetUsable(bool isUsable, short CurrentSP)
    {
        _IsUsable = isUsable && (Value < CurrentSP);
    }
}


/// <summary> �W�����v���݂��U�� </summary>
public class Command_Attack_Jump : CommandBase
{
    /// <summary> �W�����v���݂��U�� </summary>
    /// <param name="run">�W�����v���݂��U���̓��상�\�b�h</param>
    public Command_Attack_Jump(CommandCorotine run)
    {
        _Name = "�W�����v";
        _TargetType = TargetType.OneEnemy;
        _ConsumeValue = 0;
        _Explain = "�I�������G�P�̂̓���ɃW�����v���ē��݂��I\n���݂���u�ԂɃ{�^���������ƃ_���[�W�A�b�v�I\n�ō��Q�񓥂݂��邼�I";
        _IsAcquired = true;
        _IsUsable = true;
        Run = run;
    }
}

/// <summary> ���̎����̃^�[�����������R�}���h </summary>
public class Command_Pass_Abandon : CommandBase
{
    /// <summary> ���̎����̃^�[�����������R�}���h </summary>
    /// <param name="run"> �^�[���������상�\�b�h </param>
    public Command_Pass_Abandon(CommandCorotine run)
    {
        _Name = "�Ȃɂ����Ȃ�";
        _TargetType = TargetType.Own;
        _ConsumeValue = 0;
        _Explain = "���̃^�[���͉������Ȃ��ŏI��邼";
        _IsAcquired = true;
        _IsUsable = true;
        Run = run;
    }
}


#endregion
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/// <summary> コマンド選択メニュー最上位 </summary>
public enum FirstMenu : byte
{
    /// <summary> ソロアタック：一人で攻撃 </summary>
    Solo = 0,
    /// <summary> ツインズアタック：二人で攻撃 </summary>
    Twins = 1,
    /// <summary> アイテム使用 </summary>
    Item = 2,
    /// <summary> 今は行動しない </summary>
    Pass = 3,
    /// <summary> 逃げる </summary>
    Leave = 4
}

/// <summary> 入力判定 </summary>
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

/// <summary> コマンド効果の対象 </summary>
public enum TargetType : byte
{
    /// <summary> 自分 </summary>
    Own,
    /// <summary> 味方 </summary>
    Allies,
    /// <summary> 味方全体 </summary>
    AllAllies,
    /// <summary> 敵単体 </summary>
    OneEnemy,
    /// <summary> 敵単体次々 </summary>
    OneByOneEnemies,
    /// <summary> 敵全体 </summary>
    AllEnemies
}



/// <summary>
/// プレイヤーキャラクターの戦闘時の操作
/// </summary>
[RequireComponent(typeof(HumanoidAnimatorAssistant))]
public class BattleOperatorForPlayer : BattleOperator
{
    #region Animator用
    [SerializeField, Tooltip("アニメーション名 : 後退")]
    protected string _AnimNameBackward = "OnBackward";

    [SerializeField, Tooltip("アニメーション名 : ジャンプ")]
    protected string _AnimNameJump = "OnJump";

    [SerializeField, Tooltip("アニメーション名 : 踏みつけジャンプ")]
    protected string _AnimNameStepJump = "OnStepJump";

    [SerializeField, Tooltip("アニメーション名 : 回避ジャンプ")]
    protected string _AnimNameDodgeJump = "OnDodgeJump";

    [SerializeField, Tooltip("アニメーション名 : 回避踏みつけジャンプ")]
    protected string _AnimNameDodgeStepJump = "OnDodgeStepJum";

    [SerializeField, Tooltip("パラメーター名 : コマンド実行中")]
    protected string _AnimParamCommandRunning = "OnCommandRunning";

    [SerializeField, Tooltip("パラメーター名 : 戦闘思考中")]
    protected string _AnimParamThinking = "OnThinking";

    [SerializeField, Tooltip("パラメーター名 : 戦闘コマンド決定")]
    protected string _AnimParamCommandCorrect = "OnCommandCorrect";

    [SerializeField, Tooltip("パラメーター名 : 踏みつけ失敗")]
    protected string _AnimParamTrampleFailed = "TrampleFailed";

    [SerializeField, Tooltip("パラメーター名 : 踏みつけ失敗後コケ中")]
    protected string _AnimParamAfterTrampleFailed = "OnAfterTrampleFailed";
    #endregion

    #region メンバー変数
    /// <summary>true : 自分のターンが来て最初のフレーム </summary>
    bool _IsInitMyTurn = true;

    [Header("派生クラスパラメーター")]
    [SerializeField, Tooltip("Humanoidモデル用アニメーター補助コンポーネント")]
    HumanoidAnimatorAssistant _HumAnimator = default;

    [SerializeField, Tooltip("最上位コマンドカードメニューで選択中のもの")]
    FirstMenu _FirstMenu = FirstMenu.Solo;

    [SerializeField, Tooltip("二番目のコマンドメニューの選択候補コマンドリスト")]
    List<CommandBase> _SecondMenu = default;

    [SerializeField, Tooltip("二番目のコマンドメニューの選択番号")]
    byte _SecondMenuIndex = 0;

    [SerializeField, Tooltip("選択候補コマンド")]
    CommandBase _Candidate = default;

    [SerializeField, Tooltip("コマンドを実施する対象の選択番号")]
    byte _TargetIndex = 0;

    /// <summary> 判定表示 </summary>
    GUIEvaluation _Evaluation = default;

    /// <summary> 今の入力判定 </summary>
    InputEvaluation _NowEvaluation = InputEvaluation.Initial;

    /// <summary> 入力結果 </summary>
    InputEvaluation _InputResult = InputEvaluation.Initial;

    /// <summary> ソロアタック(一人で攻撃)コマンドのリスト </summary>
    List<CommandBase> _SoloAttacks = new List<CommandBase>();

    /// <summary> ツインズアタック(二人で攻撃)コマンドのリスト </summary>
    List<CommandBase> _TwinsAttacks = new List<CommandBase>();

    /// <summary> アイテム使用コマンドのリスト </summary>
    List<CommandBase> _Items = new List<CommandBase>();

    /// <summary> 逃げるコマンド </summary>
    CommandBase _LeaveCommand = default;

    /// <summary> 今は行動せず先送りするコマンドのリスト </summary>
    List<CommandBase> _PassCommands = new List<CommandBase>();

    /// <summary> キャラクターのRigidbody </summary>
    Rigidbody _RB = default;
    #endregion

    #region プロパティ
    /// <summary> 最上位コマンドカードメニューで選択中のもの </summary>
    public FirstMenu FirstMenu { get => _FirstMenu; }
    /// <summary> 二番目のコマンドメニューの選択候補コマンドリスト </summary>
    public List<CommandBase> SecondMenu { get => _SecondMenu; }
    /// <summary> 選択候補コマンド </summary>
    public CommandBase Candidate { get => _Candidate; }
    /// <summary> true : 自分のターンである </summary>
    public bool IsMyTurn { get => _Status.IsMyTurn; }
    /// <summary> true : コマンド実行中である </summary>
    public bool IsRunningCommand { get => _RunningCommand != null; }
    /// <summary> 二番目のコマンドメニューの選択番号 </summary>
    public int SecondMenuIndex { get => _SecondMenuIndex; }
    /// <summary> コマンドを実施する対象の選択番号 </summary>
    public int TargetIndex { get => _TargetIndex; }
    /// <summary> キャラクターの攻撃力 </summary>
    public int AttackStatus { get => _Status.Attack; }
    #endregion

    protected override void Start()
    {
        base.Start();
        _Evaluation = FindObjectOfType<GUIEvaluation>();

        _IsInitMyTurn = true;

        _RB = GetComponent<Rigidbody>();
        _HumAnimator = GetComponent<HumanoidAnimatorAssistant>();

        #region 各コマンドを初期化＆Runメソッドの紐づけ(ただし、先頭 index = 0 は Backコマンド用にnullを指定)

        /* ソロアタック */
        _SoloAttacks.Add(null);
        _SoloAttacks.Add(new Command_Attack_Jump(JumpAttack));

        /* ツインズアタック */
        _TwinsAttacks.Add(null);

        /* アイテム */
        _Items.Add(null);

        /* 様子見 */
        _PassCommands.Add(null);
        _PassCommands.Add(new Command_Pass_Abandon(PassAbandon));

        #endregion
    }

    protected override void Update()
    {
        base.Update();

        //接地フラグ設定
        if(_RunningCommand == null)
        {
            _RB.isKinematic = false;
        }
        else
        {
            _RB.isKinematic = true;
        }

        //Animatorに接地フラグを設定
        _Animator.SetBool(_AnimParamGrounded, (_Status as PlayerStatus).IsGrounded);
        //Animatorにコマンド実行フラグを設定
        _Animator.SetBool(_AnimParamCommandRunning, _RunningCommand != null);
    }

    /// <summary>
    /// コマンド操作を受け付けてアクションを起こす
    /// </summary>
    protected override void OperateCommand()
    {
        //コマンドを確定して実行中でない
        if (_RunningCommand == null)
        {
            //自分のターンが初めて訪れた場合に実行
            if(_IsInitMyTurn)
            {
                //待機中モーション
                _HumAnimator.PlaySmooth(_AnimParamThinking);

                //決定入力ナビ
                GUIPlayersInputNavigation.CorrectOrder(_Status.Number, true);
                GUIPlayersInputNavigation.CursorHorizontalOrder(_Status.Number, true);
                _IsInitMyTurn = false;
            }

            //FirstMenuを選択中
            if (_SecondMenu == null)
            {
                //選択中
                int enu = (int)_FirstMenu;
                if (InputAssistant.GetDownRight(_Status.Number)) enu += 1;
                else if (InputAssistant.GetDownLeft(_Status.Number)) enu -= 1;
                _FirstMenu = (FirstMenu)Mathf.Repeat(enu, 5);

                //選択確定
                if (InputAssistant.GetDownJump(_Status.Number))
                {
                    _SecondMenuIndex = 0;

                    //ナビゲーション起動
                    GUIPlayersInputNavigation.CursorVerticalOrder(_Status.Number, true);
                    GUIPlayersInputNavigation.BackOrder(_Status.Number, true);

                    //コマンド決定
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
            //_SecondMenuを選択中
            else if (_Candidate == null)
            {
                //選択中
                if (InputAssistant.GetDownDown(_Status.Number)) _SecondMenuIndex += 1;
                else if (InputAssistant.GetDownUp(_Status.Number)) _SecondMenuIndex -= 1;
                _SecondMenuIndex = (byte)Mathf.Repeat(_SecondMenuIndex, _SecondMenu.Count);

                //選択確定
                if (InputAssistant.GetDownJump(_Status.Number))
                {
                    //キャンセル処理
                    if (_SecondMenuIndex == 0)
                    {
                        _Candidate = null;
                        _SecondMenu = null;

                        //ナビゲーション状態を１つ前に戻す
                        GUIPlayersInputNavigation.CursorHorizontalOrder(_Status.Number, true);
                        GUIPlayersInputNavigation.BackOrder();
                    }
                    else
                    {
                        //コマンド決定
                        _HumAnimator.PlaySmooth(_AnimParamCommandCorrect);

                        _Candidate = _SecondMenu[_SecondMenuIndex];
                        //使用不可コマンドまたはSP不足なら無反応に
                        if (!_Candidate.IsUsable)
                        {
                            _Candidate = null;
                        }
                    }
                }
                //バック(キャンセル)処理
                if (InputAssistant.GetDownAttack(_Status.Number))
                {
                    _Candidate = null;
                    _SecondMenu = null;

                    //ナビゲーション状態を１つ前に戻す
                    GUIPlayersInputNavigation.CursorHorizontalOrder(_Status.Number, true);
                    GUIPlayersInputNavigation.BackOrder();
                }
            }
            //コマンド実行相手を選択中
            else
            {
                //最大選択数
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

                //選択中
                if (InputAssistant.GetDownDown(_Status.Number)) _TargetIndex += 1;
                else if (InputAssistant.GetDownUp(_Status.Number)) _TargetIndex -= 1;
                _TargetIndex = (byte)Mathf.Repeat(_TargetIndex, maxIndex);

                //選択確定
                if (InputAssistant.GetDownJump(_Status.Number))
                {
                    //キャンセル処理
                    if (_TargetIndex == 0) _Candidate = null;
                    //ターゲットを決めて、コマンドを実行
                    else
                    {
                        //各種入力ナビ解除
                        GUIPlayersInputNavigation.CorrectOrder();
                        GUIPlayersInputNavigation.CursorVerticalOrder();
                        GUIPlayersInputNavigation.BackOrder();

                        //SP消費 or アイテム数マイナス
                        if (_FirstMenu == FirstMenu.Item) _Candidate.Value -= 1;
                        else _Status.SPCurrent -= _Candidate.Value;

                        //戦闘の待機状態へ
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
                                //敵ステータス一覧に対し、選択した敵が先頭に来るように入れ替え
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

                        //自ターンの行動選択終了・自ターンに備える
                        _IsInitMyTurn = true;
                    }
                }
                //バック(キャンセル)処理
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
    /// 相手の行動でカウンター行動を受け付ける
    /// </summary>
    protected override void OperateCounter()
    {
        if (_Status.IsDefeated) return;

        //ジャンプ回避
        if (InputAssistant.GetDownJump(_Status.Number))
        {
            (_Status as PlayerStatus).DoJump(1f);
            _HumAnimator.PlaySmooth(_AnimNameDodgeJump);
        }
    }

    /// <summary>
    /// ジャンプ攻撃のカウンター成功時の踏みつけジャンプ
    /// </summary>
    public void DoTrample(float powerRatio)
    {
        (_Status as PlayerStatus).DoJump(powerRatio, true);
        _HumAnimator.PlaySmooth(_AnimNameDodgeStepJump);
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


    #region コマンド動作に利用するメソッド群(CommandBase派生クラスのActionに代入して利用)


    /// <summary>
    /// ジャンプ攻撃用シーケンス
    /// </summary>
    /// <param name="target">攻撃対象</param>
    IEnumerator JumpAttack(params BattleOperator[] targets)
    {
        //対象を1体に絞る
        BattleOperator target = targets[0];

        //ジャンプ入力ナビ
        GUIPlayersInputNavigation.JumpOrder(_Status.Number, true, true);

        //攻撃前に少しのインターバル
        yield return new WaitForSeconds(0.5f);

        //ジャンプ攻撃を仕掛ける際の踏み切る位置を計算
        Vector3 jumpPoint = Vector3.Lerp(target.transform.position, transform.position, 0.5f);

        //入力状態初期化
        _InputResult = InputEvaluation.Initial;

        //助走して敵頭上にジャンプTween開始
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(jumpPoint, 0.5f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameRun)));
        sequence.Append(transform.DOJump(target.HeadPoint, 3.0f, 1, 0.7f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameJump)));
        sequence.Play().OnUpdate(() => 
        {
            //入力状態が初期値でボタン入力があれば、タイミングの良し悪しを判定し入力状態に反映
            if (InputAssistant.GetDownJump(_Status.Number) && _InputResult == InputEvaluation.Initial) _InputResult = _NowEvaluation;
        });
        yield return StartCoroutine(InputEvaluater(1.2f, 0f, 0f, 0.1f, 0.2f));

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
            sequence = transform.DOJump(target.HeadPoint, 2.0f, 1, 0.5f).SetEase(Ease.Linear).OnStart(() => 
            _HumAnimator.PlaySmooth(_AnimNameStepJump)).OnUpdate(() =>
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

                sequence = DOTween.Sequence().Append(transform.DOJump(target.transform.position + (Vector3.forward * 5f), 2.0f, 1, 0.5f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameJump)));
                sequence.Append(transform.DOMove(_BasePosition + Vector3.back * 3f, 0.05f).SetEase(Ease.INTERNAL_Zero));
                sequence.Append(transform.DOMove(_BasePosition, 0.3f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameRun)));
                sequence.Play();
            }
            //入力判定がGreat
            else if (_InputResult == InputEvaluation.Great)
            {
                //ダメージ発生
                target.GaveDamage(_Status.Attack, 0.8f);

                sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 2.0f, 1, 0.5f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameJump)));
                sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameBackward)));
                sequence.Play();
            }
            //入力判定がMiss
            else
            {
                //ダメージ発生
                target.GaveDamage(_Status.Attack, 0.4f);

                sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 1.5f, 1, 0.5f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameJump)));
                sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameBackward)));
                sequence.Play();
            }
        }
        //入力判定がOK
        else if(_InputResult == InputEvaluation.OK)
        {
            //ダメージ発生
            target.GaveDamage(_Status.Attack, 0.25f);

            sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 1.5f, 1, 0.5f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameJump)));
            sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameBackward)));
            sequence.Play();
        }
        //入力判定がMiss
        else
        {
            //ダメージ発生
            target.GaveDamage(_Status.Attack, 0.1f);

            sequence = DOTween.Sequence().Append(transform.DOJump(jumpPoint, 1.0f, 2, 1.0f).SetEase(Ease.OutCubic).OnStart(() => _HumAnimator.PlaySmooth(_AnimParamTrampleFailed)));
            sequence.Append(transform.DOMove(_BasePosition, 1f).SetEase(Ease.Linear).OnStart(() => _HumAnimator.PlaySmooth(_AnimNameBackward)));
            sequence.Play();
        }

        yield return sequence.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);

        //ジャンプ入力ナビ解除
        GUIPlayersInputNavigation.JumpOrder();

        //待機状態に
        _HumAnimator.PlaySmooth(_AnimNameStay);

        _Status.IsMyTurn = false;
    }

    /// <summary>
    /// 何もせずにパスするシーケンス
    /// </summary>
    /// <param name="targets">対象</param>
    /// <returns></returns>
    IEnumerator PassAbandon(params BattleOperator[] targets)
    {
        yield return new WaitForSeconds(1.0f);
        _Status.IsMyTurn = false;
    }

    #endregion
}



#region コマンド用クラス
/// <summary> 全コマンド基底クラス </summary>
public abstract class CommandBase
{
    #region メンバー
    /// <summary> コマンド名 </summary>
    protected string _Name = "Name";

    /// <summary> コマンド効果範囲 </summary>
    protected TargetType _TargetType = TargetType.OneEnemy;

    /// <summary> MPや使用回数を表す数値 </summary>
    protected short _ConsumeValue = 0;

    /// <summary> コマンド説明 </summary>
    protected string _Explain = "";

    /// <summary> 獲得済みである </summary>
    protected bool _IsAcquired = false;

    /// <summary> 使用可能である </summary>
    protected bool _IsUsable = false;
    #endregion

    #region プロパティ
    /// <summary> コマンド名 </summary>
    public string Name { get => _Name; }
    /// <summary> コマンド効果範囲 </summary>
    public TargetType Target { get => _TargetType; }
    /// <summary> アイテム用 : アイテム残り個数 </summary>
    public short Value { get => _ConsumeValue; set => _ConsumeValue = value; }
    /// <summary> コマンド説明 </summary>
    public string Explain { get => _Explain; }
    /// <summary> true : レベルアップやシナリオとかで取得し使えるようになっている </summary>
    public bool IsAcquired { get => _IsAcquired; }
    /// <summary> false : 状態異常などで使用不可になっている </summary>
    public bool IsUsable { get => _IsUsable; }
    #endregion

    /// <summary> コマンド実行時に走らせる動作の委譲メソッド </summary>
    public delegate IEnumerator CommandCorotine(params BattleOperator[] targets);
    /// <summary> コマンド実行時に走らせる動作の委譲メンバー(上位のBattleOperatorForPlayerクラスにて紐づけ) </summary>
    public CommandCorotine Run;

    /// <summary> 使用不可フラグを調節する </summary>
    /// <param name="isUsable"> false : 使用不能状態異常になっている </param>
    /// <param name="CurrentSP"> 現在のSP値 </param>
    public void SetUsable(bool isUsable, short CurrentSP)
    {
        _IsUsable = isUsable && (Value < CurrentSP);
    }
}


/// <summary> ジャンプ踏みつけ攻撃 </summary>
public class Command_Attack_Jump : CommandBase
{
    /// <summary> ジャンプ踏みつけ攻撃 </summary>
    /// <param name="run">ジャンプ踏みつけ攻撃の動作メソッド</param>
    public Command_Attack_Jump(CommandCorotine run)
    {
        _Name = "ジャンプ";
        _TargetType = TargetType.OneEnemy;
        _ConsumeValue = 0;
        _Explain = "選択した敵１体の頭上にジャンプして踏みつけ！\n踏みつける瞬間にボタンを押すとダメージアップ！\n最高２回踏みつけるぞ！";
        _IsAcquired = true;
        _IsUsable = true;
        Run = run;
    }
}

/// <summary> 今の自分のターンを放棄するコマンド </summary>
public class Command_Pass_Abandon : CommandBase
{
    /// <summary> 今の自分のターンを放棄するコマンド </summary>
    /// <param name="run"> ターン放棄動作メソッド </param>
    public Command_Pass_Abandon(CommandCorotine run)
    {
        _Name = "なにもしない";
        _TargetType = TargetType.Own;
        _ConsumeValue = 0;
        _Explain = "このターンは何もしないで終わるぞ";
        _IsAcquired = true;
        _IsUsable = true;
        Run = run;
    }
}


#endregion
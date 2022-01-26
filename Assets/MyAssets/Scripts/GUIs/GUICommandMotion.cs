using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// プレイヤーのターンの際のコマンド選択をビジュアル化
/// </summary>
public class GUICommandMotion : MonoBehaviour
{
    /// <summary> プレイヤー用の戦闘時の制御コンポーネント </summary>
    BattleOperatorForPlayer _BattleOperator = default;

    /// <summary> 前のフレームで選択していたコマンド </summary>
    int _SelectedBeforeFrame = 0;

    /// <summary> 最初に出てくるメニューの該当順番におけるコマンドカードの位置(1番目のものは真ん中に、以降反時計回り) </summary>
    Vector3[] _CommandCardPositions = default;

    /// <summary> 最初に出てくるメニューの該当順番におけるコマンドカードの描画順 </summary>
    int[] _CommandCardWriteOrder = default;

    [SerializeField, Tooltip("最初に出てくるメニューのコマンドカードオブジェクトの位置。\n 1番目のものは真ん中に、以降反時計回りにアサインする")]
    RectTransform[] _FirstMenuTransforms = default;

    [SerializeField, Tooltip("二番目に出てくるコマンドリストメニューで、\n文字を表示させるコンポーネント群をアサインする")]
    GUIShowCommandName[] _SecondMenuTexts = default;


    // Start is called before the first frame update
    void Start()
    {
        _BattleOperator = GetComponentInParent<BattleOperatorForPlayer>();

        //カードの初期位置より、並べる座標位置を保管
        _CommandCardPositions = _FirstMenuTransforms.Select(cc => cc.localPosition).ToArray();
        _CommandCardWriteOrder = _FirstMenuTransforms.Select(cc => cc.GetSiblingIndex()).ToArray();

        //コマンドカードを非アクティブ化
        Array.ForEach(_FirstMenuTransforms, f => f.gameObject.SetActive(false));
        //コマンドリストを非アクティブ化
        Array.ForEach(_SecondMenuTexts, f => f.gameObject.SetActive(false));
    }

    // Update is called once per frame
    void Update()
    {
        //自分のターンでない
        if (!_BattleOperator.IsMyTurn) return;
        //コマンド実行中である
        if (_BattleOperator.IsRunningCommand)
        {
            //コマンドカードを非アクティブ化
            Array.ForEach(_FirstMenuTransforms, f => f.gameObject.SetActive(false));
            //コマンドリストを非アクティブ化
            Array.ForEach(_SecondMenuTexts, f => f.gameObject.SetActive(false));
            return;
        }

        if(_BattleOperator.SecondMenu == null)
        {
            //コマンドカードをアクティブ化
            Array.ForEach(_FirstMenuTransforms, f => f.gameObject.SetActive(true));
            //コマンドリストを非アクティブ化
            Array.ForEach(_SecondMenuTexts, f => f.gameObject.SetActive(false));

            //コマンドが変更されていれば、カードを移動させる
            if ((int)_BattleOperator.FirstMenu != _SelectedBeforeFrame)
            {
                CardMove();
                _SelectedBeforeFrame = (int)_BattleOperator.FirstMenu;
            }
        }
        else
        {
            //コマンドカードを非アクティブ化
            Array.ForEach(_FirstMenuTransforms, f => f.gameObject.SetActive(false));
            //コマンドリストをアクティブ化
            Array.ForEach(_SecondMenuTexts, f => f.gameObject.SetActive(true));

            //コマンドを決定していない
            if (_BattleOperator.Candidate == null)
            {
                //コマンドが変更されていれば、リスト表示を更新
                if (_BattleOperator.SecondMenuIndex + 100 != _SelectedBeforeFrame)
                {
                    CommandListViewer();
                    _SelectedBeforeFrame = _BattleOperator.SecondMenuIndex + 100;
                }
            }
            //コマンドを決定して、ターゲットを選択中
            else
            {
                //ターゲットが変更されていれば、リスト表示を更新
                if (_BattleOperator.TargetIndex + 1000 != _SelectedBeforeFrame)
                {
                    TargetListViewer();
                    _SelectedBeforeFrame = _BattleOperator.TargetIndex + 1000;
                }
            }
        }

    }

    /// <summary> 選択中のコマンドカードが頭上に来るように各カードを移動させる </summary>
    void CardMove()
    {
        //代入するVecter3配列のindexをずらす量
        int offset = (int)_BattleOperator.FirstMenu;

        //Tweenをかけながら代入
        int numberOfCommand = _CommandCardPositions.Length;
        for (int i = 0; i < numberOfCommand; i++)
        {
            int index = (i + offset) % numberOfCommand;
            _FirstMenuTransforms[i].DOKill();
            _FirstMenuTransforms[i].DOLocalMove(_CommandCardPositions[index], 0.1f).SetEase(Ease.Linear);
            _FirstMenuTransforms[i].SetSiblingIndex(_CommandCardWriteOrder[index]);
        }
    }

    /// <summary> 該当する種類のコマンド一覧を操作する </summary>
    void CommandListViewer()
    {
        //リスト総数を取得
        int numberOfCommand = _BattleOperator.SecondMenu.Count;

        //選択コマンドが真ん中に来るようにリストに反映
        int halfLength = _SecondMenuTexts.Length / 2;
        for (int i = 0; i < _SecondMenuTexts.Length; i++)
        {
            //リスト中のテキストに表示、OutOfRangeする場合は空白
            int index = _BattleOperator.SecondMenuIndex + i - halfLength;
            if (index < 0 || numberOfCommand - 1 < index)
            {
                _SecondMenuTexts[i].Show("-", "-");
            }
            else
            {
                if (index > 0)
                {
                    //単位指定
                    string unit = "SP:";
                    if (_BattleOperator.FirstMenu == FirstMenu.Item) unit = " ×";
                    CommandBase cb = _BattleOperator.SecondMenu[index];
                    _SecondMenuTexts[i].Show(cb.Name, unit + cb.Value);
                }
                //戻るコマンド
                else _SecondMenuTexts[i].Show("Back", "");
            }
        }
    }

    /// <summary> ターゲット一覧を操作する </summary>
    void TargetListViewer()
    {
        //ターゲットリストを取得
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

        //選択コマンドが真ん中に来るようにリストに反映
        int halfLength = _SecondMenuTexts.Length / 2;
        for (int i = 0; i < _SecondMenuTexts.Length; i++)
        {
            //リスト中のテキストに表示、OutOfRangeする場合は空白
            int index = _BattleOperator.TargetIndex + i - halfLength;
            if (index < 0 || numberOfTarget - 1 < index)
            {
                _SecondMenuTexts[i].Show("-", "");
            }
            else
            {
                if (index > 0) _SecondMenuTexts[i].Show(targetStatus[index - 1].Name, "");
                //戻るコマンド
                else _SecondMenuTexts[i].Show("Back", "");
            }
        }
    }
}

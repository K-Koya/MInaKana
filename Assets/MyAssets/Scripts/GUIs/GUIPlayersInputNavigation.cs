using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーのボタン入力をナビするGUI表示
/// </summary>
public class GUIPlayersInputNavigation : Singleton<GUIPlayersInputNavigation>
{
    #region Animator引数
    [SerializeField, Tooltip("ナビゲートアニメーションのベースレイヤ番号")]
    byte _BaseLayerNumber = 0;

    [SerializeField, Tooltip("ナビゲートアニメーションのアクセント用のレイヤ番号")]
    byte _AccentLayerNumber = 1;

    [SerializeField, Tooltip("移動入力ナビゲートのアニメーション名")]
    string _MoveOrderAnimName = "MoveOrder";

    [SerializeField, Tooltip("上下左右カーソル入力のナビゲートのアニメーション名")]
    string _CursorMultiOrderAnimName = "CursorMultiOrder";

    [SerializeField, Tooltip("左右カーソル入力のナビゲートのアニメーション名")]
    string _CursorHorizontalOrderAnimName = "CursorHorizontalOrder";

    [SerializeField, Tooltip("上下カーソル入力のナビゲートのアニメーション名")]
    string _CursorVerticalOrderAnimName = "CursorVerticalOrder";

    [SerializeField, Tooltip("上下左右カーソル入力のナビゲーを使わない時のアニメーション名")]
    string _NonMoveOrderAnimName = "NonMoveOrder";

    [SerializeField, Tooltip("決定入力ナビゲートのアニメーション名")]
    string _CorrectOrderAnimName = "CorrectOrder";

    [SerializeField, Tooltip("ジャンプ入力ナビゲートのアニメーション名")]
    string _JumpOrderAnimName = "JumpOrder";

    [SerializeField, Tooltip("剣攻撃入力ナビゲートのアニメーション名")]
    string _SwordOrderAnimName = "SwordOrder";

    [SerializeField, Tooltip("鞭攻撃入力ナビゲートのアニメーション名")]
    string _WhipOrderAnimName = "WhipOrder";

    [SerializeField, Tooltip("バック(キャンセル)入力ナビゲートのアニメーション名")]
    string _BackOrderAnimName = "BackOrder";

    [SerializeField, Tooltip("入力ナビゲートを協調表示するアニメーション名")]
    string _DoAccentAnimName = "DoAccent";

    [SerializeField, Tooltip("入力ナビゲートを普通に表示するアニメーション名")]
    string _NonAccentAnimName = "NonAccent";
    #endregion

    #region Animator本体
    [SerializeField, Tooltip("1P用 : 移動入力をナビゲートするGUIのアニメーション")]
    Animator _MoveOrderAnim1 = default;

    [SerializeField, Tooltip("2P用 : 移動入力をナビゲートするGUIのアニメーション")]
    Animator _MoveOrderAnim2 = default;

    [SerializeField, Tooltip("1P用 : 下ボタン入力をナビゲートするGUIのアニメーション")]
    Animator _JumpOrderAnim1 = default;

    [SerializeField, Tooltip("2P用 : 下ボタン入力をナビゲートするGUIのアニメーション")]
    Animator _JumpOrderAnim2 = default;

    [SerializeField, Tooltip("1P用 : トリガー入力をナビゲートするGUIのアニメーション")]
    Animator _JumpOrderAnimTrigger1 = default;

    [SerializeField, Tooltip("2P用 : トリガー入力をナビゲートするGUIのアニメーション")]
    Animator _JumpOrderAnimTrigger2 = default;

    [SerializeField, Tooltip("1P用 : 右ボタン入力をナビゲートするGUIのアニメーション")]
    Animator _AttackOrderAnim1 = default;

    [SerializeField, Tooltip("2P用 : 右ボタン入力をナビゲートするGUIのアニメーション")]
    Animator _AttackOrderAnim2 = default;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _MoveOrderAnim1.Play(I._NonMoveOrderAnimName);
        _MoveOrderAnim2.Play(I._NonMoveOrderAnimName);
        _JumpOrderAnim1.gameObject.SetActive(false);
        _JumpOrderAnim2.gameObject.SetActive(false);
        _JumpOrderAnimTrigger1.gameObject.SetActive(false);
        _JumpOrderAnimTrigger2.gameObject.SetActive(false);
        _AttackOrderAnim1.gameObject.SetActive(false);
        _AttackOrderAnim2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region ナビゲート要求メソッド
    /// <summary> 移動入力ナビゲートを促す </summary>
    /// <param name="playerNumber">プレイヤー番号 0:両方 1:1P向け 2:2P向け</param>
    /// <param name="isForActivate">true : 起動するために false : 消すために</param>
    /// <param name="isDoAccent"> true : 強調表示する </param>
    public static void MoveOrder(int playerNumber = 0, bool isForActivate = false, bool isDoAccent = false)
    {
        switch (playerNumber)
        {
            case 0:
                if (isForActivate)
                {
                    I._MoveOrderAnim1.Play(I._MoveOrderAnimName);
                    I._MoveOrderAnim2.Play(I._MoveOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                        I._MoveOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim1.Play(I._NonMoveOrderAnimName);
                    I._MoveOrderAnim2.Play(I._NonMoveOrderAnimName);
                }
                    break;
            case 1:
                if (isForActivate)
                {
                    I._MoveOrderAnim1.Play(I._MoveOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim1.Play(I._NonMoveOrderAnimName);
                }
                break;
            case 2:
                if (isForActivate)
                {
                    I._MoveOrderAnim2.Play(I._MoveOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim2.Play(I._NonMoveOrderAnimName);
                }
                break;
        }
    }

    /// <summary> 上下左右カーソル入力ナビゲートを促す </summary>
    /// <param name="playerNumber">プレイヤー番号 0:両方 1:1P向け 2:2P向け</param>
    /// <param name="isForActivate">true : 起動するために false : 消すために</param>
    /// <param name="isDoAccent"> true : 強調表示する </param>
    public static void CursorMultiOrder(int playerNumber = 0, bool isForActivate = false, bool isDoAccent = false)
    {
        switch (playerNumber)
        {
            case 0:
                if (isForActivate)
                {
                    I._MoveOrderAnim1.Play(I._CursorMultiOrderAnimName);
                    I._MoveOrderAnim2.Play(I._CursorMultiOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                        I._MoveOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim1.Play(I._NonMoveOrderAnimName);
                    I._MoveOrderAnim2.Play(I._NonMoveOrderAnimName);
                }
                break;
            case 1:
                if (isForActivate)
                {
                    I._MoveOrderAnim1.Play(I._CursorMultiOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim1.Play(I._NonMoveOrderAnimName);
                }
                break;
            case 2:
                if (isForActivate)
                {
                    I._MoveOrderAnim2.Play(I._CursorMultiOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim2.Play(I._NonMoveOrderAnimName);
                }
                break;
        }
    }

    /// <summary> 上下カーソル入力ナビゲートを促す </summary>
    /// <param name="playerNumber">プレイヤー番号 0:両方 1:1P向け 2:2P向け</param>
    /// <param name="isForActivate">true : 起動するために false : 消すために</param>
    /// <param name="isDoAccent"> true : 強調表示する </param>
    public static void CursorVerticalOrder(int playerNumber = 0, bool isForActivate = false, bool isDoAccent = false)
    {
        switch (playerNumber)
        {
            case 0:
                if (isForActivate)
                {
                    I._MoveOrderAnim1.Play(I._CursorVerticalOrderAnimName);
                    I._MoveOrderAnim2.Play(I._CursorVerticalOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                        I._MoveOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim1.Play(I._NonMoveOrderAnimName);
                    I._MoveOrderAnim2.Play(I._NonMoveOrderAnimName);
                }
                break;
            case 1:
                if (isForActivate)
                {
                    I._MoveOrderAnim1.Play(I._CursorVerticalOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim1.Play(I._NonMoveOrderAnimName);
                }
                break;
            case 2:
                if (isForActivate)
                {
                    I._MoveOrderAnim2.Play(I._CursorVerticalOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim2.Play(I._NonMoveOrderAnimName);
                }
                break;
        }
    }

    /// <summary> 左右カーソル入力ナビゲートを促す </summary>
    /// <param name="playerNumber">プレイヤー番号 0:両方 1:1P向け 2:2P向け</param>
    /// <param name="isForActivate">true : 起動するために false : 消すために</param>
    /// <param name="isDoAccent"> true : 強調表示する </param>
    public static void CursorHorizontalOrder(int playerNumber = 0, bool isForActivate = false, bool isDoAccent = false)
    {
        switch (playerNumber)
        {
            case 0:
                if (isForActivate)
                {
                    I._MoveOrderAnim1.Play(I._CursorHorizontalOrderAnimName);
                    I._MoveOrderAnim2.Play(I._CursorHorizontalOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                        I._MoveOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim1.Play(I._NonMoveOrderAnimName);
                    I._MoveOrderAnim2.Play(I._NonMoveOrderAnimName);
                }
                break;
            case 1:
                if (isForActivate)
                {
                    I._MoveOrderAnim1.Play(I._CursorHorizontalOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim1.Play(I._NonMoveOrderAnimName);
                }
                break;
            case 2:
                if (isForActivate)
                {
                    I._MoveOrderAnim2.Play(I._CursorHorizontalOrderAnimName);
                    if (isDoAccent)
                    {
                        I._MoveOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                else
                {
                    I._MoveOrderAnim2.Play(I._NonMoveOrderAnimName);
                }
                break;
        }
    }

    /// <summary> 決定入力ナビゲートを促す </summary>
    /// <param name="playerNumber">プレイヤー番号 0:両方 1:1P向け 2:2P向け</param>
    /// <param name="isForActivate">true : 起動するために false : 消すために</param>
    /// <param name="isDoAccent"> true : 強調表示する </param>
    public static void CorrectOrder(int playerNumber = 0, bool isForActivate = false, bool isDoAccent = false)
    {
        switch (playerNumber)
        {
            case 0:
                I._JumpOrderAnim1.gameObject.SetActive(isForActivate);
                I._JumpOrderAnim2.gameObject.SetActive(isForActivate);
                I._JumpOrderAnimTrigger1.gameObject.SetActive(isForActivate);
                I._JumpOrderAnimTrigger2.gameObject.SetActive(isForActivate);
                if (isForActivate)
                {
                    I._JumpOrderAnim1.Play(I._CorrectOrderAnimName);
                    I._JumpOrderAnim2.Play(I._CorrectOrderAnimName);
                    I._JumpOrderAnimTrigger1.Play(I._CorrectOrderAnimName);
                    I._JumpOrderAnimTrigger2.Play(I._CorrectOrderAnimName);
                    if (isDoAccent)
                    {
                        I._JumpOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                        I._JumpOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                        I._JumpOrderAnimTrigger1.Play(I._DoAccentAnimName, -1, 0f);
                        I._JumpOrderAnimTrigger2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                break;
            case 1:
                I._JumpOrderAnim1.gameObject.SetActive(isForActivate);
                I._JumpOrderAnimTrigger1.gameObject.SetActive(isForActivate);
                if (isForActivate)
                {
                    I._JumpOrderAnim1.Play(I._CorrectOrderAnimName);
                    I._JumpOrderAnimTrigger1.Play(I._CorrectOrderAnimName);
                    if (isDoAccent)
                    {
                        I._JumpOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                        I._JumpOrderAnimTrigger1.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                break;
            case 2:
                I._JumpOrderAnim2.gameObject.SetActive(isForActivate);
                I._JumpOrderAnimTrigger2.gameObject.SetActive(isForActivate);
                if (isForActivate)
                {
                    I._JumpOrderAnim2.Play(I._CorrectOrderAnimName);
                    I._JumpOrderAnimTrigger2.Play(I._CorrectOrderAnimName);
                    if (isDoAccent)
                    {
                        I._JumpOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                        I._JumpOrderAnimTrigger2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                break;
        }
    }

    /// <summary> ジャンプ入力ナビゲートを促す </summary>
    /// <param name="playerNumber">プレイヤー番号 0:両方 1:1P向け 2:2P向け</param>
    /// <param name="isForActivate">true : 起動するために false : 消すために</param>
    /// <param name="isDoAccent"> true : 強調表示する </param>
    public static void JumpOrder(int playerNumber = 0, bool isForActivate = false, bool isDoAccent = false)
    {
        switch (playerNumber)
        {
            case 0:
                I._JumpOrderAnim1.gameObject.SetActive(isForActivate);
                I._JumpOrderAnim2.gameObject.SetActive(isForActivate);
                I._JumpOrderAnimTrigger1.gameObject.SetActive(isForActivate);
                I._JumpOrderAnimTrigger2.gameObject.SetActive(isForActivate);
                if (isForActivate)
                {
                    I._JumpOrderAnim1.Play(I._JumpOrderAnimName);
                    I._JumpOrderAnim2.Play(I._JumpOrderAnimName);
                    I._JumpOrderAnimTrigger1.Play(I._JumpOrderAnimName);
                    I._JumpOrderAnimTrigger2.Play(I._JumpOrderAnimName);
                    if (isDoAccent)
                    {
                        I._JumpOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                        I._JumpOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                        I._JumpOrderAnimTrigger1.Play(I._DoAccentAnimName, -1, 0f);
                        I._JumpOrderAnimTrigger2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                break;
            case 1:
                I._JumpOrderAnim1.gameObject.SetActive(isForActivate);
                I._JumpOrderAnimTrigger1.gameObject.SetActive(isForActivate);
                if (isForActivate)
                {
                    I._JumpOrderAnim1.Play(I._JumpOrderAnimName);
                    I._JumpOrderAnimTrigger1.Play(I._JumpOrderAnimName);
                    if (isDoAccent)
                    {
                        I._JumpOrderAnim1.Play(I._DoAccentAnimName, -1, 0f);
                        I._JumpOrderAnimTrigger1.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                break;
            case 2:
                I._JumpOrderAnim2.gameObject.SetActive(isForActivate);
                I._JumpOrderAnimTrigger2.gameObject.SetActive(isForActivate);
                if (isForActivate)
                {
                    I._JumpOrderAnim2.Play(I._JumpOrderAnimName);
                    I._JumpOrderAnimTrigger2.Play(I._JumpOrderAnimName);
                    if (isDoAccent)
                    {
                        I._JumpOrderAnim2.Play(I._DoAccentAnimName, -1, 0f);
                        I._JumpOrderAnimTrigger2.Play(I._DoAccentAnimName, -1, 0f);
                    }
                }
                break;
        }
    }
    #endregion
}

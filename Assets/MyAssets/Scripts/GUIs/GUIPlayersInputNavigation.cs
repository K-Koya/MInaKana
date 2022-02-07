using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �v���C���[�̃{�^�����͂��i�r����GUI�\��
/// </summary>
public class GUIPlayersInputNavigation : Singleton<GUIPlayersInputNavigation>
{
    #region Animator����
    [SerializeField, Tooltip("�i�r�Q�[�g�A�j���[�V�����̃x�[�X���C���ԍ�")]
    byte _BaseLayerNumber = 0;

    [SerializeField, Tooltip("�i�r�Q�[�g�A�j���[�V�����̃A�N�Z���g�p�̃��C���ԍ�")]
    byte _AccentLayerNumber = 1;

    [SerializeField, Tooltip("�ړ����̓i�r�Q�[�g�̃A�j���[�V������")]
    string _MoveOrderAnimName = "MoveOrder";

    [SerializeField, Tooltip("�㉺���E�J�[�\�����͂̃i�r�Q�[�g�̃A�j���[�V������")]
    string _CursorMultiOrderAnimName = "CursorMultiOrder";

    [SerializeField, Tooltip("���E�J�[�\�����͂̃i�r�Q�[�g�̃A�j���[�V������")]
    string _CursorHorizontalOrderAnimName = "CursorHorizontalOrder";

    [SerializeField, Tooltip("�㉺�J�[�\�����͂̃i�r�Q�[�g�̃A�j���[�V������")]
    string _CursorVerticalOrderAnimName = "CursorVerticalOrder";

    [SerializeField, Tooltip("�㉺���E�J�[�\�����͂̃i�r�Q�[���g��Ȃ����̃A�j���[�V������")]
    string _NonMoveOrderAnimName = "NonMoveOrder";

    [SerializeField, Tooltip("������̓i�r�Q�[�g�̃A�j���[�V������")]
    string _CorrectOrderAnimName = "CorrectOrder";

    [SerializeField, Tooltip("�W�����v���̓i�r�Q�[�g�̃A�j���[�V������")]
    string _JumpOrderAnimName = "JumpOrder";

    [SerializeField, Tooltip("���U�����̓i�r�Q�[�g�̃A�j���[�V������")]
    string _SwordOrderAnimName = "SwordOrder";

    [SerializeField, Tooltip("�ڍU�����̓i�r�Q�[�g�̃A�j���[�V������")]
    string _WhipOrderAnimName = "WhipOrder";

    [SerializeField, Tooltip("�o�b�N(�L�����Z��)���̓i�r�Q�[�g�̃A�j���[�V������")]
    string _BackOrderAnimName = "BackOrder";

    [SerializeField, Tooltip("���̓i�r�Q�[�g�������\������A�j���[�V������")]
    string _DoAccentAnimName = "DoAccent";

    [SerializeField, Tooltip("���̓i�r�Q�[�g�𕁒ʂɕ\������A�j���[�V������")]
    string _NonAccentAnimName = "NonAccent";
    #endregion

    #region Animator�{��
    [SerializeField, Tooltip("1P�p : �ړ����͂��i�r�Q�[�g����GUI�̃A�j���[�V����")]
    Animator _MoveOrderAnim1 = default;

    [SerializeField, Tooltip("2P�p : �ړ����͂��i�r�Q�[�g����GUI�̃A�j���[�V����")]
    Animator _MoveOrderAnim2 = default;

    [SerializeField, Tooltip("1P�p : ���{�^�����͂��i�r�Q�[�g����GUI�̃A�j���[�V����")]
    Animator _JumpOrderAnim1 = default;

    [SerializeField, Tooltip("2P�p : ���{�^�����͂��i�r�Q�[�g����GUI�̃A�j���[�V����")]
    Animator _JumpOrderAnim2 = default;

    [SerializeField, Tooltip("1P�p : �g���K�[���͂��i�r�Q�[�g����GUI�̃A�j���[�V����")]
    Animator _JumpOrderAnimTrigger1 = default;

    [SerializeField, Tooltip("2P�p : �g���K�[���͂��i�r�Q�[�g����GUI�̃A�j���[�V����")]
    Animator _JumpOrderAnimTrigger2 = default;

    [SerializeField, Tooltip("1P�p : �E�{�^�����͂��i�r�Q�[�g����GUI�̃A�j���[�V����")]
    Animator _AttackOrderAnim1 = default;

    [SerializeField, Tooltip("2P�p : �E�{�^�����͂��i�r�Q�[�g����GUI�̃A�j���[�V����")]
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


    #region �i�r�Q�[�g�v�����\�b�h
    /// <summary> �ړ����̓i�r�Q�[�g�𑣂� </summary>
    /// <param name="playerNumber">�v���C���[�ԍ� 0:���� 1:1P���� 2:2P����</param>
    /// <param name="isForActivate">true : �N�����邽�߂� false : �������߂�</param>
    /// <param name="isDoAccent"> true : �����\������ </param>
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

    /// <summary> �㉺���E�J�[�\�����̓i�r�Q�[�g�𑣂� </summary>
    /// <param name="playerNumber">�v���C���[�ԍ� 0:���� 1:1P���� 2:2P����</param>
    /// <param name="isForActivate">true : �N�����邽�߂� false : �������߂�</param>
    /// <param name="isDoAccent"> true : �����\������ </param>
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

    /// <summary> �㉺�J�[�\�����̓i�r�Q�[�g�𑣂� </summary>
    /// <param name="playerNumber">�v���C���[�ԍ� 0:���� 1:1P���� 2:2P����</param>
    /// <param name="isForActivate">true : �N�����邽�߂� false : �������߂�</param>
    /// <param name="isDoAccent"> true : �����\������ </param>
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

    /// <summary> ���E�J�[�\�����̓i�r�Q�[�g�𑣂� </summary>
    /// <param name="playerNumber">�v���C���[�ԍ� 0:���� 1:1P���� 2:2P����</param>
    /// <param name="isForActivate">true : �N�����邽�߂� false : �������߂�</param>
    /// <param name="isDoAccent"> true : �����\������ </param>
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

    /// <summary> ������̓i�r�Q�[�g�𑣂� </summary>
    /// <param name="playerNumber">�v���C���[�ԍ� 0:���� 1:1P���� 2:2P����</param>
    /// <param name="isForActivate">true : �N�����邽�߂� false : �������߂�</param>
    /// <param name="isDoAccent"> true : �����\������ </param>
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

    /// <summary> �W�����v���̓i�r�Q�[�g�𑣂� </summary>
    /// <param name="playerNumber">�v���C���[�ԍ� 0:���� 1:1P���� 2:2P����</param>
    /// <param name="isForActivate">true : �N�����邽�߂� false : �������߂�</param>
    /// <param name="isDoAccent"> true : �����\������ </param>
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

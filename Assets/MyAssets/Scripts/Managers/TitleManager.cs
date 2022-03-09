using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary> �^�C�g����ʂ𐧌䂷��R���|�[�l���g </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary> Timeline�J�b�g�𐧌䂷��R���|�[�l���g </summary>
    PlayableDirector _PD = default;

    /// <summary> BGM�Đ��p��AudioSource </summary>
    AudioSource _BGNSpeaker = default;

    [Header("BGM�pAudioClip�̃A�T�C���v��")]
    [SerializeField, Tooltip("�^�C�g��BGM���A�T�C��")]
    AudioClip _AudioTitle = default;


    [Header("Timeline�p�A�T�C���v��")]
    [SerializeField, Tooltip("�^�C�����C���J�n���ɁA��A�N�e�B�u������I�u�W�F�N�g")]
    GameObject[] onStartDisableObjects = default;

    [SerializeField, Tooltip("�^�C�����C���J�n���ɁA�A�N�e�B�u������I�u�W�F�N�g")]
    GameObject[] onStartEnableObjects = default;

    [SerializeField, Tooltip("�^�C�����C���I�����ɁA��A�N�e�B�u������I�u�W�F�N�g")]
    GameObject[] onEndDisableObjects = default;

    [SerializeField, Tooltip("�^�C�����C���I�����ɁA�A�N�e�B�u������I�u�W�F�N�g")]
    GameObject[] onEndEnableObjects = default;

    [Header("Animator�p�A�T�C���v��")]
    [SerializeField, Tooltip("1P�p�X�^�[�g�{�^���̃A�j���[�^�[")]
    Animator _StartOrderAnim1 = default;

    [SerializeField, Tooltip("2P�p�X�^�[�g�{�^���̃A�j���[�^�[")]
    Animator _StartOrderAnim2 = default;

    [SerializeField, Tooltip("Animation�� : �����v��")]
    string _AnimNameStartPushOrder = "StartPushOrder";

    [SerializeField, Tooltip("Animation�� : �������T�C��")]
    string _AnimNameStartPushNow = "StartPushNow";

    /// <summary>1P�X�^�[�g�p�{�^�����͏��</summary>
    bool _IsPush1P = false;

    /// <summary>2P�X�^�[�g�p�{�^�����͏��</summary>
    bool _IsPush2P = false;

    // Start is called before the first frame update
    void Start()
    {
        _PD = FindObjectOfType<PlayableDirector>();
        _BGNSpeaker = GetComponent<AudioSource>();

        //�J�b�g�Đ��E�I�����Ɏ��s���������\�b�h���`
        _PD.played += OnStart;
        _PD.stopped += OnEnd;

        _BGNSpeaker.clip = _AudioTitle;
        _BGNSpeaker.Play();
    }

    // Update is called once per frame
    void Update()
    {
        bool push1P = InputAssistant.GetJump(1);
        bool push2P = InputAssistant.GetJump(2);

        //���͏�񂪍X�V���ꂽ��A�j���[�V������ύX
        if(_IsPush1P != push1P)
        {
            if (push1P) _StartOrderAnim1.Play(_AnimNameStartPushNow);
            else _StartOrderAnim1.Play(_AnimNameStartPushOrder);
            _IsPush1P = push1P;
        }
        if(_IsPush2P != push2P)
        {
            if (push2P) _StartOrderAnim2.Play(_AnimNameStartPushNow);
            else _StartOrderAnim2.Play(_AnimNameStartPushOrder);
            _IsPush2P = push2P;
        }

        //���͂�����΃f���o�g���V�[���֊ۂ��؂蔲���G�t�F�N�g�������đJ��
        if (push1P && push2P)
        {
            MySceneManager.I.SceneChange(MySceneManager.I.SceneNameGrassField, 1f, LoadSceneEffectType.CircleBlack);
            gameObject.SetActive(false);
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
    }
}

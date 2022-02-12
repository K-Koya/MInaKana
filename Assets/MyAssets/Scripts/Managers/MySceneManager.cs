using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �V�[���̏�Ԃ𐧌䂷��R���|�[�l���g
/// </summary>
public class MySceneManager : Singleton<MySceneManager>
{
    #region �V�[���J�ڋ@�\
    [Header("BuildSetting�œo�^����Scene�����ȉ��ɓo�^����")]
    [SerializeField, Tooltip("Scene�� : �^�C�g��")]
    string _SceneNameTitle = "Title";

    [SerializeField, Tooltip("Scene�� : ���R�G���A")]
    string _SceneNameGrassField = "GrassField";

    /// <summary> Scene�J�ڂł�����G�t�F�N�g�𑀍삷��Animator </summary>
    Animator _EffectAnimator = default;

    [Header("Animator�œo�^�����G�t�F�N�g�pAnimation�����ȉ��ɓo�^����")]
    [SerializeField, Tooltip("Animation�� : �~�`�؂蔲���̈Ó]������")]
    string _AnimNameCircleBlackIning = "CircleBlackIning";

    [SerializeField, Tooltip("Animation�� : �Ó]����~�`�؂蔲�������]����")]
    string _AnimNameCircleBlackOuting = "CircleBlackOuting";
    #endregion

    #region �|�[�Y�����@�\
    /// <summary>
    /// IPauseAndResume��OnPause���`����
    /// </summary>
    public delegate void OnPause();
    /// <summary>
    /// IPauseAndResume��OnResume���`����
    /// </summary>
    public delegate void OnResume();
    #endregion

    #region �v���p�e�B
    /// <summary> Scene�� : �^�C�g�� </summary>
    public string SceneNameTitle { get => _SceneNameTitle; }
    /// <summary> Scene�� : ���R�G���A </summary>
    public string SceneNameGrassField { get => _SceneNameGrassField; }
    #endregion

    protected override void Awake()
    {
        IsDontDestroyOnLoad = true;
        base.Awake();
    }

    void Start()
    {
        _EffectAnimator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// �C�ӂ̃G�t�F�N�g�������Ȃ���Scene�J��
    /// </summary>
    /// <param name="sceneName">�J�ڐ�Scene��</param>
    /// <param name="delay">�J�ڂɗv���鎞��</param>
    /// <param name="type">�G�t�F�N�g�̎��</param>
    public void SceneChange(string sceneName, float delay = 0f, LoadSceneEffectType type = LoadSceneEffectType.None)
    {
        //Scene�J�ڃG�t�F�N�g��v���ʂɎw��
        switch (type)
        {
            case LoadSceneEffectType.CircleBlack:
                SceneManager.sceneLoaded += OnLoadedNext;
                _EffectAnimator.Play(_AnimNameCircleBlackIning);
                break;
            default: break;
        }

        //Scene�J�ڂ�delay���x�����s
        StartCoroutine(DelayLoadScene(sceneName, delay));
    }

    /// <summary> ����Scene���ǂݍ��܂ꂽ���Ɏ��s���郁�\�b�h </summary>
    void OnLoadedNext(Scene scene, LoadSceneMode mode)
    {
        _EffectAnimator.Play(_AnimNameCircleBlackOuting);
    }

    /// <summary>Scene�J�ڃR���[�`��</summary>
    /// <param name="sceneName">�J�ڐ�Scene��</param>
    /// <param name="delay">�x������</param>
    IEnumerator DelayLoadScene(string sceneName, float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        //Scene�J��
        SceneManager.LoadScene(sceneName);
    }
}

/// <summary>
/// Scene���܂����ۂɂ͂��ރG�t�F�N�g�̎��
/// </summary>
public enum LoadSceneEffectType
{
    /// <summary> �������Ȃ� </summary>
    None,
    /// <summary> �~�`�̐؂蔲�������Ȃ���Ó]�t�F�[�h </summary>
    CircleBlack,
}

/// <summary>
/// �|�[�Y�����Ώۂɂ���R���|�[�l���g�Ɍp��������
/// </summary>
public interface IPauseAndResume
{
    /// <summary>
    /// �|�[�Y�ɂȂ����u�ԂɎ��{���鏈��
    /// </summary>
    public void OnPause();

    /// <summary>
    /// �|�[�Y�����Ɠ����Ɏ��{���鏈��
    /// </summary>
    public void OnResume();
}

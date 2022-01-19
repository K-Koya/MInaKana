using System.Linq;
using UnityEngine;

/// <summary>
/// �g�p�����^�O���̕�������Ǘ�
/// </summary>
public class TagNameManager : Singleton<TagNameManager>
{
    [Header("Add Tag�c �Őݒ肵���^�O���̕�������ȉ��ɓo�^����")]

    #region �����o
    [SerializeField, Tooltip("���C���J�����^�O��")]
    private string _TagNameMainCamera = "MainCamera";

    [SerializeField, Tooltip("�擪�v���C���[�^�O��")]
    private string _TagNamePlayer = "Player";

    [SerializeField, Tooltip("����v���C���[�^�O��")]
    private string _TagNameAllies = "Allies";

    [SerializeField, Tooltip("�G�^�O��")]
    private string _TagNameEnemy = "Enemy";

    [SerializeField, Tooltip("�擪�v���C���[�̌��_�ʒu�^�O��")]
    private string _TagNamePlayerBase = "PlayerBase";

    [SerializeField, Tooltip("����v���C���[�̌��_�ʒu�^�O��")]
    private string _TagNameAlliesBase = "AlliesBase";

    [SerializeField, Tooltip("�G�̌��_�ʒu�^�O��")]
    private string _TagNameEnemyBase = "EnemyBase";
    #endregion

    #region �v���p�e�B
    /// <summary> ���C���J�����^�O�� </summary>
    public string TagNameMainCamera { get => _TagNameMainCamera; }
    /// <summary> �擪�v���C���[�^�O�� </summary>
    public string TagNamePlayer { get => _TagNamePlayer; }
    /// <summary> ����v���C���[�^�O�� </summary>
    public string TagNameAllies { get => _TagNameAllies; }
    /// <summary> �G�^�O�� </summary>
    public string TagNameEnemy { get => _TagNameEnemy; }
    /// <summary> �擪�v���C���[�̌��_�ʒu�^�O�� </summary>
    public string TagNamePlayerBase { get => _TagNamePlayerBase; }
    /// <summary> ����v���C���[�̌��_�ʒu�^�O�� </summary>
    public string TagNameAlliesBase { get => _TagNameAlliesBase; }
    /// <summary> �G�v���C���[�̌��_�ʒu�^�O�� </summary>
    public string TagNameEnemyBase { get => _TagNameEnemyBase; }
    #endregion

    protected override void Awake()
    {
        IsDontDestroyOnLoad = true;
        base.Awake();
    }
}

/// <summary>
/// ������tag�Ƃ̈�v��������郁�\�b�h��ǉ�
/// </summary>
static partial class Extensions
{
    /// <summary>
    /// CompareTag�̕�����r�o�[�W����
    /// </summary>
    /// <param name="obj">�Ώۂ̃Q�[���I�u�W�F�N�g</param>
    /// <param name="tags">��r�Ώۃ^�O</param>
    /// <returns>true : �����̃^�O�ƈ�ł���v����</returns>
    public static bool CompareTags(this GameObject obj, string[] tags)
    {
        return tags.Contains(obj.tag);
    }
}
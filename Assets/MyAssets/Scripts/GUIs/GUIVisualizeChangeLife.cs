using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��_���[�W�l��񕜒l��\������G�t�F�N�g
/// </summary>
[RequireComponent(typeof(Sprite)), RequireComponent(typeof(Animator))]
public class GUIVisualizeChangeLife : MonoBehaviour
{
    /// <summary> �_���[�W�𐔒l�\������e�L�X�g </summary>
    Text _ValueText = default;

    /// <summary> ���o�p�A�j���[�^�[ </summary>
    Animator _Animator = default;

    // Start is called before the first frame update
    void Start()
    {
        _ValueText = GetComponentInChildren<Text>();
        _Animator = GetComponent<Animator>();
    }

    /// <summary> �_���[�W���l���ꏊ���w�肵�ĉ��o���o���Ȃ���Đ� </summary>
    /// <param name="damage">�_���[�W���l</param>
    /// <param name="position">�\������ꏊ</param>
    public void Damage(int damage, Vector3 position)
    {
        //���l�w��
        _ValueText.text = damage.ToString();

        //�w��ʒu�ōĐ�
        transform.position = position;
        _Animator.Play("DamagedEffect", 0, 0);
    }
}

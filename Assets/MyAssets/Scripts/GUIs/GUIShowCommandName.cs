using UnityEngine;
using UnityEngine.UI;

public class GUIShowCommandName : MonoBehaviour
{
    [SerializeField, Tooltip("�R�}���h��")]
    Text _Name = default;

    [SerializeField, Tooltip("�R�}���h����SP�������̓A�C�e���c��")]
    Text _Value = default;

    /// <summary> ���X�g��s�ɕ\�� </summary>
    /// <param name="cName"> �R�}���h�� </param>
    /// <param name="value"> �R�}���h����SP�������̓A�C�e���c�� </param>
    public void Show(string cName, string value)
    {
        _Name.text = cName;
        _Value.text = value;
    }
}

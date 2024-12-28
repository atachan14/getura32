using UnityEngine;
using TMPro;

public class NamePlate : MonoBehaviour
{
    public TextMeshProUGUI nameTMP; // TextMeshPro�̎Q��
    public string DisplayName
    {
        get => nameTMP.text;  // ���݂̃e�L�X�g���擾
        set => nameTMP.text = value;  // �e�L�X�g��ݒ�
    }
    public PlayerStatus playerStatus;   // PlayerStatus�̎Q��

    void Start()
    {
        // PlayerStatus���ݒ肳��Ă��Ȃ��ꍇ�̓G���[�`�F�b�N
        if (playerStatus != null && nameTMP != null)
        {
            // PlayerStatus��name��TextMeshPro�ɐݒ�
            nameTMP.text = playerStatus.Name.Value;  // name��\��
            Debug.Log("playerStatus.name");
        }
        else
        {
            Debug.LogWarning("PlayerStatus or playerNameText is not assigned!");
            Debug.Log("playerStatus.name"+ playerStatus.name);
            Debug.Log("nameTMP.text:"+nameTMP.text);
        }
    }

}

using UnityEngine;
using TMPro;

public class NamePlate : MonoBehaviour
{
    public TextMeshPro playerNameText;  // TextMeshPro�̎Q��
    public PlayerStatus playerStatus;   // PlayerStatus�̎Q��

    void Start()
    {
        // PlayerStatus���ݒ肳��Ă��Ȃ��ꍇ�̓G���[�`�F�b�N
        if (playerStatus != null && playerNameText != null)
        {
            // PlayerStatus��name��TextMeshPro�ɐݒ�
            playerNameText.text = playerStatus.name;  // name��\��
        }
        else
        {
            Debug.LogWarning("PlayerStatus or playerNameText is not assigned!");
        }
    }
    void Update()
    {
        if (playerStatus != null && playerNameText != null)
        {
            playerNameText.text = playerStatus.name;  // name���ς�邽�тɍX�V
        }
    }

}

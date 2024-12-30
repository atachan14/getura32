using UnityEngine;
using TMPro;
using Unity.Netcode;

using Unity.Collections;

public class NamePlate : NetworkBehaviour
{
    // FixedString64Bytes ���g�p
    private NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>(
        default(FixedString64Bytes), // �f�t�H���g�l
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public TextMeshProUGUI nameTMP;

    void Start()
    {
        if (IsOwner)
        {
            // ���[�J���v���C���[�̖��O��ݒ�
            string savedName = PlayerPrefs.GetString("PlayerName", "DefaultName");
            SetPlayerNameServerRpc(savedName);
        }
    }

    // ���O��ServerRpc�Őݒ�
    [ServerRpc]
    void SetPlayerNameServerRpc(string newName)
    {
        playerName.Value = newName; // �T�[�o�[�Œl���X�V
    }

    void Update()
    {
        // �l�b�g���[�N�ϐ����X�V���ꂽ��\���𔽉f
        nameTMP.text = playerName.Value.ToString();
    }
}

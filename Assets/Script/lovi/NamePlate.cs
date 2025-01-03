using UnityEngine;
using TMPro;
using Unity.Netcode;

using Unity.Collections;

public class NamePlate : NetworkBehaviour
{
    private NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>(
        default(FixedString64Bytes), // �f�t�H���g�l
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    [SerializeField] private  TextMeshProUGUI nameTMP;

    void Start()
    {
        if (IsOwner)
        {
            // ���[�J���v���C���[�̖��O��ݒ�
            string savedName = PlayerPrefs.GetString("PlayerName", "DefaultName");
            SetPlayerNameServerRpc(savedName);
        }
    }

    public void SetPlayerName(string newName)
    {
        playerName.Value = newName; // �T�[�o�[�Œl���X�V
        nameTMP.text = newName;
    }

    public string GetPlayerName()
    {
        return playerName.Value.ToString();
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

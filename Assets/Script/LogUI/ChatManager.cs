using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager CI;
    [SerializeField] private GameObject chatItemPrefab;
    [SerializeField] private float maxWidth = 250f;

    string myName;
    ulong myId;


    private void Awake()
    {
        CI = this;
    }

    void Start()
    {
        //myName = ClientSetting.CI.TuraaName;
        myName = PlayerPrefs.GetString("TuraaName");
        myId = NetworkManager.Singleton.LocalClientId;
    }


    public void AddBlue(string value, Color senderColor, ulong wisId)
    {
        //DebuLog.C.AddDlList($"AddBlueServerRpc:{myName} , {value} ,{senderColor}");
        GameObject pt = MatchingStatus.C.PartnerTuraa;
        string senderName = myName;
        if (pt) senderName += $" ( & {pt.GetComponent<NamePlate>().GetName()} )";

        if (WisFCheck(wisId, value)) AddBlueServerRpc(senderName, value, senderColor, myId, wisId);
        else WisIdError();
    }

    bool WisFCheck(ulong wisId, string value)
    {
        if (wisId == 9999) return true;

        string tn = NetworkManager.Singleton.ConnectedClients[wisId].PlayerObject.GetComponent<NamePlate>().GetName();
        if (value.StartsWith($"[To:{tn}] ")) return true;
        else return false;
    }

    void WisIdError()
    {
        string senderName = "Error";
        string value = "I couldn't tell.";
        string value2 = "Was this Wis or All ?";
        GameObject chatItem = GenerateChatItem(senderName, value, Color.red);
        GameObject chatItem2 = GenerateChatItem(senderName, value2, Color.red);
        ChatDisplay.CI.ChatItemList.Insert(0, chatItem);
        ChatDisplay.CI.ChatItemList.Insert(0, chatItem2);
        ChatDisplay.CI.UpdateChatDisplay();
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddBlueServerRpc(string senderName, string value, Color senderColor, ulong senderId, ulong wisId)
    {
        //DebuLog.C.AddDlList($"AddBlueServerRpc:{senderName} , {value} ,{senderColor}");
        AddBlueClientRpc(senderName, value, senderColor, senderId, wisId);
    }
    [ClientRpc]
    public void AddBlueClientRpc(string senderName, string value, Color senderColor, ulong senderId, ulong wisId)
    {
        Debug.Log($"addBlueCRpc myId,senderId,wisId:{myId},{senderId},{wisId}");
        if (wisId != 9999)
        {
            Debug.Log($"addBlueCRpc !=9999 pass");
            if (wisId != myId && senderId != myId) return;
            Debug.Log($"addBlueCRpc 2 pass");
            senderColor = new Color(255 / 255, 154 / 255, 255 / 255);
        }

        Debug.Log($"addBlueCRpc senderName,value,senderColor:{senderName},{value},{senderColor}");
        GameObject chatItem = GenerateChatItem(senderName, value, senderColor);
        ChatDisplay.CI.ChatItemList.Insert(0, chatItem);
        ChatDisplay.CI.UpdateChatDisplay();
    }




    GameObject GenerateChatItem(string senderName, string value, Color senderColor)
    {
        GameObject chatItem = Instantiate(chatItemPrefab, ChatDisplay.CI.transform);
        chatItem.GetComponent<ChatItem>().Value.text = $"[ {senderName} ] ";

        foreach (char c in value)
        {
            chatItem.GetComponent<ChatItem>().Value.text += c;
            if (chatItem.GetComponent<ChatItem>().Value.preferredWidth > maxWidth)
            {
                chatItem.GetComponent<ChatItem>().Value.text = chatItem.GetComponent<ChatItem>().Value.text.Substring(0, chatItem.GetComponent<ChatItem>().Value.text.Length - 1) + "(ry";
                break;
            }
        }
        //DebuLog.C.AddDlList($"GenerateChatItem naka");
        chatItem.GetComponent<ChatItem>().SenderName.color = senderColor;
        chatItem.GetComponent<ChatItem>().Value.color = senderColor;
        return chatItem;
    }

    string WisTagChecker()
    {
        return "s";
    }



}

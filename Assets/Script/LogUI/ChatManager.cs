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


    private void Awake()
    {
        CI = this;
    }

    void Start()
    {
        //myName = ClientSetting.CI.TuraaName;
        myName = PlayerPrefs.GetString("TuraaName");
    }

   
    public void AddBlue(string value, Color senderColor)
    {
        //DebuLog.C.AddDlList($"AddBlueServerRpc:{myName} , {value} ,{senderColor}");
        GameObject pt = MatchingStatus.C.PartnerTuraa;
        string senderName = myName;
        if (pt) senderName += $" ( & {pt.GetComponent<NamePlate>().GetName()} )";
        AddBlueServerRpc(senderName, value, senderColor);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddBlueServerRpc(string senderName, string value, Color senderColor)
    {
        //DebuLog.C.AddDlList($"AddBlueServerRpc:{senderName} , {value} ,{senderColor}");
        AddBlueClientRpc(senderName, value, senderColor);
    }
    [ClientRpc]
    public void AddBlueClientRpc(string senderName, string value, Color senderColor)
    {
        //DebuLog.C.AddDlList($"AddBlueClientRpc:{senderName} , {value} ,{senderColor}");

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



}

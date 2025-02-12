using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ChatDisplay : MonoBehaviour
{
    public static ChatDisplay CI;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] float yDistance = 30f;
    public List<GameObject> ChatItemList { get; set; } = new List<GameObject>();
    private Color inputColor = Color.blue;
    ulong wisId = 9999;

    public bool NowInput { get; set; }

    private void Awake()
    {
        CI = this;
    }
    public void SubmitChat()
    {
        if (inputField.text != "")
        {
            DebuLog.C.AddDlList($"OnSubmitChat text,wisId:{inputField.text},{wisId}");
            ChatManager.CI.AddBlue(inputField.text, inputColor, wisId);
        }
        inputField.text = "";
        wisId = 9999;
    }

    public void WisSet(GameObject turaa)
    {
        wisId = turaa.GetComponent<NetworkObject>().OwnerClientId;
        string tn = turaa.GetComponent<NamePlate>().GetName();
        inputField.text = $"[To:{tn}] " + inputField.text;
        Debug.Log($"WisSet end wisId:{wisId}");
    }

    public void OnSelectField()
    {
        NowInput = true;
    }

    public void OnDisableField()
    {
        NowInput = false;
    }


    public void UpdateChatDisplay()
    {
        DebuLog.C.AddDlList($"UpdateChatDisplay");
        for (int i = 0; i < ChatItemList.Count; i++)
        {
            RectTransform chatItemRect = ChatItemList[i].GetComponent<RectTransform>();
            chatItemRect.anchoredPosition = new Vector3(0, i * yDistance, 0);
        }
    }
}

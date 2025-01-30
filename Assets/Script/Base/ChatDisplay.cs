using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChatDisplay : MonoBehaviour
{
    public static ChatDisplay CI;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] float yDistance = 30f;
    public List<GameObject> ChatItemList { get; set; } = new List<GameObject>();
    private Color inputColor = Color.blue;

    private void Awake()
    {
        CI = this;
    }
    public void OnSubmitChat()
    {
        if (inputColor == Color.blue)
        {
            DebuLog.C.AddDlList($"OnSubmitChat:{inputField.text}");
            ChatManager.CI.AddBlue(inputField.text, inputColor);
        }
        inputField.text = "";
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

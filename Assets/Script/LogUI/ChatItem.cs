using TMPro;
using UnityEngine;

public class ChatItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI senderNameField;
    [SerializeField] private TextMeshProUGUI valueField;

    public TextMeshProUGUI SenderName => senderNameField;
    public TextMeshProUGUI Value => valueField;

}

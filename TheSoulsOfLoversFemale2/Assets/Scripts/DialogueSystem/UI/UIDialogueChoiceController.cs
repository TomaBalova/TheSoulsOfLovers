using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueChoiceController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Choice;
    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    private DialogueType m_ChoiceNextType;

    public DialogueChoice Choice
    {
        set
        {
            m_Choice.text = value.ChoicePreview;
            m_ChoiceNextType = value.ChoiceType;
        }
    }

    private void Start()
    {

        GetComponent<Button>().onClick.AddListener(OnClick);
        Debug.Log("popapopapopa");
    }

    private void OnClick()
    {
        Debug.Log("Tytytytytyt");
        m_DialogueChannel.RaiseRequestDialogueType(m_ChoiceNextType);
    }
}

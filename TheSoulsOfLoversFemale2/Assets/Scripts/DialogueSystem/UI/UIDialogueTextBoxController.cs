using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueTextBoxController : MonoBehaviour, DialogueTypeVisitor
{
    [SerializeField]
    private TextMeshProUGUI m_SpeakerText;
    [SerializeField]
    private TextMeshProUGUI m_DialogueText;

    [SerializeField]
    private RectTransform m_ChoicesBoxTransform;
    [SerializeField]
    private UIDialogueChoiceController m_ChoiceControllerPrefab;

    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    private bool m_ListenToInput = false;
    private DialogueType m_NextType = null;

    private void Awake()
    {
        m_DialogueChannel.OnDialogueTypeStart += OnDialogueTypeStart;
        m_DialogueChannel.OnDialogueTypeEnd += OnDialogueTypeEnd;

        gameObject.SetActive(false);
        m_ChoicesBoxTransform.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        m_DialogueChannel.OnDialogueTypeEnd -= OnDialogueTypeEnd;
        m_DialogueChannel.OnDialogueTypeStart -= OnDialogueTypeStart;
    }



    private void Update()
    {
        if (m_ListenToInput && Input.GetButtonDown("Submit"))
        {
            m_DialogueChannel.RaiseRequestDialogueType(m_NextType);
        }
    }

    private void OnDialogueTypeStart(DialogueType type)
    {
        gameObject.SetActive(true);

        m_DialogueText.text = type.DialogueLine.Text;
        m_SpeakerText.text = type.DialogueLine.Speaker.CharacterName;

        m_ChoicesBoxTransform.transform.GetChild(0).gameObject.SetActive(true);

        type.Accept(this);
    }

    private void OnDialogueTypeEnd(DialogueType type)
    {
        m_NextType = null;
        m_ListenToInput = false;
        m_DialogueText.text = "";
        m_SpeakerText.text = "";

        foreach (Transform child in m_ChoicesBoxTransform)
        {
            if (m_ChoicesBoxTransform.transform.GetChild(0) != child)
                Destroy(child.gameObject);
        }


        gameObject.SetActive(false);
        m_ChoicesBoxTransform.gameObject.SetActive(false);
    }

    public void Visit(OneLineDialogueType type)
    {
        m_ListenToInput = true;
        m_NextType = type.NextType;
    }

    public void Visit(ChoiceDialogueType type)
    {
        m_ChoicesBoxTransform.gameObject.SetActive(true);

        var positionBtn = m_ChoicesBoxTransform.transform.GetChild(0).gameObject.transform.position;
        var x = positionBtn.x;
        var y = positionBtn.y;

        foreach (DialogueChoice choice in type.Choices)
        {
            UIDialogueChoiceController newChoice = Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxTransform);
            newChoice.transform.position = new Vector2(x, y);
            newChoice.GetComponent<Button>().image.enabled = true;
            newChoice.GetComponent<Button>().GetComponent<Button>().enabled = true;
            newChoice.GetComponent<Button>().GetComponent<UIDialogueChoiceController>().enabled = true;
            newChoice.GetComponent<Button>().transform.GetChild(0).GetComponent<TMP_Text>().enabled = true;

            newChoice.Choice = choice;
            y += 80;
        }
        m_ChoicesBoxTransform.transform.GetChild(0).gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueException : System.Exception
{
    // Метод обработки ошибок
    public DialogueException(string message)
        : base(message)
    {
    }
}

public class DialogueSequencer : MonoBehaviour
{
    public delegate void DialogueCallback(Dialogue dialogue);
    public delegate void DialogueTypeCallback(DialogueType type);

    public DialogueCallback OnDialogueStart;
    public DialogueCallback OnDialogueEnd;
    public DialogueTypeCallback OnDialogueNodeStart;
    public DialogueTypeCallback OnDialogueNodeEnd;

    private Dialogue m_CurrentDialogue;
    private DialogueType m_CurrentType;


    private Transform player;




    // Обработка начала диалога
    public void StartDialogue(Dialogue dialogue)
    {
        if (m_CurrentDialogue == null)
        {
            player = GameObject.FindWithTag("Player").transform;
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            m_CurrentDialogue = dialogue;
            OnDialogueStart?.Invoke(m_CurrentDialogue);
            StartDialogueType(dialogue.FirstType);

        }
        else
        {
            throw new DialogueException("Can't start a dialogue when another is already running.");
        }
    }

    // Обработка окончания диалога
    public void EndDialogue(Dialogue dialogue)
    {
        if (m_CurrentDialogue == dialogue)
        {
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            StopDialogueType(m_CurrentType);
            OnDialogueEnd?.Invoke(m_CurrentDialogue);
            m_CurrentDialogue = null;
        }
        else
        {
            throw new DialogueException("Trying to stop a dialogue that ins't running.");
        }
    }

    private bool CanStartType(DialogueType type)
    {
        return (m_CurrentType == null || type == null || m_CurrentType.CanBeFollowedByType(type));
    }

    public void StartDialogueType(DialogueType type)
    {
        if (CanStartType(type))
        {
            StopDialogueType(m_CurrentType);

            m_CurrentType = type;

            if (m_CurrentType != null)
            {
                OnDialogueNodeStart?.Invoke(m_CurrentType);
            }
            else
            {
                EndDialogue(m_CurrentDialogue);
            }
        }
        else
        {
            throw new DialogueException("Failed to start dialogue node.");
        }
    }

    private void StopDialogueType(DialogueType type)
    {
        if (m_CurrentType == type)
        {
            OnDialogueNodeEnd?.Invoke(m_CurrentType);
            m_CurrentType = null;
        }
        else
        {
            throw new DialogueException("Trying to stop a dialogue node that ins't running.");
        }
    }
}

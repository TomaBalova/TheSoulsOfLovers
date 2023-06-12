using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInstigator : MonoBehaviour
{
    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    private DialogueSequencer m_DialogueSequencer;

    private void Awake()
    {
        m_DialogueSequencer = new DialogueSequencer();

        m_DialogueSequencer.OnDialogueStart += OnDialogueStart;
        m_DialogueSequencer.OnDialogueEnd += OnDialogueEnd;
        m_DialogueSequencer.OnDialogueNodeStart += m_DialogueChannel.RaiseDialogueTypeStart;
        m_DialogueSequencer.OnDialogueNodeEnd += m_DialogueChannel.RaiseDialogueTypeEnd;

        m_DialogueChannel.OnDialogueRequested += m_DialogueSequencer.StartDialogue;
        m_DialogueChannel.OnDialogueTypeRequested += m_DialogueSequencer.StartDialogueType;
    }

    private void OnDestroy()
    {
        m_DialogueChannel.OnDialogueTypeRequested -= m_DialogueSequencer.StartDialogueType;
        m_DialogueChannel.OnDialogueRequested -= m_DialogueSequencer.StartDialogue;

        m_DialogueSequencer.OnDialogueNodeEnd -= m_DialogueChannel.RaiseDialogueTypeEnd;
        m_DialogueSequencer.OnDialogueNodeStart -= m_DialogueChannel.RaiseDialogueTypeStart;
        m_DialogueSequencer.OnDialogueEnd -= OnDialogueEnd;
        m_DialogueSequencer.OnDialogueStart -= OnDialogueStart;

        m_DialogueSequencer = null;
    }

    private void OnDialogueStart(Dialogue dialogue)
    {
        m_DialogueChannel.RaiseDialogueStart(dialogue);
    }

    private void OnDialogueEnd(Dialogue dialogue)
    {

        m_DialogueChannel.RaiseDialogueEnd(dialogue);
    }
}

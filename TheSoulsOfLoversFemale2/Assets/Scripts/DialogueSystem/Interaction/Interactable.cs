using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    UnityEvent m_OnInteraction;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    GameObject player;


    private bool isCharacterIn = false;

    public bool getIsCharacterIn()
    {
        return isCharacterIn;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isCharacterIn = true;
            text.text = "Äכÿ םאקאכא הטאכמדא םאזלטעו E";
        }
    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && isCharacterIn)
        {
            text.text = "";
            m_OnInteraction.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            text.text = "";
            isCharacterIn = false;
        }
    }

}

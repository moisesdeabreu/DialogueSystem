using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Text titleText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Image iconImage;
    [SerializeField] private RectTransform dialogueBoxPanel;
    [SerializeField] private Button continueButton;

    [Header("Configuration")]
    [SerializeField] private Ease easeTransition;

    private Queue<string> sentences;
    private bool hasImage;
    private bool lastDialog;
    private float hideInSeconds;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        lastDialog = false;
        titleText.text = dialogue.title;
        iconImage.sprite = dialogue.icon;
        hasImage = dialogue.icon != null;
        hideInSeconds = dialogue.autoHideInSeconds;

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        if (sentences.Count == 0 )
        {
            Debug.Log("DialogManager: No sentences!");
            HideDialogueBox();
            return;
        } else
        {
            dialogueBoxPanel.DOAnchorPos(new Vector2(0, -8), .4f).SetEase(easeTransition);
            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        string sentence = sentences.Dequeue();
        switch (sentences.Count)
        {
            case 0:
                lastDialog = true;
                continueButton.gameObject.SetActive(false);
                iconImage.enabled = hasImage;

                StartCoroutine(TypeSentence(sentence));
               
                break;
            default:
                continueButton.gameObject.SetActive(true);
                iconImage.enabled = false;
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
                break;
        }
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        if (hideInSeconds != 0 && lastDialog)
        {
            StartCoroutine("WaitToHideDialogueBox");
        }
    }

    IEnumerator WaitToHideDialogueBox()
    {
        yield return new WaitForSeconds(hideInSeconds);
        HideDialogueBox();
    }

    private void HideDialogueBox()
    {
        dialogueBoxPanel.DOAnchorPos(new Vector2(0, 100), .4f).SetEase(easeTransition);
    }

    private void ShowDialogueBox()
    {
        dialogueBoxPanel.DOAnchorPos(new Vector2(0, -8), .4f).SetEase(easeTransition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimation;
    private Story currentStory;

    public bool dialogueIsPlaying{get; private set; }

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";

    private const string PORTRAIT_TAG = "portrait";

    private const string LAYOUT_TAG = "layout";



    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one dialogue manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }
    private void Update()
    {
        //return if dialoge isn't playing
        if (!dialogueIsPlaying)
        {
            return;
        }

        if(InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
           dialogueText.text = currentStory.Continue();
           //Handletags
           HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        } 
    }

    private void HandleTags(List<string> currentTags)
    {
        // loops through each tag and handle it 
        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                
                case PORTRAIT_TAG:
                    portraitAnimation.Play(tagValue);
                    break;

                case LAYOUT_TAG:
                    Debug.Log("Layout= " + tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;


            
                
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIMgr : MonoBehaviour
{
    private PlayerController pController;
    private PlayerControllerNew pControllerNew;

    public GameObject tipObj;
    public GameObject dialogueBox;
    public Text textLabel;
    public Text nameLabel;

    public TextAsset textAsset;
    public TextAsset finishedTextAsset;
    private List<string> textList = new List<string>();
    public int index=0;
    public float normalSpeed;
    public float skipSpeed;
    public float textSpeed;

    public GameObject player;
    public GameObject npc;

    public bool isFinished;
    private bool hasInit;

    public bool skip;
    public bool isShowing;


    private void Awake()
    {
        pController=this.GetComponentInParent<PlayerController>();
        pControllerNew=this.GetComponentInParent<PlayerControllerNew>();

        EventCenter.Instance.AddEventListener("TriggerDialogue",SetDialogueActive);
        textSpeed = normalSpeed;
    }
    private void OnDisable()
    {
        EventCenter.Instance.RemoveEvent("TriggerDialogue", SetDialogueActive);
    }
    public void InitTextAsset(TextAsset textAsset)
    {
        textList.Clear();
        index = 0;
        string[] lineData=textAsset.text.Split('\n');

        foreach(string line in lineData)
        {
            textList.Add(line);
            //print(line);
        }
    }

    public void HideDialogueBox()
    {
        dialogueBox.SetActive(false);
    }

    public void TriggerDialogue()
    {
        EventCenter.Instance.TriggerEvent("TriggerDialogue",null);
    }
    private void SetDialogueActive(object info)
    {
        if (!hasInit)
        {
            hasInit = true;
            if(tipObj!=null)
                tipObj.SetActive(false);
            pController?.playerInput.GamePlay.Disable();
            pControllerNew?.playerInput.GamePlay.Disable();
            if (isFinished)
                InitTextAsset(finishedTextAsset);
            else
                InitTextAsset(textAsset);
        }                           
        ShowText();
    }

    public void ShowText()
    {
        if (index >= textList.Count)
        {
            dialogueBox.SetActive (false);
            isFinished = true;
            hasInit = false;
            index = 0;
            pController?.playerInput.GamePlay.Enable();
            pControllerNew?.playerInput.GamePlay.Enable();
            return;
        }
        if (!isShowing&&!skip)
        {
            StartCoroutine(ShowTextCoroutine());
        }
        else if (isShowing && !skip)
        {
            skip=true;
        }
    }

    IEnumerator ShowTextCoroutine()
    {
        isShowing = true;
        textLabel.text = "";
        nameLabel.text = ""; 
        print(textList[index]);
        switch (textList[index])
        {
            case "Kronus:\r":
                nameLabel.text = "Kronus";
                this.GetComponent<UITrack>().targetTrans=player.transform;
                dialogueBox.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + this.GetComponent<UITrack>().offset);
                index++;
                break;
            case "Alex:\r":
                nameLabel.text = "Alex";
                this.GetComponent<UITrack>().targetTrans = npc.transform;
                dialogueBox.transform.position = Camera.main.WorldToScreenPoint(npc.transform.position + this.GetComponent<UITrack>().offset);
                index++;
                break;
        }
        dialogueBox.SetActive(true);
        for (int i = 0; i < textList[index].Length; i++)
        {
            if (skip)
                textSpeed = skipSpeed;
            textLabel.text += textList[index][i];
            yield return new WaitForSeconds(textSpeed);
        }

        skip = false;
        textSpeed = normalSpeed;
        isShowing = false;
        index++;
    }
}

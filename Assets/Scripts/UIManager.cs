using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Canvas ui;
    private TextMeshProUGUI text;
    private Image panel;
    private Queue<object[]> queue = new Queue<object[]>();
    private bool isDisplayingText;

    /*private GameObject uIObjectInventoryEmpty;
    private TextMeshProUGUI textObjectInventory;
    private Image imageSlotObjectInventory;*/

    public static event EventHandler<Move> MovementLimitation;

    [SerializeField] private int fadeDuration = 10;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        ui = FindObjectOfType<Canvas>();
        text = ui.GetComponentInChildren<TextMeshProUGUI>();
        panel = ui.GetComponentInChildren<Image>();
        
        /*uIObjectInventoryEmpty = GameObject.Find("UIObjectInventory");
        textObjectInventory = uIObjectInventoryEmpty.GetComponentInChildren<TextMeshProUGUI>();
        imageSlotObjectInventory = uIObjectInventoryEmpty.GetComponentInChildren<Image>(); */
        

    }

    private void OnEnable()
    {
        PlayerScript.DisplayUIText += DisplayText;
        //PlayerScript.DisplayObjectInventory += DisplayObjectInventory;
    }

    private void OnDisable()
    {
        PlayerScript.DisplayUIText -= DisplayText;
        //PlayerScript.DisplayObjectInventory -= DisplayObjectInventory;
    }

    private void DisplayText(object sender, PlayerScript.UIText uiText)
    {
        string displayText = uiText.GetDisplayText();
        int duration = uiText.GetDuration();
        bool limitMovement = uiText.GetMovementLimitation();
        
        if (isDisplayingText)
        {
            object[] toQueue = new object[3];
            toQueue[0] = displayText;
            toQueue[1] = duration;
            toQueue[2] = limitMovement;
            queue.Enqueue(toQueue);
        }
        else
        {
            StartCoroutine(FadeInUI(displayText, duration, limitMovement));
        }
    }

    private IEnumerator FadeInUI(string displayText, int duration, bool isMovementLimited, bool onlyFadeText = false)
    {
        if (isMovementLimited)
        {
            MovementLimitation.Invoke(this, new Move(true));
        }
        text.text = displayText;
        isDisplayingText = true;
        if (!onlyFadeText)
        {
            for (int i = 0; i < fadeDuration; i++)
            {
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b,
                    panel.color.a + 100f / 255 / fadeDuration);
                yield return new WaitForSeconds(1f / 60);
            }
        }
        for (int i = 0; i < fadeDuration; i++)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 
                text.color.a + 1f / fadeDuration);
            yield return new WaitForSeconds(1f / 60);
        }
        StartCoroutine(StayTimer(duration));
    }

    private IEnumerator FadeOutUI(bool onlyFadeText = false)
    {
        for (int i = 0; i < fadeDuration; i++)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 
                text.color.a - 1f / fadeDuration);
            if (!onlyFadeText)
            {
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b,
                    panel.color.a - 100f / 255 / fadeDuration);
            }

            yield return new WaitForSeconds(1f / 60);
        }

        if (!onlyFadeText)
        {
            isDisplayingText = false;
            MovementLimitation.Invoke(this, new Move(false));
        }
    }

    private IEnumerator StayTimer(int duration)
    {
        if (duration == 0)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(duration + 2 * fadeDuration / 60);
        }

        if (queue.Count == 0)
        {
            StartCoroutine(FadeOutUI());
        }
        else
        {
            StartCoroutine(FadeOutUI(true));
            yield return new WaitForSeconds(fadeDuration / 60f);
            object[] fromQueue = queue.Dequeue();
            StartCoroutine(FadeInUI((string) fromQueue[0], (int) fromQueue[1], (bool) fromQueue[2], true));
        }
    }

    public class Move
    {
        private bool isMomvementLimited;

        public Move(bool isMomvementLimited)
        {
            this.isMomvementLimited = isMomvementLimited;
        }
        
        public bool GetMovementLimitation()
        {
            return isMomvementLimited;
        }
    }

    /*private void DisplayObjectInventory(object sender, PlayerScript.UIObjectInventory inventory)
    {
        textObjectInventory.color = new Color(textObjectInventory.color.r, textObjectInventory.color.g, textObjectInventory.color.b,255);
        imageSlotObjectInventory.color = new Color(imageSlotObjectInventory.color.r, imageSlotObjectInventory.color.g,
            imageSlotObjectInventory.color.b, 255);
        imageSlotObjectInventory.sprite = inventory.GetSprite();
    }*/
}

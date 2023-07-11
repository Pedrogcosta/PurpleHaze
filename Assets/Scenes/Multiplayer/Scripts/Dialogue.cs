using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TMP_Text textComponent;
    public float textSpeed;

    public static string[] text;
    public static Action onFinish;

    private bool finished;
    private int index;

    void OnEnable()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (finished)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = text[index];
                finished = true;
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        finished = false;
        foreach(char c in text[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(1 / textSpeed);
        }
        finished = true;
    }

    void NextLine()
    {
        if (index < text.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());

        }
        else
        {
            if (onFinish != null) {
                onFinish();
            }
            gameObject.SetActive(false);
        }
    }
}

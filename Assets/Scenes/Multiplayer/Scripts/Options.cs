using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    public Button[] optionButtons;
    public static string[] options;
    public static Action<int> onPress;

    private int currentSelectionIndex;

    void OnEnable()
    {
        currentSelectionIndex = 0;
        DisplayOptions();
    }

    void Update()
    {
        HandleInput();
    }

    void DisplayOptions()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                var buttonText = optionButtons[i].GetComponentInChildren<TMP_Text>();
                if (i == currentSelectionIndex)
                {
                    buttonText.color = Color.red;
                    buttonText.text = "<" + options[i] + ">";
                }
                else
                {
                    buttonText.color = Color.white;
                    buttonText.text = options[i];
                }
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OptionSelected(int optionIndex)
    {
        onPress(optionIndex);
        gameObject.SetActive(false);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentSelectionIndex--;
            if (currentSelectionIndex < 0)
            {
                currentSelectionIndex = options.Length - 1;
            }
            DisplayOptions();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentSelectionIndex++;
            if (currentSelectionIndex >= options.Length)
            {
                currentSelectionIndex = 0;
            }
            DisplayOptions();
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            OptionSelected(currentSelectionIndex);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxController : MonoBehaviour
{
    public string[] textBoxContents;

    static public bool isActive;

    private int currentTextBox = 0;
    private Text text;
    private Button nextButton;


    void Start()
    {
        if (isActive) Debug.LogError("TWO INSTANCES OF TEXTBOX CONTROLLER ACTIVE!");
        text = GetComponentInChildren<Text>();
        nextButton = GetComponentInChildren<Button>();
        Time.timeScale = 0;
        isActive = true;

        if (nextButton && nextButton.isActiveAndEnabled)
            nextButton.onClick.AddListener(() => {
                NextButton();
            });

        Display();
    }

    public static void CreateTextBoxes(string[] contents)
    {
        TextBoxController prototype = Resources.Load<TextBoxController>("TextBoxController");
        TextBoxController textboxes = Instantiate(prototype);
        textboxes.textBoxContents = contents;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            NextButton();
        }
    }

    private void NextButton()
    {
        ++currentTextBox;
        Display();
    }

    private void Display()
    {
        int textBoxesLength = textBoxContents.Length;
        if (currentTextBox < textBoxesLength)
            text.text = textBoxContents[currentTextBox];
        else
        {
            Destroy(gameObject);
            Time.timeScale = 1;
            isActive = false;
        }
    }
}

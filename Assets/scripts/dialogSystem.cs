using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogSystem : MonoBehaviour
{
    [Header("UI")]
    public Text textLabel;
    public Image faceImage;

    [Header("Text")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;

    [Header("Character Info")]
    public Sprite hat_guy;
    public Sprite player;

    bool textFinished;
    bool cancelTyping;

    List<string> textList = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {
        GetText(textFile);
    }

    private void OnEnable()
    {
        /*textLabel.text = textList[index];
        index++;*/
        textFinished = true;
        StartCoroutine(SetTextUI());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && index == textList.Count)
        {
            gameObject.SetActive(false); // Close dialog box.
            index = 0;
            return; // Exit the function.
        }
        /*if (Input.GetKeyDown(KeyCode.R) && textFinished)
        {
            *//*textLabel.text = textList[index];
            index++;*//*
            StartCoroutine(SetTextUI());
        }*/

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (textFinished && !cancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished)
            {
                cancelTyping = !cancelTyping;
            }
        }
    }

    // Get text from file.
    void GetText(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineData = file.text.Split('\n');

        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    // Use coroutine to display text.
    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";

        // Set the character's face.
        switch (textList[index])
        {
            case "A\r":
                faceImage.sprite = hat_guy;
                index++;
                break;
            case "B\r":
                faceImage.sprite = player;
                index++;
                break;
        }

        /*for (int i = 0; i < textList[index].Length; i++)
        {
            textLabel.text += textList[index][i];

            yield return new WaitForSeconds(textSpeed);
        }*/

        int letter = 0;
        while(!cancelTyping && letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        cancelTyping = false;

        textFinished = true;
        index++;
    }
}

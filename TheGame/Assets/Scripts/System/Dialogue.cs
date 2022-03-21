using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    private Animator anim;
    public bool freeze;
    [Header("UI")]
    public Text textLabel;
    public Image faceImage;    //角色頭像
    public GameObject arrow;

    [Header("text")]
    public TextAsset textFlie; //對話文本
    public int index;          //對話序列
    public float textSpeed;    //跑文字的速度

    [Header("head")]
    public Sprite face01, face02;

    bool textFinished;         //檢查是否句子結束
    bool cancelTyping;

    List<string> textList = new List<string>();
    
    void Awake()
    {
        GetTextFormFile(textFlie);
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(SetTextUI());
        anim.SetTrigger("move");
    }

    void Update()
    {
        StartCoroutine(Animator());
        if (Input.GetKeyDown(KeyCode.M) && !freeze)
        {
            if (textFinished && !cancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished && !cancelTyping)
            {
                cancelTyping = true;
            }
        }
    }

    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineDate = file.text.Split('\n');

        foreach (var line in lineDate)
        {
            textList.Add(line);
        }
    }
    

    IEnumerator SetTextUI()
    {
        arrow.SetActive(false);
        textFinished = false;
        textLabel.text = "";

        switch (textList[index].Trim().ToString())
        {
            case "A":
                faceImage.sprite = face01;
                index++;
                break;
            case "B":
                faceImage.sprite = face02;
                index++;
                break;

        }

        int letter = 0;
        while (!cancelTyping && letter < textList[index].Length -1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
        arrow.SetActive(true);
    }

    IEnumerator Animator()
    {
        if (Input.GetKeyDown(KeyCode.M) && index == textList.Count)  //當對話文本結束
        {
            freeze = true;
            if (Input.GetKeyDown(KeyCode.M))
            {
                anim.SetBool("end", true);
                yield return new WaitForSeconds(0.8f);
                index = 0;
                gameObject.SetActive(false);
                anim.SetBool("end", false);
                freeze = false;
            }
        }
    }
}

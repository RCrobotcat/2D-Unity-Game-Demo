using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkTip : MonoBehaviour
{
    public GameObject pressRToTalkTip;
    public GameObject talkUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            pressRToTalkTip.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        pressRToTalkTip.SetActive(false);
        if (talkUI.activeSelf)
            talkUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pressRToTalkTip.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            talkUI.SetActive(true);
        }
    }
}

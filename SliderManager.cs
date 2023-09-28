using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct PageImages
{
    public RectTransform pageTr;
    public Image[] imgMembers;
    public Button buttonSetter;
}

public class SliderManager : MonoBehaviour
{
    public bool isPageUp;
    public bool isPageDown;
    private float currentTime;

    [Tooltip("ÀÛÀ»¼ö·Ï »¡¶óÁü")]
    [SerializeField] private float moveSpeed = 1f;

    public PageImages[] pages;
    public RectTransform currentPage;
    public int currentPageNum;
    public int targetPageNum;

    private void OnDisable()
    {
        ResetMarker();
    }

    public void ResetMarker()
    {
        isPageUp = false;
        isPageDown = false;

        currentTime = 0.0f;

        currentPageNum = -1;
        targetPageNum = 0;

        foreach(var child in pages)
        {
            child.pageTr.anchoredPosition = new Vector2(-child.pageTr.rect.width, 0.0f);
            child.pageTr.gameObject.SetActive(false);

            child.buttonSetter.enabled = false;
        }

        for (int i = 0; i < pages.Length; i++)
        {
            for (int j = 0; j < pages[i].imgMembers.Length; j++)
            {
                pages[i].imgMembers[j].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
        }

        currentPage = pages[targetPageNum].pageTr;
        currentPage.gameObject.SetActive(true);
    }

    private void Update()
    {
        PageChange();
    }

    public void PageChange()
    {
        if (targetPageNum != currentPageNum)
        {
            if (!isPageDown && currentPageNum != -1)
            {
                pages[currentPageNum].buttonSetter.enabled = false;

                currentPage = pages[currentPageNum].pageTr;

                currentTime += Time.deltaTime;
                if (currentTime >= moveSpeed)
                {
                    currentTime = moveSpeed;

                    currentPage.anchoredPosition = new Vector2(-currentPage.rect.width, 0.0f);

                    for (int i = 0; i < pages[currentPageNum].imgMembers.Length; i++)
                    {
                        pages[currentPageNum].imgMembers[i].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                    }

                    currentPage.gameObject.SetActive(false);

                    currentTime = 0.0f;

                    pages[currentPageNum].buttonSetter.enabled = true;

                    isPageDown = true;
                }

                float t = currentTime / moveSpeed;

                currentPage.anchoredPosition = Vector2.Lerp(currentPage.anchoredPosition, new Vector2(-currentPage.rect.width, 0.0f), t);

                for (int i = 0; i < pages[currentPageNum].imgMembers.Length; i++)
                {
                    pages[currentPageNum].imgMembers[i].color = Color.Lerp(pages[currentPageNum].imgMembers[i].color, new Color(0.0f, 0.0f, 0.0f, 0.0f), t * 0.75f);
                }
            }

            if (!isPageUp)
            {
                pages[targetPageNum].buttonSetter.enabled = false;

                var tmpNum = 0;

                if (targetPageNum < currentPageNum)
                {
                    tmpNum = currentPageNum - 1;
                    if (tmpNum <= 0)
                    {
                        tmpNum = 0;
                    }
                }
                else if (targetPageNum > currentPageNum)
                {
                    tmpNum = currentPageNum + 1;
                    if (tmpNum >= pages.Length)
                    {
                        tmpNum = pages.Length - 1;
                    }
                }

                currentPage = pages[tmpNum].pageTr;
                currentPage.gameObject.SetActive(true);

                currentTime += Time.deltaTime;

                if (currentTime >= moveSpeed)
                {
                    currentTime = moveSpeed;

                    currentPage.anchoredPosition = Vector2.zero;

                    for (int i = 0; i < pages[tmpNum].imgMembers.Length; i++)
                    {
                        pages[tmpNum].imgMembers[i].color = Color.white;
                    }

                    currentPageNum = tmpNum;

                    currentPage = pages[currentPageNum].pageTr;

                    currentTime = 0.0f;

                    pages[targetPageNum].buttonSetter.enabled = true;

                    isPageUp = true;
                }

                float t2 = currentTime / moveSpeed;

                currentPage.anchoredPosition = Vector2.Lerp(currentPage.anchoredPosition, Vector2.zero, t2);

                for (int i = 0; i < pages[tmpNum].imgMembers.Length; i++)
                {
                    pages[tmpNum].imgMembers[i].color = Color.Lerp(pages[tmpNum].imgMembers[i].color, Color.white, t2 * 0.75f);
                }
            }
        }

        if (targetPageNum == currentPageNum)
        {
            isPageUp = false;
            isPageDown = false;
        }
    }

    public void Btn_ChangeNextPage()
    {
        targetPageNum = currentPageNum + 1;
    }
}

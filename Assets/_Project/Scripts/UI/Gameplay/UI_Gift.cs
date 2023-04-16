using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gift : MonoBehaviour
{
    Image selectedImg;
    UI_CollectedGiftController controller;

    int index;
    public void Setup(UI_CollectedGiftController _controller, int _index)
    {
        if(selectedImg == null)
            selectedImg = GetComponent<Image>();

        controller = _controller;
        index = _index;
        GifDeselected();
        DisableGift();
    }

    public void EnableGift()
    {
        gameObject.SetActive(true);
    }

    public void DisableGift()
    {
        gameObject.SetActive(false);
    }

    public void GiftSelected()
    {
        selectedImg.enabled = true;
    }

    public void GifDeselected()
    {
        selectedImg.enabled = false;
    }

    public void InformManagerImSelected()
    {
        controller.GiftSelected(index);
    }
}

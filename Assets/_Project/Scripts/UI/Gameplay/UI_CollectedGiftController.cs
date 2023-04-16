using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class UI_CollectedGiftController : MonoBehaviour
{
    List<UI_Gift> UIGifts = new List<UI_Gift>();
    List<GiftData> currentGifts;
    int indexSelected = 0;

    public void Setup()
    {
        UIGifts = GetComponentsInChildren<UI_Gift>().ToList();
        for (int i = 0; i < UIGifts.Count; i++)
        {
            UIGifts[i].Setup(this, i);
        }
    }

    /// <summary>
    /// Salva i dati dei regali e si attiva
    /// </summary>
    /// <param name="_gifts"></param>
    public void ActivateGiftPanel(List<GiftData> _gifts)
    {
        indexSelected = 0;
        currentGifts = _gifts;
        DeactivateAllLevel();
        for (int i = 0; i < _gifts.Count; i++)
        {
            UI_Gift gift = UIGifts[i];
            gift.EnableGift();
            if (i == indexSelected)
            {
                GiftSelected(i);
            }
        }
    }

    /// <summary>
    /// pulisce il regali e si disabilita
    /// </summary>
    public void RemoveDatas()
    {
        foreach (var gift in UIGifts)
        {
            gift.DisableGift();
        }
    }

    public void GiftSelected(int _index)
    {
        indexSelected = _index;

        for (int i = 0; i < UIGifts.Count; i++)
        {
            if (i == indexSelected)
            {
                UIGifts[i].GiftSelected();
            }
            else
            {
                UIGifts[i].GifDeselected();
            }
        }

        if (currentGifts.Count > 0)
        {
            LevelController.I.GetHouseController().SetHighlight(currentGifts[indexSelected]);
        }
    }

    void DeactivateAllLevel()
    {
        for (int i = 0; i < UIGifts.Count; i++)
        {
            if (i >= currentGifts.Count)
            {
                UIGifts[i].DisableGift();
            }
        }
    }
}

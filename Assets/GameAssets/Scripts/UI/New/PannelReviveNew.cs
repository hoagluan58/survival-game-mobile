using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class PannelReviveNew : PanelBase
{
    [SerializeField] Image imgFillAmountTime;
    [SerializeField] TextMeshProUGUI tmpTimes;

    private Coroutine C_CountDown;

    public override void ActiveMe(Action callBack = null)
    {
        base.ActiveMe(callBack);
        CountDown();
    }


    public void Revive_Click()
    {
        GameMaster.I.Revive();
        this.gameObject.SetActive(false);
    }

    public void NoThank_Clicked()
    {
        GameMaster.I.LoadResult();
    }

    public void CountDown()
    {
        StopCountDown();
        this.gameObject.SetActive(true);
        C_CountDown = StartCoroutine(I_CountDown());
    }

    public void StopCountDown()
    {
        if (C_CountDown != null)
        {
            StopCoroutine(C_CountDown);
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator I_CountDown()
    {
        float t = 5f;

        while (t > 0f)
        {
            t--;
            tmpTimes.text = t.ToString();
            imgFillAmountTime.DOFillAmount(t / 5f, 0.2f);
            yield return new WaitForSeconds(1f);
        }

        GameMaster.I.LoadResult();
    }

}

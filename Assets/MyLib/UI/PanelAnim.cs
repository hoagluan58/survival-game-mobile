using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class PanelAnim : MonoBehaviour
{
    public UIElementAnim[] m_elementAnims;

    public void Show(Action callBack = null)
    {
        float maxDuration = 0;
        if (m_elementAnims != null && m_elementAnims.Length > 0)
        {
            for (int i = 0; i < m_elementAnims.Length; i++)
            {
                maxDuration = Mathf.Max(maxDuration, m_elementAnims[i].Duration);
                m_elementAnims[i].Show();
            }
        }

        DOTween.To((t) => { }, 0, maxDuration, maxDuration).OnComplete(() => { callBack(); });
    }

    public void Hide(Action callBack = null)
    {
        float maxDuration = 0;
        if (m_elementAnims != null && m_elementAnims.Length > 0)
        {
            for (int i = 0; i < m_elementAnims.Length; i++)
            {
                maxDuration = Mathf.Max(maxDuration, m_elementAnims[i].Duration);
                m_elementAnims[i].Hide();
            }
        }

        DOTween.To((t) => { }, 0, maxDuration, maxDuration).OnComplete(() => { callBack(); });
    }
}

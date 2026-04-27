using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimRedWarning : MonoBehaviour
{
    private Image _img;

    private void OnEnable()
    {
        if (_img == null)
            _img = GetComponent<Image>();
        StartCoroutine(IE_Anim());
    }
    private IEnumerator IE_Anim()
    {
        while (true)
        {
            _img.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _img.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}

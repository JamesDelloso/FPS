using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image[] image;
    private Color32[] normalImageColour;
    [SerializeField]
    private Color32[] highlightImageColour;

    [SerializeField]
    private TMP_Text[] text;
    private Color32[] normalTextColour;
    [SerializeField]
    private Color32[] highlightTextColour;
    private FontStyles normalFontStyle;
    [SerializeField]
    private FontStyles highlightFontStyle = FontStyles.Normal;

    private Vector3 normalScale;
    [SerializeField]
    private Vector3 highlightScale = Vector3.one;

    // Start is called before the first frame update
    void Start()
    {
        normalImageColour = new Color32[image.Length];
        for(int i=0;i<image.Length;i++)
        {
            normalImageColour[i] = image[i].color;
        }
        //if(image != null && image.Length > 0)
        //{
        //    normalImageColour = image[0].color;
        //}
        normalTextColour = new Color32[text.Length];
        for(int i=0;i<text.Length;i++)
        {
            normalTextColour[i] = text[i].color;
            normalFontStyle = text[0].fontStyle;
        }
        //if(text != null && text.Length > 0)
        //{
        //    normalTextColour = text[0].color;
        //    normalFontStyle = text[0].fontStyle;
        //}
        normalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = highlightScale;
        for(int i=0;i<image.Length;i++)
        {
            image[i].color = highlightImageColour[i];
        }
        for(int i=0;i<text.Length;i++)
        {
            text[i].color = highlightTextColour[i];
            text[i].fontStyle = highlightFontStyle;
        }
        //foreach(Image i in image)
        //{
        //    i.color = highlightImageColour;
        //}
        //foreach (TMP_Text t in text)
        //{
        //    t.color = highlightTextColour;
        //    t.fontStyle = highlightTextStyle;
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = normalScale;
        for (int i = 0; i < image.Length; i++)
        {
            image[i].color = normalImageColour[i];
        }
        for (int i = 0; i < text.Length; i++)
        {
            text[i].color = normalTextColour[i];
            text[i].fontStyle = normalFontStyle;
        }
        //foreach (Image i in image)
        //{
        //    i.color = normalImageColour;
        //}
        //foreach (TMP_Text t in text)
        //{
        //    t.color = normalTextColour;
        //    t.fontStyle = normalFontStyle;
        //}
    }
}

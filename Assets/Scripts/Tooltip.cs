using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class Tooltip : MonoBehaviour
{
    private static GameObject tooltip;
    private static GameObject text;
    private static RectTransform rectT;
    private static TextMeshProUGUI textC;
    private int spacing = 10;
    private static bool show = false;

    void Start()
    { 
        Init();
    }

    private void Init()
    {
        tooltip = GameObject.Find("Canvas").transform.Find("Tooltip").gameObject;
        text = tooltip.transform.Find("TooltipText").gameObject;
        rectT = tooltip.GetComponent<RectTransform>();
        textC = text.GetComponent<TextMeshProUGUI>();
    }

    private Vector3 CalcTooltipPos()
    {
        Vector3 ret = new Vector3();
        ret = Input.mousePosition;

        Vector3[] corners = new Vector3[4];
        rectT.GetWorldCorners(corners);


        float height = Vector3.Distance(corners[0], corners[1]);
        float width = Vector3.Distance(corners[0], corners[3]);

        if ((ret.y + height) > Screen.height)
        {
            ret.y = Screen.height - (height / 2);
        }
        else
        {
            ret.y += (height / 2);

        }


        if ((ret.x + width) > Screen.width)
        {
            ret.x = Screen.width - (width / 2);
        }
        else
        {
            ret.x += (width / 2);
        }

        return ret;
    }



    private Vector2 CalcSize()
    {
        return new Vector2(textC.preferredWidth + spacing * 2f, textC.preferredHeight + spacing * 2f);
    }

    public void ShowItem(string item)
    {
        show = true;
        string itemDesc = Inventory.itemsJson["items"][Inventory.GetItemIndex(item)]["description"].stringValue;
        if (textC.text != itemDesc)
        {
            textC.text = itemDesc;
        }

        tooltip.SetActive(true);


    }

    private void Update()
    {
        if (show)
        {
            rectT.sizeDelta = CalcSize();
            rectT.position = CalcTooltipPos();
        }
    }

    public void ShowString(string name)
    {
        show = true;

        if (textC.text != name)
        {
            textC.text = name;
        }

        tooltip.SetActive(true);

    }

    public void Hide()
    {
        if (tooltip != null)
        {
            tooltip.SetActive(false);
        }
        show = false;
    }
}

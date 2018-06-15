using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AllyRobotController : RobotController
{
    private ViewManager viewManager;
    private Text text;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        viewManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ViewManager>();

        GameObject UItextGO = new GameObject("Text" + gameObject.name);
        UItextGO.transform.SetParent(GameObject.Find("Canvas").transform);

        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.anchoredPosition = Vector2.zero;

        text = UItextGO.AddComponent<Text>();
        text.text = "";
        text.color = Color.black;
        text.fontSize = 20;
        text.font = Resources.Load("Arial") as Font;
        text.alignment = TextAnchor.UpperCenter;
    }

    private void OnGUI()
    {
        Vector2 uiPos = viewManager.currentCamera.WorldToViewportPoint(transform.position);
        if (uiPos.x > 0 && uiPos.x < 1 && uiPos.y > 0 && uiPos.y < 1 && viewManager.MapMode)
        {
            text.gameObject.SetActive(true);
            text.text = _payload.ToString();
            text.rectTransform.anchorMin = uiPos;
            text.rectTransform.anchorMax = uiPos;
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
}

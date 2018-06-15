using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResourceManagement : MonoBehaviour {
    protected int _initialLoad = 100;
    [HideInInspector]
    public int _currentLoad = 0;
    protected ViewManager _viewManager;
    protected Text _text;

    #region Getter/Settes for initialLoad and currentLoad
    public void SetInitialLoad(int load)
    {
        _initialLoad = load;
    }

    public void SetCurrentLoad(int load)
    {
        _currentLoad = load;
    }

    public int GetCurrentLoad()
    {
        return _currentLoad;
    }

    public void IncrementLoad(int inc)
    {
        _currentLoad += inc;
    }
    #endregion

    protected virtual void Start () {

        SetCurrentLoad(_initialLoad);
        _viewManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ViewManager>();
    }

    protected Text InitText(Text text)
    {
        GameObject UItextGO = new GameObject("Text" + gameObject.name);
        UItextGO.transform.SetParent(GameObject.Find("Canvas").transform);
        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.anchoredPosition = Vector2.zero;
        text = UItextGO.AddComponent<Text>();
        text.text = "";
        text.color = Color.black;
        text.fontSize = 20;
        text.font = ResourcesData.GetObject(ResourcesData._font, FontEnum.ARIAL) as Font;
        text.alignment = TextAnchor.UpperCenter;

        return text;
    }
	
    /// <summary>
    /// Used when taking resources from here
    /// </summary>
    /// <param name="resourcesTaken">Nb of resources to try to take</param>
    /// <returns>returns the number of resources taken</returns>
    public int TakeResources(int resourcesTaken)
    {
        if (_currentLoad - resourcesTaken >= 0)
        {
            _currentLoad -= resourcesTaken;
            return resourcesTaken;
        }
        else
        {
            if (_currentLoad == 0)
                return _currentLoad;
            else
            {
                int load = _currentLoad;
                _currentLoad = 0;
                return load;
            }
        }
    }


    /// <summary>
    /// Displays the number of resources left if the player is on MapMode
    /// </summary>
    protected virtual void OnGUI()
    {
        if (_text && _viewManager.currentCamera)
        {
            Vector2 uiPos = _viewManager.currentCamera.WorldToViewportPoint(transform.position);
            if (uiPos.x > 0 && uiPos.x < 1 && uiPos.y > 0 && uiPos.y < 1 && _viewManager.MapMode)
            {
                _text.gameObject.SetActive(true);
                _text.text = _currentLoad.ToString() + " / " + _initialLoad;
                _text.rectTransform.anchorMin = uiPos;
                _text.rectTransform.anchorMax = uiPos;
            }
            else
            {
                _text.gameObject.SetActive(false);
            }
        }
    }
}

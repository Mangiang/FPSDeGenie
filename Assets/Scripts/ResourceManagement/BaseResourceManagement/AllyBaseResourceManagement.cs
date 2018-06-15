using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AllyBaseResourceManagement : BaseResourceManagement
{
    private AllyBaseManager _baseManager;

    protected override void Start()
    {
        base.Start();

        _baseManager = GetComponent<AllyBaseManager>();
        _text = base.InitText(_text);
    }

    /// <summary>
    /// Displays the number of resources left if the player is on MapMode
    /// </summary>
    protected override void OnGUI()
    {
        Vector2 uiPos = _viewManager.currentCamera.WorldToViewportPoint(transform.position);
        if (uiPos.x > 0 && uiPos.x < 1 && uiPos.y > 0 && uiPos.y < 1 && _viewManager.MapMode)
        {
            _text.gameObject.SetActive(true);
            _text.text = _currentLoad.ToString();
            _text.rectTransform.anchorMin = uiPos;
            _text.rectTransform.anchorMax = uiPos;
        }
        else
        {
            _text.gameObject.SetActive(false);
        }
    }

    public override void DumpResources(int load)
    {
        base.DumpResources(load);

        NodeResourceManagement node = _baseManager.Dest.GetComponent<NodeResourceManagement>();
        if (node && node._currentLoad > 0)
        {
            _baseManager.SetDestination(_baseManager.Dest);
        }
        else
        {
            _baseManager.SetDestination(null);
        }
    }
}

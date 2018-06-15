using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NodeResourceManagement : ResourceManagement {

    protected override void Start()
    {
        base.Start();
        _text = base.InitText(_text);
    }
}

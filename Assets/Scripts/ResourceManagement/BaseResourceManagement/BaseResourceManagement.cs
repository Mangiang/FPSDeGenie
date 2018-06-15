using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BaseResourceManagement : ResourceManagement
{
    protected override void Start()
    {
        base.Start();

        SetCurrentLoad(0);
        SetInitialLoad(int.MaxValue);
    }
    
    /// <summary>
    /// Used when dumping resources in
    /// </summary>
    /// <param name="load">The resources to add</param>
    public virtual void DumpResources(int load)
    {
        int newLoad = _currentLoad + load;
        _currentLoad = (newLoad <= _initialLoad ? newLoad : _initialLoad);
    }
}

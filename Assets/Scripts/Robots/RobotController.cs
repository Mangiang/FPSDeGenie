using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RobotController : MonoBehaviour {

    protected NavMeshAgent _navAgent;
    protected GameObject _resourceNodeDestination;
    [HideInInspector]
    protected GameObject _base;
    protected GameObject _currentDestination;
    protected int _payload = 0;
    protected int _maxPV = 30;
    protected int _PV = 30;
    protected bool _alive = true;

	// Use this for initialization
	protected virtual void Start () {
        _navAgent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_PV <= 0 && _alive)
        {
            _alive = false;
            _navAgent.isStopped = true;
            _navAgent.enabled = false;
            GetComponent<Collider>().enabled = false;
            transform.rotation = Quaternion.Euler(new Vector3(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            Instantiate(ResourcesData.GetObject(ResourcesData._fx, FxEnum.SMOKE), transform.position, Quaternion.identity);
        }
	}
    
    public void SetBase(GameObject Base)
    {
        _base = Base;
    }

    public void SetDestination(GameObject dst)
    {
        if (_payload == 0)
        {
            if (_navAgent == null)
            {
                _navAgent = GetComponent<NavMeshAgent>();
            }
            _navAgent.SetDestination(dst.transform.position);
            _currentDestination = dst;
            _navAgent.isStopped = false;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Resource" && collision.gameObject == _currentDestination)
        {
            _navAgent.isStopped = true;
            _navAgent.SetDestination(_base.transform.position);
            _currentDestination = _base;
            _navAgent.isStopped = false;
            if (_payload == 0)
                _payload = collision.gameObject.GetComponent<ResourceManagement>().TakeResources(10);
        }
        else if (collision.gameObject == _base && _currentDestination == _base)
        {
            _navAgent.isStopped = true;
            int prevPayLoad = _payload;
            _payload = 0;
            collision.gameObject.GetComponent<BaseResourceManagement>().DumpResources(prevPayLoad);
        }
    }

    public void Damage(int dmg)
    {
        _PV -= dmg;
    }
}

using UnityEngine;

public class LiftAnimationScript : MonoBehaviour
{

    private Transform _cable;
    private Transform _hook;
    
    private float _cableStartLength = 1.9f;
    private float _cableEndLength = 0.5f;

    private float _hookStartHeight = 8.25f;
    private float _hookEndHeight = 29.0f;

    private bool _isLifting = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cable = transform.Find("scp_cy_crane_01_cable");
        _hook = transform.Find("scp_cy_crane_01_hook");
    }

    private void LiftAnimation()
    {
        _cable.localScale = new Vector3(_cable.localScale.x, (float)(_cable.localScale.y - 0.05), _cable.localScale.z);
        _hook.localPosition = new Vector3(_hook.localPosition.x, (float)(_hook.localPosition.y + 0.05), _hook.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

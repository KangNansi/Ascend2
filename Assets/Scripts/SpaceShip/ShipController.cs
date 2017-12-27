using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    public SpaceShip ship;
    public Transform target;
    public RectTransform targetUI;
    public float sensibility;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float hr = Input.GetAxis("HorizontalR");
        float vr = Input.GetAxis("VerticalR");


        MoveShip(new Vector3(h, v, 0) * ship.speed);
        MoveTarget(new Vector3(hr, -vr, 0) * Time.deltaTime * sensibility);

        ship.weapon.target = target;

        ship.Shooting = Input.GetAxis("Fire1")>0.1f;
        

	}

    void MoveShip(Vector3 motion)
    {
        ship.rigid.velocity = motion;
        ClampedPosition(ship.transform);
    }

    void MoveTarget(Vector3 motion)
    {
        ClampedMove(target, motion);
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.transform.position);
        targetUI.position = screenPoint;
    }

    void ClampedMove(Transform t, Vector3 motion)
    {
        Vector3 viewPosition = Camera.main.WorldToViewportPoint(t.transform.position + motion);
        if (viewPosition.x < 0.1f)
            viewPosition.x = 0.1f;
        if (viewPosition.x > 0.9f)
            viewPosition.x = 0.9f;
        if (viewPosition.y < 0.1f)
            viewPosition.y = 0.1f;
        if (viewPosition.y > 0.9f)
            viewPosition.y = 0.9f;
        t.transform.position = Camera.main.ViewportToWorldPoint(viewPosition);
    }

    void ClampedPosition(Transform t)
    {
        Vector3 viewPosition = Camera.main.WorldToViewportPoint(t.transform.position);
        if (viewPosition.x < 0.1f)
            viewPosition.x = 0.1f;
        if (viewPosition.x > 0.9f)
            viewPosition.x = 0.9f;
        if (viewPosition.y < 0.1f)
            viewPosition.y = 0.1f;
        if (viewPosition.y > 0.9f)
            viewPosition.y = 0.9f;
        t.transform.position = Camera.main.ViewportToWorldPoint(viewPosition);
    }
}

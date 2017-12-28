using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    public SpaceShip ship;
    public Transform target;
    public RectTransform targetUI;
    public float sensibility;
    public float travelingSpeed = 2.0f;

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
        MoveTarget(new Vector3(h, v, 0) * sensibility);

        //ship.transform.rotation = Quaternion.LookRotation((target.position - ship.transform.position).normalized, Vector3.up);
        ship.transform.rotation = Quaternion.AngleAxis(h * -45, ship.transform.forward) * Quaternion.AngleAxis(v * -25, ship.transform.right);
        ship.weapon.target = target;
        //ship.weapon.baseSpeed = transform.forward * travelingSpeed;
        //ship.weaponB.baseSpeed = transform.forward * travelingSpeed;
        ship.weapon.Direction = (target.position - transform.position).normalized;
        ship.weaponB.Direction = ship.weapon.Direction;

        ship.Shooting = Input.GetAxis("Fire1")>0.1f;

        transform.position += transform.forward * Time.deltaTime * travelingSpeed;
	}

    void MoveShip(Vector3 motion)
    {
        ship.rigid.velocity = motion;
        ClampedPosition(ship.transform);
    }

    void MoveTarget(Vector3 motion)
    {
        Vector3 viewportShip = Camera.main.WorldToViewportPoint(ship.transform.position+ship.transform.forward*10);
        //motion.z = 1;
        //motion = (motion + Vector3.one) / 2f;
        motion += viewportShip;
        //motion.z = 10;
        Ray ray = Camera.main.ViewportPointToRay(motion);
        target.transform.position = ray.origin + ray.direction * 100;
        //target.position = Camera.main.ViewportToWorldPoint(motion);
        ClampedPosition(target);
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

    void ClampedPosition(Transform t, float min = 0.01f, float max = 0.99f)
    {
        Vector3 viewPosition = Camera.main.WorldToViewportPoint(t.transform.position);
        if (viewPosition.x < min)
            viewPosition.x = min;
        if (viewPosition.x > max)
            viewPosition.x = max;
        if (viewPosition.y < min)
            viewPosition.y = min;
        if (viewPosition.y > max)
            viewPosition.y = max;
        t.transform.position = Camera.main.ViewportToWorldPoint(viewPosition);
    }
}

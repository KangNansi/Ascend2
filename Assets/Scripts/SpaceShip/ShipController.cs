using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

public class ShipController : MonoBehaviour {

    public delegate void OnUpdate();
    public class ShipState
    {
        public float time = 0f;
        public bool receiveDamage = true;

        public OnUpdate update = null;
    }

    Stack<ShipState> stateMachine = new Stack<ShipState>();

    public SpaceShip ship;
    public Transform target;
    public Canvas UICanvas;
    public RectTransform targetUI;
    public RectTransform targetUIB;
    public Image lifeBar;
    public BGCcMath path;
    new public CameraController camera;
    float distance = 0;
    public float CurrentDistance
    {
        get
        {
            return distance;
        }
    }
    public float CurrentDistanceRatio
    {
        get
        {
            return distance/path.GetDistance();
        }
    }

    public Text DistanceUI;

    public float sensibility;
    public float travelingSpeed = 2.0f;
    public float targetForwardValue = 7f;
    public float targetDistance = 4f;
    public float targetBDistance = 2f;

    public float rollDirection = 1f;
    public float rollCooldown = 2f;
    float rollCooldownBuffer = 2f;

    //Locking
    public Missile missile;
    public Image lockUI;
    bool LockMode = false;
    List<Enemy> targets = new List<Enemy>();
    List<Image> targetsUI = new List<Image>();

    float shipPosition;
    Vector3 tangent;
    public Vector3 Tangent
    {
        get { return tangent; }
    }

    float h;
    float v;

    // Use this for initialization
    void Start () {
        ship.Controller = this;
        shipPosition = ship.transform.localPosition.z;
        path.Fields = BGCurveBaseMath.Fields.PositionAndTangent;

        ShipState normal = new ShipState();
        normal.update = OnUpdateNormal;
        stateMachine.Push(normal);

        ship.OnDamage += () => camera.Shake(2f);
	}
	
	// Update is called once per frame
	void Update () {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        float hr = Input.GetAxis("HorizontalR");
        float vr = Input.GetAxis("VerticalR");

        distance += Time.deltaTime * travelingSpeed;

        MoveShip(new Vector3(h, v, 0) * ship.speed);
        MoveTarget(new Vector3(h, v, 0) * sensibility);

        //ship.transform.rotation = Quaternion.LookRotation((target.position - ship.transform.position).normalized, Vector3.up);
        ship.transform.localPosition = new Vector3(ship.transform.localPosition.x, ship.transform.localPosition.y, shipPosition);
        ship.receiveDamage = stateMachine.Peek().receiveDamage;
        if (stateMachine.Peek().update != null)
        {
            stateMachine.Peek().update();
        }
        ship.weapon.target = target;
        ship.weapon.baseSpeed = transform.forward * travelingSpeed;
        ship.weaponB.baseSpeed = transform.forward * travelingSpeed;
        ship.weapon.Direction = (target.position - ship.transform.position).normalized;
        ship.weaponB.Direction = ship.weapon.Direction;

        

        //transform.position += transform.forward * Time.deltaTime * travelingSpeed;
        


        DistanceUI.text = "Distance: " + distance;
        transform.position = path.CalcPositionAndTangentByDistance(distance, out tangent);
        //Debug.Log(tangent);
        transform.rotation = Quaternion.LookRotation(tangent);

        //Locking
        if(Input.GetAxis("TargetFire") > 0.2f && !LockMode)
        {
            LockMode = true;
        }
        else if(Input.GetAxis("TargetFire") < 0.2f && LockMode)
        {
            LockMode = false;
            StartCoroutine(SpawnMissile());
        }
        if(LockMode)
        {
            LockingUpdate();
        }

        if(lifeBar != null)
        {
            lifeBar.fillAmount = (ship.Life / ship.maxLife);
        }
	}

    void OnUpdateNormal()
    {
        rollCooldownBuffer += Time.deltaTime;
        ship.Shooting = Input.GetAxis("Fire1") > 0.1f;
        ship.transform.localRotation = Quaternion.AngleAxis(h * -45, Vector3.forward)
            * Quaternion.AngleAxis(v * -25, Vector3.right);

        if (Input.GetButtonDown("Dash") && rollCooldownBuffer > rollCooldown)
        {
            rollDirection = (h < 0) ? -1 : 1;
            ship.Shooting = false;
            ShipState roll = new ShipState();
            roll.update = OnUpdateRoll;
            roll.receiveDamage = false;
            stateMachine.Push(roll);
        }
    }

    void OnUpdateRoll()
    {
        stateMachine.Peek().time += Time.deltaTime;
        ship.transform.localRotation = Quaternion.AngleAxis(rollDirection * - 1400 * stateMachine.Peek().time, Vector3.forward)
            * Quaternion.AngleAxis(v * -25, Vector3.right);
        if(stateMachine.Peek().time > 1f)
        { 
            rollCooldownBuffer = 0;
            stateMachine.Pop();
        }
    }

    bool IsOutOfScreen(GameObject g)
    {
        Vector3 viewPosition = Camera.main.WorldToViewportPoint(g.transform.position);
        if(viewPosition.x < 0 || viewPosition.x > 1 || viewPosition.y < 0 || viewPosition.y > 1)
        {
            return true;
        }
        return false;
    }

    void LockingUpdate()
    {
        //Remove destroyed and out of sights
        for(int i = targets.Count-1; i >= 0; i--)
        {
            if(targets[i] == null || IsOutOfScreen(targets[i].gameObject))
            {
                targets.RemoveAt(i);
                Destroy(targetsUI[i]);
                targetsUI.RemoveAt(i);
            }
        }

        Ray ray = new Ray(ship.transform.position, (target.transform.position - ship.transform.position).normalized);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 0.1f);
        if(Physics.Raycast(ray, out hit, 1000, ~LayerMask.NameToLayer("Enemies"), QueryTriggerInteraction.Ignore))
        {
            Enemy e = hit.collider.GetComponent<Enemy>();
            if(e != null && !targets.Contains(e))
            {
                targets.Add(e);
                targetsUI.Add(Instantiate<Image>(lockUI, UICanvas.transform));
            }
        }
        LockingUIUpdate();
    }

    void LockingUIUpdate()
    {
        for(int i = 0; i < targets.Count; i++)
        {
            targetsUI[i].rectTransform.position = Camera.main.WorldToScreenPoint(targets[i].transform.position);
        }
    }

    void MoveShip(Vector3 motion)
    {
        ship.rigid.velocity = transform.rotation * motion;
        ClampedPosition(ship.transform);
    }

    void MoveTarget(Vector3 motion)
    {
        Ray shipRay = new Ray(transform.position, (ship.transform.position - transform.position).normalized);
        Vector3 viewportShip = Camera.main.WorldToViewportPoint(ship.transform.position+ ship.transform.forward*targetForwardValue);
        //motion.z = 1;
        //motion = (motion + Vector3.one) / 2f;
        motion += viewportShip;
        //motion.z = 10;
        Ray ray = Camera.main.ViewportPointToRay(motion);
        target.transform.position = ship.transform.position + ray.direction * targetDistance;
        //target.position = Camera.main.ViewportToWorldPoint(motion);
        ClampedPosition(target);
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.transform.position);
        targetUI.position = screenPoint;
        Vector3 targetBpos = ship.transform.position + (target.transform.position - ship.transform.position) * targetBDistance;
        targetUIB.position = Camera.main.WorldToScreenPoint(targetBpos);
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

    void ClearLockBuffer()
    {

    }

    IEnumerator SpawnMissile()
    {
        foreach(Enemy e in targets)
        {
            if(e != null)
            {
                Missile m = Instantiate<Missile>(missile);
                m.transform.position = ship.weapon.transform.position;
                m.target = e.transform;
            }
            yield return new WaitForSeconds(0.2f);
        }
        targets.Clear();
        foreach (Image i in targetsUI)
        {
            Destroy(i);
        }
        targetsUI.Clear();
    }
}

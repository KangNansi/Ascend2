using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public ShipController controller;
    
    [System.Serializable]
    public class Event
    {
        public Squadron squadron;
        public float distance;
        public float offset = 300;
        public Vector3 SquadronOffset;
    }
    [SerializeField]
    public List<Event> events;
    int progression = 0;

	// Use this for initialization
	void Start () {
        Sort();
	}

    public void Sort()
    {
        events.Sort((a, b) => (int)(a.distance - b.distance));
    }
	
	// Update is called once per frame
	void Update () {
		if(progression < events.Count && events[progression].distance < controller.CurrentDistance)
        {
            Debug.Log("Spawning");
            Squadron squad = Instantiate<Squadron>(events[progression].squadron);
            //squad.transform.position = controller.path.CalcByDistance(BansheeGz.BGSpline.Curve.BGCurveBaseMath.Field.Position, events[0].distance + 5);
            squad.Launch(controller.path, events[progression].distance+events[progression].offset, events[progression].SquadronOffset);
            progression++;
        }
	}
}

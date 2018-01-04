using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor {

    Vector3 scrollView;
    BGCcMath path;
    int pointDistance = 50;
    float pointSize = 0.2f;

    int controlID = 0;

    LevelManager.Event selected = null;

    private void OnEnable()
    {
        LevelManager level = target as LevelManager;
        path = level.controller.path;
    }

    public override void OnInspectorGUI()
    {
        LevelManager level = target as LevelManager;
        //base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();

        level.controller = (ShipController)EditorGUILayout.ObjectField(level.controller, typeof(ShipController), true);

        pointDistance = EditorGUILayout.IntField("Point Distance", pointDistance);
        pointSize = EditorGUILayout.FloatField("Point Size", pointSize);

        //Draw Selected
        if(selected != null)
        {
            Color c = GUI.backgroundColor;
            GUIStyle s = new GUIStyle();
            s.normal.textColor = Color.white;
            GUI.backgroundColor = new Color(0.5f, 0.0f, 0.0f);
            GUILayout.BeginVertical("Box");
            GUI.backgroundColor = c;
            EventEditor(selected);
            GUILayout.EndVertical();
        }

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Add"))
        {
            level.events.Add(new LevelManager.Event());
        }
        if (GUILayout.Button("Sort"))
        {
            level.Sort();
        }
        EditorGUILayout.EndHorizontal();
        
        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        
        EditorGUILayout.BeginVertical();
        for(int i = 0; i < level.events.Count; i++)
        {
            if(EventEditor(level.events[i]))
            {
                level.events.RemoveAt(i);
                i--;
                continue;
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
        Rect drop_area = GUILayoutUtility.GetLastRect();
        DragAndDropper(drop_area);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }

    void DragAndDropper(Rect drop_area)
    {
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    Debug.Log("Dragged Object");
                    foreach (Object dragged_object in DragAndDrop.objectReferences)
                    {
                        if(dragged_object is GameObject)
                        {
                            GameObject obj = (dragged_object as GameObject);
                            Squadron s = obj.GetComponent<Squadron>();
                            if(s != null)
                            {
                                LevelManager.Event e = new LevelManager.Event();
                                e.squadron = s;
                                (target as LevelManager).events.Add(e);
                            }
                        }
                    }
                }
                break;
        }
    }

    bool EventEditor(LevelManager.Event e)
    {
        EditorGUILayout.BeginVertical("Box");
        if(GUILayout.Button("Select"))
        {
            selected = e;
            Repaint();
        }
        if(GUILayout.Button("Copy"))
        {
            LevelManager.Event newEvt = new LevelManager.Event();
            newEvt.squadron = e.squadron;
            newEvt.offset = e.offset;
            newEvt.distance = e.distance;
            newEvt.SquadronOffset = e.SquadronOffset;
            (target as LevelManager).events.Add(newEvt);
        }
        EditorGUILayout.BeginHorizontal();
        e.squadron = (Squadron)EditorGUILayout.ObjectField(e.squadron, typeof(Squadron), true);
        e.distance = LabeledFloatField("Distance", e.distance);
        e.offset = LabeledFloatField("Offset", e.offset);
        if(GUILayout.Button(EditorGUIUtility.IconContent("TL Close button"), "IconButton"))
        {
            return true;
        }
        EditorGUILayout.EndHorizontal();

        e.SquadronOffset = EditorGUILayout.Vector3Field("", e.SquadronOffset);

        EditorGUILayout.EndVertical();
        return false;
    }

    float LabeledFloatField(string label, float field)
    {
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(100));
        EditorGUILayout.LabelField(label, GUILayout.MaxWidth(100));
        float r = EditorGUILayout.FloatField(field, GUILayout.MaxWidth(100));
        EditorGUILayout.EndVertical();
        return r;
    }

    //Scene GUI

    private void OnSceneGUI()
    {
        controlID = GUIUtility.GetControlID(FocusType.Passive);
        DropOnScene();
        LevelManager level = target as LevelManager;
        float maxDistance = path.GetDistance();
        Vector3 lastPos = path.CalcByDistance(BGCurveBaseMath.Field.Position, 0);
        Handles.color = Color.white;
        for (int i = pointDistance; i < maxDistance; i+= pointDistance)
        {
            Vector3 pos = path.CalcByDistance(BGCurveBaseMath.Field.Position, i);
            Handles.DrawLine(lastPos, pos);
            lastPos = pos;
        }
        Handles.DrawLine(lastPos, path.CalcByDistance(BGCurveBaseMath.Field.Position, maxDistance));

        Selection();

        List<LevelManager.Event> events = level.events;
        foreach(LevelManager.Event e in events)
        {
            EventHandler(e);
        }

        if(selected != null)
        {
            DrawSelectedInfo();
        }
    }

    private void Selection()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            foreach(LevelManager.Event e in (target as LevelManager).events)
            {
                Vector3 position = path.CalcByDistance(BGCurveBaseMath.Field.Position, e.distance);
                Vector2 screenPosition = HandleUtility.WorldToGUIPoint(position);
                if (Vector2.Distance(screenPosition, Event.current.mousePosition) < 5)
                {
                    selected = e;
                    Repaint();
                }
            }
        }
    }

    float DistanceToRay(Vector3 X0, Ray ray) {
       Vector3 X1 = ray.origin; // get the definition of a line from the ray
       Vector3 X2 = ray.origin + ray.direction;
       Vector3 X0X1 = (X0-X1); 
       Vector3 X0X2 = (X0-X2); 
  
       return ( Vector3.Cross(X0X1,X0X2).magnitude / (X1-X2).magnitude ); // magic
     }

private void DropOnScene()
    {
        Event evt = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;

        Vector3 closestPoint = new Vector3();
        float minDistance = -1;
        float pDist = 0;
        for(int i = 0; i < path.GetDistance(); i+=pointDistance)
        {
            Vector3 p = path.CalcByDistance(BGCurveBaseMath.Field.Position, i);
            float dist = DistanceToRay(p, ray);
            if (minDistance == -1 || dist < minDistance)
            {
                closestPoint = p;
                minDistance = dist;
                pDist = i;
            }
        }

        switch (evt.type)
        {
            case EventType.Repaint:
                if(DragAndDrop.objectReferences.Length > 0)
                {
                    Handles.color = Color.cyan;
                    Handles.SphereHandleCap(controlID, closestPoint, Quaternion.identity, pointSize * HandleUtility.GetHandleSize(closestPoint), EventType.Repaint);
                }
                break;
            case EventType.DragUpdated:
            case EventType.DragPerform:
                Debug.Log("Dropping");
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    Debug.Log("Dragged Object");
                    foreach (Object dragged_object in DragAndDrop.objectReferences)
                    {
                        if (dragged_object is GameObject)
                        {
                            Debug.Log("Object is GameObject");
                            GameObject obj = (dragged_object as GameObject);
                            Squadron s = obj.GetComponent<Squadron>();
                            if (s != null)
                            {
                                Debug.Log("Adding Object");
                                LevelManager.Event e = new LevelManager.Event();
                                e.squadron = s;
                                e.distance = pDist;
                                (target as LevelManager).events.Add(e);
                            }
                        }
                    }
                }
                Event.current.Use();

                break;
        }
    }

    private void EventHandler(LevelManager.Event e)
    {
        Handles.color = (e == selected)?Color.yellow:Color.white;
        Vector3 position = path.CalcByDistance(BGCurveBaseMath.Field.Position, e.distance);
        Vector3 handlePosition = Handles.FreeMoveHandle(position, Quaternion.identity, pointSize * HandleUtility.GetHandleSize(position), Vector3.one, Handles.SphereHandleCap);
        path.CalcPositionByClosestPoint(handlePosition, out e.distance);


        GUIStyle style = new GUIStyle();
        style.fontSize = 12;
        style.normal.textColor = Color.red;
        //Information
        if(e.squadron != null)
            Handles.Label(position, e.squadron.ToString(), style);
    }

    private void DrawSelectedInfo()
    {
        Vector3 spawnPosition = path.CalcByDistance(BGCurveBaseMath.Field.Position, selected.distance+selected.offset);
        Handles.color = Color.red;
        Vector3 handlePosition = Handles.FreeMoveHandle(spawnPosition, Quaternion.identity, pointSize * 0.5f * HandleUtility.GetHandleSize(spawnPosition), Vector3.one, Handles.SphereHandleCap);
        float newPosition;
        path.CalcPositionByClosestPoint(handlePosition, out newPosition);
        selected.offset = newPosition - selected.distance;

    }
}

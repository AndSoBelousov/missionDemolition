using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine Supere; // Одиночка

    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake()
    {
        Supere = this;// Установить ссылку на объект одиночку
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }

    public GameObject poi
    {
        get { return (_poi); }                  
        set
        {
            _poi = value;
            if(_poi!=null)
            {
                //если содержит действительную ссылку, сбросить все остальные параметры в исходное состояние
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    private void AddPoint()
    {
        //вызывается для добовления точки линии
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist) { return; } // если точка недостаточно далеко от преведущей, просто выйти 

        if(points.Count ==0)
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            //установить первые две точки
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //включить LineRenderer
            line.enabled = true;
        }
        else
        {
            //обычная последовательность добавления точек
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    //возвращаем местоположение последней созданной точки 
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                return (Vector3.zero);

            }
            return (points[points.Count - 1]);
        }
    }

    private void FixedUpdate()
    {
        if(poi == null)//если свойство poi содержит пустое значение, найти интересующий объект
        {
            if(FollowCam.POI != null)
            {
                if(FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else { return; }
            }
            else { return; }
        }
        AddPoint();
        if(FollowCam.POI == null)
        {
            // усли FollowCam.POI содержит null, записать null в poi
            poi = null;
        }
    }
}

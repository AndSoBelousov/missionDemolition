using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject    POI; // Ссылка на интересующий объект
    [Header("Set in Inspector")]
    public float                easing = 0.05f;
    public Vector2              minXY = Vector2.zero;
    [Header("Set Dynamically")]
    public float                camZ; //желаемая координата Z

    private void Awake()
    {
        camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        //if (POI == null) return; // выйти если нет интересующего объекта

        //Vector3 destination = POI.transform.position;

        Vector3 destination;
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;
            // Если интересующий объект - снаряд, убедиться, что он остановился
            if(POI.tag =="Projectile")
            {
                if(POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        transform.position = destination;

        //Изменить размер orthographicSize камеры, чтобы земля осталась в поле зрения 
        Camera.main.orthographicSize = destination.y + 10;
    }
}

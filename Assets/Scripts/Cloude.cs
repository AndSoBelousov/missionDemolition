using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloude : MonoBehaviour
{
    public GameObject   cloudeSphere;
    public int          numSpheresMin = 6;
    public int          numSpheresMax = 10;
    public Vector3      sphereOffsetScale = new Vector3(5, 2, 1);
    public Vector2      sphereScaleRangeX = new Vector2(4, 8);
    public Vector2      sphereScaleRangeY = new Vector2(3, 4);
    public Vector2      sphereScaleRangeZ = new Vector2(2, 4);
    public float        scaleYMin = 2f;

    private List<GameObject> spheres;

    private void Start()
    {
        spheres = new List<GameObject>();

        int num = Random.Range(numSpheresMin, numSpheresMax);
        for(int i = 0; i < num; i++)
        {
            GameObject sp = Instantiate<GameObject>(cloudeSphere);
            spheres.Add(sp);
            Transform spTrans = sp.transform;
            spTrans.SetParent(this.transform);

            //Случайное местороложение 
            Vector3 offset = Random.insideUnitSphere;
            offset.x = sphereOffsetScale.x;
            offset.y = sphereOffsetScale.y;
            offset.z = sphereOffsetScale.z;
            spTrans.localPosition = offset;

            //Случайный масштаб
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            //Скорректировать мастштаб у по расстоянию х от центра  
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            spTrans.localScale = scale;
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Restart();
        //}

    }

    private void Restart()
    {
        foreach(GameObject sp in spheres)
        {
            Destroy(sp);
        }
        Start();
    }
}


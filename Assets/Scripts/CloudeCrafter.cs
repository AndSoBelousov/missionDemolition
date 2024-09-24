using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudeCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 CloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f;

    private GameObject[] cloudInstances;

    private void Awake()
    {
        // Создаем массив для хранения всех экземпляров облаков
        cloudInstances = new GameObject[numClouds];
        // Найти родительский игровой объект CloudAnchor
        GameObject anchor = GameObject.Find("CloudAnchor");
        // Создать в цикле заданное количиство облаков
        GameObject cloud;
        for(int i =0; i < numClouds; i ++)
        {
            cloud = Instantiate<GameObject>(cloudPrefab);
            //местоположение
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.RandomRange(cloudPosMin.x, CloudPosMax.x);
            cPos.y = Random.RandomRange(cloudPosMin.y, CloudPosMax.y);

            //Масштабировать облако
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //Меньшие облока(с меньшим scaleU) должны быть ближе к  земле 
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100 - 90 * scaleU;

            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //сделать облако дочерним anchor
            cloud.transform.SetParent(anchor.transform);

            cloudInstances[i] = cloud;
        }

    }

    private void Update()
    {
        // Обойти в цикл все созданные облака
        foreach(GameObject cloud in cloudInstances)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            //скорость быстрее для ближних облаков 
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            // если облако сместилось далеко в лево
            if(cPos.x <= cloudPosMin.x)
            {
                cPos.x = CloudPosMax.x;
            }

            cloud.transform.position = cPos;    
        }


    }
}

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
        // ������� ������ ��� �������� ���� ����������� �������
        cloudInstances = new GameObject[numClouds];
        // ����� ������������ ������� ������ CloudAnchor
        GameObject anchor = GameObject.Find("CloudAnchor");
        // ������� � ����� �������� ���������� �������
        GameObject cloud;
        for(int i =0; i < numClouds; i ++)
        {
            cloud = Instantiate<GameObject>(cloudPrefab);
            //��������������
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.RandomRange(cloudPosMin.x, CloudPosMax.x);
            cPos.y = Random.RandomRange(cloudPosMin.y, CloudPosMax.y);

            //�������������� ������
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //������� ������(� ������� scaleU) ������ ���� ����� �  ����� 
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100 - 90 * scaleU;

            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //������� ������ �������� anchor
            cloud.transform.SetParent(anchor.transform);

            cloudInstances[i] = cloud;
        }

    }

    private void Update()
    {
        // ������ � ���� ��� ��������� ������
        foreach(GameObject cloud in cloudInstances)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            //�������� ������� ��� ������� ������� 
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            // ���� ������ ���������� ������ � ����
            if(cPos.x <= cloudPosMin.x)
            {
                cPos.x = CloudPosMax.x;
            }

            cloud.transform.position = cPos;    
        }


    }
}

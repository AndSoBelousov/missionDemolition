using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    private GameObject _launchPoint;

    //поля, устанавливаемые в инспекторе Unity
    [Header("Set in Inspector")]
    public GameObject          prefabProjectile;
    public float               velocityMult = 8f;

    //поля, устанавливамые динамически
    [Header("Set Dynamicall")]
    public GameObject          launshPoint;
    public Vector3             launchPos;
    public GameObject          projectile;
    public bool                aimingMode;

    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get 
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;

        } 

    }

    private void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        _launchPoint = launchPointTrans.gameObject;
        _launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    private void OnMouseEnter()
    {
        //print("Slingshot: OnMouseEnter");
        _launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        //print("Slingshot: OnMouseExit");
        _launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        // Игрок нажал кнопку мыши, когда указатель был над рогадкой
        aimingMode = true;
        //Создать снаряд
        projectile = Instantiate(prefabProjectile) as GameObject;
        //Поместить в точку _launchPoint
        projectile.transform.position = launchPos;
        //Сделать его кинематическим
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    private void Update()
    {
        //Если рогатка не в режиме прицеливания, не выполнять этот код
        if (!aimingMode) return;

        //Получить текущие экранные координаты указателя мыши
        Vector3 mousePos2D = Input.mousePosition ;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //Найти разность координат между launchPos и mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;

        //Ограничить mouseDelta радиусом коллайдера объекта Slingshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //Передвинуть снаряд в новую позицию
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if(Input.GetMouseButtonUp(0)) //Кнопка мыши отпущена
        {
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null; // не удаляет созданный экземпляр Projectile, но освободит поле projectile

            MissionDemolition.ShotFired();
            ProjectileLine.Supere.poi = projectile;
        }
    }
}

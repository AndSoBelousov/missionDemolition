using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playng,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // скрытый объект одиночка

    [Header("Set in Inspector")]
    // ссылки на текст
    public Text uitLevel;
    public Text uitShorts;
    public Text uitButton;
    

    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set Dynamiclly")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // режим FollowCam

    private void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    private void StartLevel()
    {
        // уничтожить прежний замок если он существует 
        if(castle !=null)
        {
            Destroy(castle);
        }

        //уничтожить прежние снаряды, если они существуют
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //создать новый замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //Переустановить камеру в начальную позицию 
        SwitchView("Show Both");
        ProjectileLine.Supere.Clear();

        //Сбросить цель
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playng;
    }

    private void UpdateGUI()
    {
        // показать данные в элементах ПИ
        uitLevel.text = "Level: " + (level + 1) + " of" + levelMax;
        uitShorts.text = "Shots Taken: " + shotsTaken;
    }

    private void Update()
    {
        UpdateGUI();

        // проверить завершение уровня
        if ((mode == GameMode.playng) && Goal.goalMet)
        {
            //Изменить режим, чтобы прекратить проверку завершения уровня
            mode = GameMode.levelEnd;
            // Уменьшить масштаб
            SwitchView("Show Both");
            // начать новый уровень через 2 секж
            Invoke("NextLevel", 2f);
        }
        
    }

    void NextLevel()
    {
        level++;
        if(level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if ( eView == "") { eView = uitButton.text; }

        showing = eView;
        switch(showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }

    }

    //Статический метод, позволяющий из любого кода увеличить shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}

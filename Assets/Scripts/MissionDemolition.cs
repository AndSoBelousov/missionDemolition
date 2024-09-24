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
    static private MissionDemolition S; // ������� ������ ��������

    [Header("Set in Inspector")]
    // ������ �� �����
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
    public string showing = "Show Slingshot"; // ����� FollowCam

    private void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    private void StartLevel()
    {
        // ���������� ������� ����� ���� �� ���������� 
        if(castle !=null)
        {
            Destroy(castle);
        }

        //���������� ������� �������, ���� ��� ����������
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //������� ����� �����
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //�������������� ������ � ��������� ������� 
        SwitchView("Show Both");
        ProjectileLine.Supere.Clear();

        //�������� ����
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playng;
    }

    private void UpdateGUI()
    {
        // �������� ������ � ��������� ��
        uitLevel.text = "Level: " + (level + 1) + " of" + levelMax;
        uitShorts.text = "Shots Taken: " + shotsTaken;
    }

    private void Update()
    {
        UpdateGUI();

        // ��������� ���������� ������
        if ((mode == GameMode.playng) && Goal.goalMet)
        {
            //�������� �����, ����� ���������� �������� ���������� ������
            mode = GameMode.levelEnd;
            // ��������� �������
            SwitchView("Show Both");
            // ������ ����� ������� ����� 2 ����
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

    //����������� �����, ����������� �� ������ ���� ��������� shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}

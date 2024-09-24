using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // статическое поле доступное любому другому коду
    static public bool goalMet = false;

    private const string _projectile = "Projectile";
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == _projectile)
        {
            Goal.goalMet = true;

            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}

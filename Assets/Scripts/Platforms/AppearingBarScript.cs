using UnityEngine;
using System.Collections;

public class AppearingBarScript : MonoBehaviour
{
    bool activated = true;
    // set A of objects
    GameObject verticalBar1;
    GameObject verticalBar2;
    GameObject verticalBar3;
    GameObject horizontalBar1;
    GameObject horizontalBar2;
    GameObject horizontalBar3;
    // set B of objects
    GameObject verticalBar4;
    GameObject verticalBar5;
    GameObject verticalBar6;
    GameObject horizontalBar4;
    GameObject horizontalBar5;
    GameObject horizontalBar6;

    void Start()
    {
        // set A of objects
        verticalBar1 = GameObject.FindWithTag("verticalBar1");
        verticalBar2 = GameObject.FindWithTag("verticalBar2");
        verticalBar3 = GameObject.FindWithTag("verticalBar3");
        horizontalBar1 = GameObject.FindWithTag("horizontalBar1");
        horizontalBar2 = GameObject.FindWithTag("horizontalBar2");
        horizontalBar3 = GameObject.FindWithTag("horizontalBar3");
        // set B of objects
        verticalBar4 = GameObject.FindWithTag("verticalBar4");
        verticalBar5 = GameObject.FindWithTag("verticalBar5");
        verticalBar6 = GameObject.FindWithTag("verticalBar6");
        horizontalBar4 = GameObject.FindWithTag("horizontalBar4");
        horizontalBar5 = GameObject.FindWithTag("horizontalBar5");
        horizontalBar6 = GameObject.FindWithTag("horizontalBar6");
    }

    void Update()
    {
        if (activated && CharacterController2D.PlayerJumped)
        {
            CharacterController2D.PlayerJumped = false;
            // set A of objects
            verticalBar1.SetActive(false);
            verticalBar2.SetActive(false);
            verticalBar3.SetActive(false);
            horizontalBar1.SetActive(false);
            horizontalBar2.SetActive(false);
            horizontalBar3.SetActive(false);
            // set B of objects
            verticalBar4.SetActive(true);
            verticalBar5.SetActive(true);
            verticalBar6.SetActive(true);
            horizontalBar4.SetActive(true);
            horizontalBar5.SetActive(true);
            horizontalBar6.SetActive(true);

            activated = !activated;
        }
        if (!activated && CharacterController2D.PlayerJumped)
        {
            CharacterController2D.PlayerJumped = false;
            // set A of objects
            verticalBar1.SetActive(true);
            verticalBar2.SetActive(true);
            verticalBar3.SetActive(true);
            horizontalBar1.SetActive(true);
            horizontalBar2.SetActive(true);
            horizontalBar3.SetActive(true);
            // set B of objects
            verticalBar4.SetActive(false);
            verticalBar5.SetActive(false);
            verticalBar6.SetActive(false);
            horizontalBar4.SetActive(false);
            horizontalBar5.SetActive(false);
            horizontalBar6.SetActive(false);

            activated = !activated;
        }
    }
}
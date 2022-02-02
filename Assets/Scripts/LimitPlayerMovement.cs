using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LimitPlayerMovement
{
    private static GameObject player;

    public static void LimitMovement()
    {
        player = GameObject.Find("Player");      



        //player.GetComponent<Movement>().enabled = false;
        //player.GetComponent<CameraMove>().enabled = false;
        //player.GetComponent<Sprint>().enabled = false;
        //player.GetComponent<Jump>().enabled = false;
    }

    public static void DisableLimitMovement()
    {
        player = GameObject.Find("Player");

        player.GetComponent<Movement>().enabled = true;
        player.GetComponent<CameraMove>().enabled = true;
        player.GetComponent<Sprint>().enabled = true;
        player.GetComponent<Jump>().enabled = true;
    }
}
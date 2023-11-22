using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour{

    public int blackDeathCount = 0;
    public int whiteDeathCount = 0;
    void OnTriggerEnter (Collider other) {
        switch (other.gameObject.tag)
        {
            case "Red" : GameManager.red--; blackDeathCount++; break;
            case "Blue": GameManager.blue--; whiteDeathCount++; break;
            default: Debug.Log("Death script Error!"); break;
        }
        GameObject.Destroy(other.gameObject);
	}
}

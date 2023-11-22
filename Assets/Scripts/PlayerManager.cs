using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public int ownId;
    PhotonView PV;
    RoomManager roomManager;

    GameObject controller;
    void Awake(){
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PV.IsMine){
            CreateController();
        }
    }

    void CreateController(){
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        Debug.Log("Instantiated Player Controller");
    }

    public void Die(int _ownId)
    {
        Destroy(controller);
        CreateController();
    }
}

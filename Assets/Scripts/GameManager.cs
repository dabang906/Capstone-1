using System;
using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.IO;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

public class AlBool
{
	public int turn = 1;
}


public class GameManager : MonoBehaviour {
	public static int turn = 1, red = 3, blue = 3;
	public static bool allowShoot = true;
    public static float pow = 8.0f;

    public GameObject[] blackSpawnPoint;
    public GameObject[] whiteSpawnPoint;
	public GameObject spawnpoints;
	public GameObject newSpawnPoint;

	public AlBool alBool = new AlBool();

	public int ownId;
	public int blackNewSpawn = 0, whiteNewSpawn = 0;

    public string path;
	BallDrag bd;

	Death death;
    void Awake()
    {
        path = Application.persistentDataPath + "/save";
		Debug.Log(path);
        //red += blackNewSpawn - death.blackDeathCount;
		//blue += whiteNewSpawn - death.whiteDeathCount;
		if (alBool.turn == 1)
		{
			SaveTurnData();
		}
		LoadTurnData();
		turn = alBool.turn;
        if (turn == 1)
		{
            if (PhotonNetwork.IsMasterClient)
			{
				for (int i = 0; i < red	; i++)
				{
					PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AL1"), blackSpawnPoint[i].transform.position, blackSpawnPoint[i].transform.rotation);
				}
			}
			else
			{
				for (int i = 0; i < blue; i++)
				{
					PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AL4"), whiteSpawnPoint[i].transform.position, whiteSpawnPoint[i].transform.rotation);
				}
			}
		}
		else
		{

            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < red; i++)
                {
                    path = Application.persistentDataPath + "/black" + i;
                    string data = File.ReadAllText(path);
					bd.al = JsonUtility.FromJson<Al>(data);
                    string[] tmpPosArray = bd.al.pos.Split('/');
                    string[] tmpRoArray = bd.al.rot.Split('/');

                    Vector3 TmpPos = new Vector3(float.Parse(tmpPosArray[0]), float.Parse(tmpPosArray[1]), float.Parse(tmpPosArray[2]));
                    Vector3 TmpRo = new Vector3(float.Parse(tmpRoArray[0]), float.Parse(tmpRoArray[1]), float.Parse(tmpRoArray[2]));

                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AL1"), TmpPos, Quaternion.identity);
                }
            }
            else
            {
                for (int i = 0; i < blue; i++)
                {
                    path = Application.persistentDataPath + "/white" + i+4;
                    string data = File.ReadAllText(path);
                    bd.al = JsonUtility.FromJson<Al>(data);
                    string[] tmpPosArray = bd.al.pos.Split('/');
                    string[] tmpRoArray = bd.al.rot.Split('/');

                    Vector3 TmpPos = new Vector3(float.Parse(tmpPosArray[0]), float.Parse(tmpPosArray[1]), float.Parse(tmpPosArray[2]));
					Vector3 TmpRo = new Vector3(float.Parse(tmpRoArray[0]), float.Parse(tmpRoArray[1]), float.Parse(tmpRoArray[2]));
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AL4"), TmpPos, Quaternion.identity);

                }
            }
        }
		if (turn % 5 == 0 && ownId == 2) {
			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AL1"), newSpawnPoint.transform.position, newSpawnPoint.transform.rotation);
            blackNewSpawn++;
		}
		else if (turn % 5 == 0 && ownId == 1) {
			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AL4"), newSpawnPoint.transform.position, newSpawnPoint.transform.rotation);
            whiteNewSpawn++;
        }
        spawnpoints.SetActive(false);
    }

    void Start()
    {
        path = Application.persistentDataPath + "/save"; // 경로 지정
        if (turn % 2 == 1)
	    {
		    player1Turn();
	    }
	    else
	    {
		    player2Turn();
	    }
    }

    public void player1Turn(){
		transform.position = new Vector3 (0, 18, -7);
		transform.rotation = Quaternion.Euler (50, 0, 0);
		if (turn % 2 == 0)
		{
			turn++;
		}
	}
	public void player2Turn(){
		transform.position = new Vector3 (0, 17, 4);
		transform.rotation = Quaternion.Euler (50, 180, 0);
		if(turn % 2 == 1){
			turn++;	
		}
	}

    void OnDisable()
    {
		SaveTurnData();
    }

	public void SaveTurnData()
	{
        alBool.turn = turn;
        string data = JsonUtility.ToJson(alBool);
        File.WriteAllText(path, data);
    }
	public void LoadTurnData()
	{
        string data = File.ReadAllText(path);
        alBool = JsonUtility.FromJson<AlBool>(data);
    }
}

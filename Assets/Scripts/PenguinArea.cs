using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

public class PenguinArea : Area
{
    public PenguinAgent penguinAgent;
    public GameObject penguinBaby;
    public TextMeshPro cumulativeRewardText;
    public Fish fishPrefeb;

    private PenguinAcademy penguinAcademy;
    private List<GameObject> fishList;

    public override void ResetArea(){

    	RemoveAllFis();
    	PlacePenguin();
		PlaceBaby();
		SpawnFish(4, penguinAcademy.FishSpeed);
    }
    public void RemoveSpecificFish(GameObject fishObject){
    	fishList.Remove(fishObject);
    	Destroy(fishObject);
    }
    public int FishRemaining{
    	get { retun fishList.coun; }//
    }
}

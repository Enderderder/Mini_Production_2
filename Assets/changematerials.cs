using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changematerials : MonoBehaviour {
    MeshRenderer meshRenderer;
    public GameObject player;
    public Material[] spawnable;
    public Material[] notspawnable;
	// Use this for initialization
	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();

	}
    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "NonSpawnable")
        {
            meshRenderer.sharedMaterials = notspawnable;
            player.GetComponent<Player>().canspawnturretzone = false;
        }
        else if (other.tag == "Spawnable")
        {
            meshRenderer.sharedMaterials = spawnable;
            player.GetComponent<Player>().canspawnturretzone = true;
        }

        
    }
    private void OnTriggerExit(Collider other)
    {
        meshRenderer.sharedMaterials = notspawnable;
        player.GetComponent<Player>().canspawnturretzone = false;
    }
}

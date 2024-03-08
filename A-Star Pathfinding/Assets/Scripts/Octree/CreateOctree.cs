using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOctree : MonoBehaviour
{
    public GameObject[] worldObjects;
    public int nodeMinSize = 5;
    Octree otree;

    void Start()
    {
        otree = new Octree(worldObjects, nodeMinSize);
    }
    void OnDrawGizmos()
    {
        if (Application.isPlaying) 
        {
            otree.rootNode.Draw();
        }    
    }
}

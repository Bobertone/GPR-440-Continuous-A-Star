using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Octree
{
    public OctreeNode rootNode;

    public void CreateOctree(GameObject[] worldObjects, float minNodeSize, LayerMask WorldObjLayer)
    {
        Bounds bounds = new Bounds();
        foreach (GameObject go in worldObjects)
        {
            bounds.Encapsulate(go.GetComponent<Collider>().bounds);
        }

        float maxSize = Mathf.Max(new float[] { bounds.size.x, bounds.size.y, bounds.size.z });
        Vector3 sizeVector = new Vector3(maxSize, maxSize, maxSize) * 0.5f;
        bounds.SetMinMax(bounds.center - sizeVector, bounds.center + sizeVector);
        rootNode = new OctreeNode(bounds, minNodeSize);
        AddObjects(worldObjects);        
    }

    public void AddObjects(GameObject[] worldObjects) 
    {
        foreach (GameObject go in worldObjects) 
        {
            rootNode.AddObject(go);
        }
    }

}
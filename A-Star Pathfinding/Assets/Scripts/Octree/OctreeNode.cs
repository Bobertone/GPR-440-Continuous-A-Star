using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using static Unity.VisualScripting.Metadata;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static UnityEngine.UI.Image;

public class OctreeNode
{
    Bounds nodeBounds;
    float minSize;
    Bounds[] childBounds;
    OctreeNode[] children = null;
    bool isOccupied = false;
    bool isTotallyOccupied = false;
    float accCost;
    public float heuristic = 1f;
    public bool visited = false;
    float childLength;
    public OctreeNode(Bounds b, float minNodeSize)
    {
        nodeBounds = b;
        minSize = minNodeSize;

        float quarter = nodeBounds.size.y / 4.0f;
        childLength = nodeBounds.size.y / 2.0f;
        Vector3 childSize = new Vector3(childLength, childLength, childLength);
        childBounds = new Bounds[8];
        childBounds[0] = new Bounds(nodeBounds.center + new Vector3(quarter, quarter, quarter), childSize);
        childBounds[1] = new Bounds(nodeBounds.center + new Vector3(-quarter, quarter, quarter), childSize);
        childBounds[2] = new Bounds(nodeBounds.center + new Vector3(quarter, -quarter, quarter), childSize);
        childBounds[3] = new Bounds(nodeBounds.center + new Vector3(quarter, quarter, -quarter), childSize);
        childBounds[4] = new Bounds(nodeBounds.center + new Vector3(-quarter, -quarter, quarter), childSize);
        childBounds[5] = new Bounds(nodeBounds.center + new Vector3(-quarter, quarter, -quarter), childSize);
        childBounds[6] = new Bounds(nodeBounds.center + new Vector3(quarter, -quarter, -quarter), childSize);
        childBounds[7] = new Bounds(nodeBounds.center + new Vector3(-quarter, -quarter, -quarter), childSize);
    }

    public void AddObject(GameObject go) 
    {
        DivideAndAdd(go);
    }

    public void DivideAndAdd(GameObject go)
    {
        if (nodeBounds.Intersects(go.GetComponent<Collider>().bounds))
        {
            isOccupied = true;
        
            if (nodeBounds.size.y <= minSize)
            {
                //RemoveCorners();
                isTotallyOccupied = true;
                return;
            } 
        }
        
        if (children == null) 
        {
            children = new OctreeNode[8];
        }
        bool dividing = false;
        for (int i = 0; i < 8; i++) 
        {
            if (children[i] == null) 
            {
                children[i] = new OctreeNode(childBounds[i], minSize);
            }
            if (childBounds[i].Intersects(go.GetComponent<Collider>().bounds)) 
            {
                dividing = true;
                children[i].DivideAndAdd(go);
            }
        }

        if (dividing == false)
        {
            Debug.Log("wow");
            children = null;
        }
    }

    public void AddCorners(LayerMask worldObjLayer)
    {
            //+++
            Vector3Int corner = new Vector3Int((int)(nodeBounds.center.x + childLength), (int)(nodeBounds.center.y + childLength), (int)(nodeBounds.center.z + childLength));
            if (!Physics.CheckSphere(corner, .1f, worldObjLayer))
                if (!CreateOctree.pointMap.ContainsKey(corner))
                    CreateOctree.pointMap.Add(corner, new Point(corner));
            //-++
            corner = new Vector3Int((int)(nodeBounds.center.x - childLength), (int)(nodeBounds.center.y + childLength), (int)(nodeBounds.center.z + childLength));
            if (!Physics.CheckSphere(corner, .1f, worldObjLayer))
                if (!CreateOctree.pointMap.ContainsKey(corner))
                    CreateOctree.pointMap.Add(corner, new Point(corner));
            //+-+
            corner = new Vector3Int((int)(nodeBounds.center.x + childLength), (int)(nodeBounds.center.y - childLength), (int)(nodeBounds.center.z + childLength));
            if (!Physics.CheckSphere(corner, .1f, worldObjLayer))
                if (!CreateOctree.pointMap.ContainsKey(corner))
                    CreateOctree.pointMap.Add(corner, new Point(corner));
            //++-
            corner = new Vector3Int((int)(nodeBounds.center.x + childLength), (int)(nodeBounds.center.y + childLength), (int)(nodeBounds.center.z - childLength));
            if (!Physics.CheckSphere(corner, .1f, worldObjLayer))
                if (!CreateOctree.pointMap.ContainsKey(corner))
                    CreateOctree.pointMap.Add(corner, new Point(corner));
            //--+
            corner = new Vector3Int((int)(nodeBounds.center.x - childLength), (int)(nodeBounds.center.y - childLength), (int)(nodeBounds.center.z + childLength));
            if (!Physics.CheckSphere(corner, .1f, worldObjLayer))
                if (!CreateOctree.pointMap.ContainsKey(corner))
                    CreateOctree.pointMap.Add(corner, new Point(corner));
            //-+-
            corner = new Vector3Int((int)(nodeBounds.center.x - childLength), (int)(nodeBounds.center.y + childLength), (int)(nodeBounds.center.z - childLength));
            if (!Physics.CheckSphere(corner, .1f, worldObjLayer))
                if (!CreateOctree.pointMap.ContainsKey(corner))
                    CreateOctree.pointMap.Add(corner, new Point(corner));
            //+--
            corner = new Vector3Int((int)(nodeBounds.center.x + childLength), (int)(nodeBounds.center.y - childLength), (int)(nodeBounds.center.z - childLength));
            if (!Physics.CheckSphere(corner, .1f, worldObjLayer))
                if (!CreateOctree.pointMap.ContainsKey(corner))
                    CreateOctree.pointMap.Add(corner, new Point(corner));
            //---
            corner = new Vector3Int((int)(nodeBounds.center.x - childLength), (int)(nodeBounds.center.y - childLength), (int)(nodeBounds.center.z - childLength));
            if (!Physics.CheckSphere(corner, .1f, worldObjLayer))
                if (!CreateOctree.pointMap.ContainsKey(corner))
                    CreateOctree.pointMap.Add(corner, new Point(corner));
        if (children != null)
            {
            for (int i = 0; i < 8; i++)
            {
                if (children[i] != null)
                {
                    children[i].AddCorners(worldObjLayer);
                }
            }
        }
    }
    public void RemoveCorners()
    {
        //+++
        Vector3Int corner = new Vector3Int((int)(nodeBounds.center.x + childLength), (int)(nodeBounds.center.y + childLength), (int)(nodeBounds.center.z + childLength));
        if (CreateOctree.pointMap.ContainsKey(corner)) { CreateOctree.pointMap.Remove(corner); }
        //-++
        corner = new Vector3Int((int)(nodeBounds.center.x - childLength), (int)(nodeBounds.center.y + childLength), (int)(nodeBounds.center.z + childLength));
        if (CreateOctree.pointMap.ContainsKey(corner)) { CreateOctree.pointMap.Remove(corner); }
        //+-+
        corner = new Vector3Int((int)(nodeBounds.center.x + childLength), (int)(nodeBounds.center.y - childLength), (int)(nodeBounds.center.z + childLength));
        if (CreateOctree.pointMap.ContainsKey(corner)) { CreateOctree.pointMap.Remove(corner); }
        //++-
        corner = new Vector3Int((int)(nodeBounds.center.x + childLength), (int)(nodeBounds.center.y + childLength), (int)(nodeBounds.center.z - childLength));
        if (CreateOctree.pointMap.ContainsKey(corner)) { CreateOctree.pointMap.Remove(corner); }
        //--+
        corner = new Vector3Int((int)(nodeBounds.center.x - childLength), (int)(nodeBounds.center.y - childLength), (int)(nodeBounds.center.z + childLength));
        if (CreateOctree.pointMap.ContainsKey(corner)) { CreateOctree.pointMap.Remove(corner); }
        //-+-
        corner = new Vector3Int((int)(nodeBounds.center.x - childLength), (int)(nodeBounds.center.y + childLength), (int)(nodeBounds.center.z - childLength));
        if (CreateOctree.pointMap.ContainsKey(corner)) { CreateOctree.pointMap.Remove(corner); }
        //+--
        corner = new Vector3Int((int)(nodeBounds.center.x + childLength), (int)(nodeBounds.center.y - childLength), (int)(nodeBounds.center.z - childLength));
        if (CreateOctree.pointMap.ContainsKey(corner)) { CreateOctree.pointMap.Remove(corner); }
        //---
        corner = new Vector3Int((int)(nodeBounds.center.x - childLength), (int)(nodeBounds.center.y - childLength), (int)(nodeBounds.center.z - childLength));
        if (CreateOctree.pointMap.ContainsKey(corner)) { CreateOctree.pointMap.Remove(corner); }
        /*    
        if (children != null)
            {
            for (int i = 0; i < 8; i++)
            {
                if (children[i] != null)
                {
                    children[i].AddCorners();
                }
            }
        }*/
    }

    public void Draw() 
    {
        if (isTotallyOccupied)
        {
            Gizmos.color = new Color(1, 0, 0);
        }else{ 
            Gizmos.color = new Color(0, 1, 0); 
        }
        Gizmos.DrawWireCube(nodeBounds.center, nodeBounds.size);
        if(children != null) 
        {
            for (int i = 0; i < 8; i++) 
            {
                if (children[i] != null) 
                {
                    children[i].Draw();
                }
            }
        }
    }
    /*
    public Vector3 FindObjectPos(GameObject go)
    {
        for (int i = 0; i < 8; i++)
        {
            if (childBounds[i].Contains(go.GetComponent<Transform>().position))
            {
                if (children[i].children != null) 
                {
                    return children[i].FindObjectPos(go);
                }
                else 
                {
                    return nodeBounds.center;
                }
            }
        }
        return Vector3.zero;
    }
    
    public OctreeNode FindObjectNode(GameObject go)
    {
        for (int i = 0; i < 8; i++)
        {
            if (childBounds[i].Contains(go.GetComponent<Transform>().position))
            {
                if (children[i].children != null)
                {
                    return children[i].FindObjectNode(go);
                }
                else
                {
                    return children[i];
                }
            }
        }
        return children[0];
    }
    */
}

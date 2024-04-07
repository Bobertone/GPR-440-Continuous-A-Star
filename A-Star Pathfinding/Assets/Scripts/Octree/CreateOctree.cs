using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
public class Point
{
    Vector3Int pos;
    //public List<Point> neighbors;
    public Dictionary<Point, float> neighborMap;

    public Point()
    {
        neighborMap = new Dictionary<Point, float>();
        //neighbors = new List<Point>();
    }
}

public class CreateOctree : MonoBehaviour
{
    public GameObject[] worldObjects;
    public int nodeMinSize = 5;
    Octree otree;
    public static Dictionary<Vector3Int, Point> pointMap;
    public Collider[] collisions;
    public LayerMask worldObjMask;
    [SerializeField] int neighborCount;

    void Start()
    {
        otree = new Octree();
        pointMap = new Dictionary<Vector3Int, Point>();
        otree.CreateOctree(worldObjects, nodeMinSize, worldObjMask);
        //otree.rootNode.AddCorners();
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying) 
        {
            otree.rootNode.Draw();
            Draw();
        }
    }

    public void Draw() 
    {
        Gizmos.color = new Color(0, 0, 1);
        foreach (var pos in pointMap.Keys)
        {
            Gizmos.DrawSphere(pos, .05f* nodeMinSize);
        }
    }

    public void FindNeighbors() 
    {
        //corner = key value pair <vect3int(position), Point>
        foreach (var corner in pointMap)
        {
            //other = key value pair <vect3int(position), Point>
            foreach (var other in pointMap)
            {
                //make sure to not compare the same two points
                if(corner.Key != other.Key) 
                {
                    //if the neighbor list is not full yet, fill it
                    if (corner.Value.neighborMap.Count < neighborCount)
                    {
                        corner.Value.neighborMap.Add(other.Value, Vector3.Distance(corner.Key,other.Key));
                    }
                    else 
                    {
                        //compare the potential neighbor (other) against the existing neighbors
                        //neighbor = key value pair <Point, float(distance)>
                        foreach (var neighbor in corner.Value.neighborMap)
                        {
                            //compare distances, keep the closest points as neighbors
                            if (neighbor.Value > Vector3.Distance(corner.Key, other.Key)) 
                            {
                                corner.Value.neighborMap.Remove(neighbor.Key);
                                corner.Value.neighborMap.Add(other.Value, Vector3.Distance(corner.Key, other.Key));
                            }
                        }
                    }
                }
            }
        }
        //make sure neighbor pairs are mutual
        //corner = key value pair <vect3int(position), Point>
        foreach (var corner in pointMap)
        {
            foreach (var neighbor in corner.Value.neighborMap)
            {
                if (!neighbor.Key.neighborMap.ContainsKey(corner.Value))
                {
                    neighbor.Key.neighborMap.Add(corner.Value, neighbor.Value);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class Point
{
    public Vector3Int pos;
    //neighborMap is a dictionary of neighboring points and their distance from this point
    public Dictionary<Point, float> neighborMap;
    public float distanceToGoal;
    public float distanceTraveled;

    public Point(Vector3 position)
    {
        this.pos = new Vector3Int((int)position.x, (int)position.y, (int)position.z);
        this.neighborMap = new Dictionary<Point, float>();
    }
}

public class CreateOctree : MonoBehaviour
{
    public GameObject[] worldObjects;
    public int nodeMinSize = 5;
    Octree otree;
    public static Dictionary<Vector3Int, Point> pointMap;
    public LayerMask worldObjLayer;
    [SerializeField] int neighborCount;
    [SerializeField] GameObject start;
    [SerializeField] GameObject goal;
    public Point startPoint;
    public Point endPoint;

    void Start()
    {
        otree = new Octree();
        pointMap = new Dictionary<Vector3Int, Point>();
        StartCoroutine(Main());
    }

    IEnumerator Main() 
    {
        otree.CreateOctree(worldObjects, nodeMinSize, worldObjLayer);
        yield return new WaitForSeconds(.25f);

        otree.rootNode.AddCorners(worldObjLayer);
        AddPathEnds();
        FindNeighbors();
        GetGoalDistance(goal.transform.position);
        yield return new WaitForSeconds(.25f);

        start.GetComponent<MoveAgent>().AStarSearch(startPoint, endPoint);
        start.GetComponent<MoveAgent>().FollowPath();
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
        Gizmos.color = new Color(1, 1, 0);
        foreach (var pos in pointMap.Keys)
        {
            Gizmos.DrawSphere(pos, .05f* nodeMinSize);
        }
        start.GetComponent<MoveAgent>().Draw();
    }

    public void GetGoalDistance(Vector3 goalPos)
    {
        //corner = key value pair <vect3int(position), Point>
        foreach (var corner in pointMap) 
        {
            corner.Value.distanceToGoal = Vector3.Distance(corner.Key, goalPos);
        }
    }

    public void AddPathEnds() 
    {
        startPoint = new Point(new Vector3Int((int)start.transform.position.x, (int)start.transform.position.y, (int)start.transform.position.z));
        endPoint = new Point(new Vector3Int((int)goal.transform.position.x, (int)goal.transform.position.y, (int)goal.transform.position.z));
        pointMap.Add(startPoint.pos, startPoint);
        pointMap.Add(endPoint.pos, endPoint);
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
                if (corner.Key != other.Key)
                {
                    if (!Physics.Linecast(corner.Key, other.Key, worldObjLayer))
                    {
                        //if the neighbor list is not full yet, fill it
                        if (corner.Value.neighborMap.Count < neighborCount)
                        {
                            corner.Value.neighborMap.Add(other.Value, Vector3.Distance(corner.Key, other.Key));
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
                                    break;
                                }
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
//build priority queue from start pos
//for each neighbor, check if its blocked or already visited
//if neither, add to queue

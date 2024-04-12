using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ESarkis;
using static UnityEditor.FilePathAttribute;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
public class MoveAgent : MonoBehaviour
{
    public PriorityQueue<Point> frontier;
    //Dictionary<Point, bool> visitedPoints;
    public Dictionary<Point, Point> cameFrom;
    public Dictionary<Point, float> costSoFar;
    List<Point> path;

    [SerializeField] float speed;
    bool shouldMove;
    [SerializeField] int pathIndex;

    //https://www.redblobgames.com/pathfinding/a-star/implementation.html#csharp

    void Awake()
    {
        frontier = new PriorityQueue<Point>();
        cameFrom = new Dictionary<Point, Point>();
        costSoFar = new Dictionary<Point, float>();
        path = new List<Point>();

        shouldMove = false;
        pathIndex = 0;
    }

    void FixedUpdate()
    {
        if (shouldMove) 
        {
            FollowPath();
        }
    }

    public void AStarSearch(Point start, Point goal)
    {
        frontier.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Point current = frontier.Dequeue();

            if (current.Equals(goal))
            {
                shouldMove = true;
                Point pathPoint = current;
                path.Insert(0, pathPoint);
                while (pathPoint != start) 
                {
                    pathPoint = cameFrom[pathPoint];
                    path.Insert(0, cameFrom[pathPoint]);
                }

                break;
            }
            //next = key value pair <Point, float(distance)>
            foreach (var next in current.neighborMap)
            {
                //new cost = cost so far + the distance to travel
                float newCost = costSoFar[current] + next.Value;
                if (!costSoFar.ContainsKey(next.Key) || newCost < costSoFar[next.Key])
                {
                    costSoFar[next.Key] = newCost;
                    //estimated total cost(priority) = newCostSoFar(cost so far + the distance to travel) + heuristic(distanceToGoal)
                    float priority = newCost + next.Key.distanceToGoal;
                    frontier.Enqueue(next.Key, priority);
                    cameFrom[next.Key] = current;
                }
            }
        }
    }

    public void FollowPath()
    {
        transform.position = Vector3.MoveTowards(transform.position, path[pathIndex].pos, speed);
        // Check if the position of the agent and goal are approximately equal.
        if (Vector3.Distance(transform.position, path[pathIndex].pos) < 0.01f)
        {
            if (pathIndex == path.Count - 1) 
            {
                transform.position = path[0].pos;
                pathIndex = -1;
            }
            pathIndex++;
        }
    }

    public void Draw() 
    {
        Gizmos.color = new Color(0, 1, 1);
        foreach (var line in cameFrom) 
        {
            Gizmos.DrawLine(line.Key.pos, line.Value.pos);
        }

        Gizmos.color = new Color(0, 0, 1);
        for (int i = 0; i < path.Count-1; i++)
        {
            Gizmos.DrawLine(path[i].pos, path[i+1].pos);
        }

    }



}

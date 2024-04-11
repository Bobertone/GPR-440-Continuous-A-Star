using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ESarkis;
using static UnityEditor.FilePathAttribute;
public class MoveAgent : MonoBehaviour
{
    public PriorityQueue<Point> frontier;
    //Dictionary<Point, bool> visitedPoints;

    public Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
    public Dictionary<Point, double> costSoFar = new Dictionary<Point, double>();

    //https://www.redblobgames.com/pathfinding/a-star/implementation.html#csharp
    public void AStarSearch(Point start, Point goal)
    {
        frontier.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.Equals(goal))
            {
                break;
            }
            //next = key value pair <Point, float(distance)>
            foreach (var next in current.neighborMap)
            {
                //new cost = cost so far + the distance to travel
                double newCost = costSoFar[current] + next.Value;
                if (!costSoFar.ContainsKey(next.Key) || newCost < costSoFar[next.Key])
                {
                    costSoFar[next.Key] = newCost;
                    //estimated total cost(priority) = newCostSoFar(cost so far + the distance to travel) + heuristic(distanceToGoal)
                    double priority = newCost + next.Key.distanceToGoal;
                    frontier.Enqueue(next.Key, priority);
                    cameFrom[next.Key] = current;
                }
            }
        }
    }
    

}

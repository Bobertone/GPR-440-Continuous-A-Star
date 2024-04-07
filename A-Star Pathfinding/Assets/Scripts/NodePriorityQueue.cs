using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodePriorityQueue : MonoBehaviour
{
    //todo: priority queue
    //store a list of octree nodes
    //function that arranges the list in order of accumulated cost

    //put the neighbors of a given tile in the frontier queue

    Queue<GameObject> frontier = new Queue<GameObject>();
    [SerializeField] GameObject startPosition;
    [SerializeField] GameObject endPosition;
    [SerializeField] GameObject octree;
    /*
    void EnqueueNullNeighbors(int x, int y)
    {
        if (x + 1 < gridWidth)
        {
            if (!grid[x + 1][y].GetComponent<TileData>().visited)
            {
                frontier.Enqueue(grid[x + 1][y]);
                grid[x + 1][y].GetComponent<TileData>().visited = true;
            }
        }
        if (x - 1 >= 0)
        {
            if (!grid[x - 1][y].GetComponent<TileData>().visited)
            {
                frontier.Enqueue(grid[x - 1][y]);
                grid[x - 1][y].GetComponent<TileData>().visited = true;
            }
        }
        if (y + 1 < gridHeight)
        {
            if (!grid[x][y + 1].GetComponent<TileData>().visited)
            {
                frontier.Enqueue(grid[x][y + 1]);
                grid[x][y + 1].GetComponent<TileData>().visited = true;
            }
        }
        if (y - 1 >= 0)
        {
            if (!grid[x][y - 1].GetComponent<TileData>().visited)
            {
                frontier.Enqueue(grid[x][y - 1]);
                grid[x][y - 1].GetComponent<TileData>().visited = true;
            }
        }
    }
    */

    //the body of the wave function collapse code
    void WaveFunctionCollapseGeneration()
    {
        //find the start node
        //OctreeNode startNode = octree.GetComponent<Octree>().rootNode.FindObjectNode(startPosition);
        //startNode.visited = true;
        /*
        frontier.Enqueue(grid[randomX][randomY]);

        while (frontier.Count != 0) //loop through all the tiles, using a frontier akin to a breadth first search
        {
            currentTile = frontier.Dequeue();
            chosenTile = currentTile.GetComponent<TileData>().ChooseRandomPossibleTile();
            if (!firstTileFinished) { Debug.Log("Chosen Tile: " + chosenTile); }
            currentTile.GetComponent<SpriteRenderer>().sprite = currentTileset[chosenTile];
            UpdateNeighbors(chosenTile, (int)currentTile.transform.position.x, (int)currentTile.transform.position.y);
            EnqueueNullNeighbors((int)currentTile.transform.position.x, (int)currentTile.transform.position.y);
            firstTileFinished = true;
        }
        */
    }
}
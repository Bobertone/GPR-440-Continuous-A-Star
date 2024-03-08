using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class SpatialHashing : MonoBehaviour
{
    [SerializeField] float bucketSize = 1.0f;

    // a bucket holds all the data in a specific quantized space
    HashSet<GameObject> bucket = new HashSet<GameObject>();

    // a dictionary of all the buckets in our world
    Dictionary<Vector2, HashSet<GameObject>> map = new Dictionary<Vector2, HashSet<GameObject>>();

    // a set of all game objects for faster global world iteration and cleanup
    HashSet<GameObject> worldObjects = new HashSet<GameObject>();

    Vector2Int Quantize(float cellSize, Vector2 pos){
        return new Vector2Int(
        (int)(Mathf.Floor(pos.x + cellSize / 2) / cellSize),
        (int)(Mathf.Floor(pos.y + cellSize/2) / cellSize));
    }
}

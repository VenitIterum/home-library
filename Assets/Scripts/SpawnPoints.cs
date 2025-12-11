using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField]
    public List<PointData> SpawnPointsList;
}

[Serializable]
public class PointData
{
    public GameObject point;
    public int booksCount;
}
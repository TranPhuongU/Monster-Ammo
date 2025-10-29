using System.Collections.Generic;
using UnityEngine;

public class PathProvider : MonoBehaviour
{
    public List<Transform> pathPoints;

    public List<Transform> GetPathPoints()
    {
        return pathPoints;
    }
}

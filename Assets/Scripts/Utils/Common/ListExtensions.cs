using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T TakeRandom<T>(this List<T> list)
        => list[Random.Range(0, list.Count)];
}

using System.Collections.Generic;
using UnityEngine;

public class GridUnit
{
    public bool Occupied { get; private set; }

    public void Occupy()
    {
        Occupied = true;
    }
}

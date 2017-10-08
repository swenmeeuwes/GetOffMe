using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    bool Enabled { get; set; }
    void Spawn();
}

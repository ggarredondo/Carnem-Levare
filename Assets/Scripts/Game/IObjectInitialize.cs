using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectInitialize
{
    public void Initialize(ref GameObject player, ref GameObject enemy);
}

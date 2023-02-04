using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlowable
{
    public float slowFactor { get; set; }
    public bool trapped { get; set; }
    public void SlowDown(float factor);
    public void Recover();
}

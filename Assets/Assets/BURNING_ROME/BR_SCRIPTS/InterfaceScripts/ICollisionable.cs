using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionable
{
    public void OnCollisionWith(ICollisionable collisionable);
}

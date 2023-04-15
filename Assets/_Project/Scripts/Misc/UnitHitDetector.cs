using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHitDetector : MonoBehaviour
{
    Befana parent;
    public void Init(Befana _parent)
    {
        parent = _parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        var santa = other.GetComponentInParent<Santa>();
        if (santa != null)
        {
            parent.UnitHit(santa);
        }
    }
}

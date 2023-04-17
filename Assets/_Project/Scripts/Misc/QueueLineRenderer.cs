using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueLineRenderer : PoolObjectBase
{
    LineRenderer lineRenderer;

    public void SetColor(Color color)
    {
        lineRenderer.material.color = color;
    }

    public override void OnSetup()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void OnRetrieve()
    {
        lineRenderer.positionCount = 0;
    }

    public LineRenderer GetLineRenderer()
    {
        return lineRenderer;
    }

}

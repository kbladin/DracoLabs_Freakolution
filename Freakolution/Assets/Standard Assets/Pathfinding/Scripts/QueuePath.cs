using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class QueuePath 
{
    public Vector3 startPos;
    public Vector3 endPos;
    public Action<List<Vector3>> storeRef;
	public bool isBestEffort = false;

    public QueuePath(Vector3 sPos, Vector3 ePos, Action<List<Vector3>> theRefMethod, bool bestEffort)
    {
        startPos = sPos;
        endPos = ePos;
        storeRef = theRefMethod;
		isBestEffort = bestEffort;
    }

	public QueuePath(Vector3 sPos, Vector3 ePos, Action<List<Vector3>> theRefMethod)
	{
		startPos = sPos;
		endPos = ePos;
		storeRef = theRefMethod;
	}
}

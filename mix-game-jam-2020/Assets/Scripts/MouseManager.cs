using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private VertexType mouseType = VertexType.EMPTY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public VertexType GetMouseType()
    {
        return mouseType;
    }

    public void SetMouseType(VertexType type)
    {
        mouseType = type;
    }

    public void SetMouseTypeToRoad()
    {
        SetMouseType(VertexType.ROAD);
    }

    public void SetMouseTypeToStructure()
    {
        SetMouseType(VertexType.STRUCTURE);
    }

    public void SetMouseTypeToEmpty()
    {
        SetMouseType(VertexType.EMPTY);
    }

    public void SetMouseTypeToTownHall()
    {
        SetMouseType(VertexType.TOWNHALL);
    }
}

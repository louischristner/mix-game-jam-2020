using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public Sprite emptyCell = null;
    public Sprite townHallCell = null;
    public List<Sprite> structureCells = new List<Sprite>();
    public List<Sprite> roadCells = new List<Sprite>();

    public MouseManager mouseManager = null;
    public MapGenerator mapGenerator = null;

    private int x;
    private int y;
    private VertexType prevVertexType = VertexType.NONE;
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        x = (int) transform.position.x;
        y = (int) transform.position.y;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (emptyCell != null) {
            spriteRenderer.sprite = emptyCell;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mapGenerator.GetGrid()[x, y]._type == VertexType.EMPTY) {
            spriteRenderer.sprite = emptyCell;
        } else if (mapGenerator.GetGrid()[x, y]._type == VertexType.STRUCTURE) {
            if (prevVertexType != VertexType.STRUCTURE) {
                int index = Random.Range(0, structureCells.Count);
                spriteRenderer.sprite = structureCells[index];
            }
        } else if (mapGenerator.GetGrid()[x, y]._type == VertexType.TOWNHALL) {
            spriteRenderer.sprite = townHallCell;
        } else if (mapGenerator.GetGrid()[x, y]._type == VertexType.ROAD) {
            VertexType[] neighbours = mapGenerator.GetGrid().GetAllAdjacentVertexType(x, y);
            int countRoads = 0;

            foreach (VertexType type in neighbours) {
                if (type == VertexType.ROAD) {
                    countRoads += 1;
                }
            }

            if (countRoads == 0) {
                spriteRenderer.sprite = roadCells[11];
            } else if (countRoads == 1) {
                if (neighbours[0] == VertexType.ROAD) {
                    spriteRenderer.sprite = roadCells[15];
                } else if (neighbours[1] == VertexType.ROAD) {
                    spriteRenderer.sprite = roadCells[14];
                } else if (neighbours[2] == VertexType.ROAD) {
                    spriteRenderer.sprite = roadCells[13];
                } else {
                    spriteRenderer.sprite = roadCells[12];
                }
            } else if (countRoads == 2) {
                if (neighbours[0] == VertexType.ROAD && neighbours[1] == VertexType.ROAD) {
                    spriteRenderer.sprite = roadCells[5];
                } else if (neighbours[0] == VertexType.ROAD && neighbours[2] == VertexType.ROAD) {
                    spriteRenderer.sprite = roadCells[1];
                } else if (neighbours[0] == VertexType.ROAD && neighbours[3] == VertexType.ROAD) {
                    spriteRenderer.sprite = roadCells[3];
                } else if (neighbours[1] == VertexType.ROAD && neighbours[2] == VertexType.ROAD) {
                    spriteRenderer.sprite = roadCells[4];
                } else if (neighbours[1] == VertexType.ROAD && neighbours[3] == VertexType.ROAD) {
                    spriteRenderer.sprite = roadCells[0];
                } else {
                    spriteRenderer.sprite = roadCells[2];
                }
            } else if (countRoads == 3) {
                if (neighbours[3] != VertexType.ROAD) {
                    spriteRenderer.sprite = roadCells[7];
                } else if (neighbours[0] != VertexType.ROAD) {
                    spriteRenderer.sprite = roadCells[9];
                } else if (neighbours[1] != VertexType.ROAD) {
                    spriteRenderer.sprite = roadCells[6];
                } else {
                    spriteRenderer.sprite = roadCells[8];
                }
            } else if (countRoads == 4) {
                spriteRenderer.sprite = roadCells[10];
            } else {
                spriteRenderer.sprite = emptyCell;
            }
        }


        // Set prevVertexType
        if (mapGenerator.GetGrid()[x, y]._type != prevVertexType) {
            prevVertexType = mapGenerator.GetGrid()[x, y]._type;
        }
    }

    void OnMouseDown()
    {
        if (mouseManager.GetMouseType() == VertexType.EMPTY) {
            mapGenerator.GetGrid()[x, y]._type = mouseManager.GetMouseType();
        } else if (mapGenerator.GetGrid()[x, y]._type == VertexType.EMPTY) {
            mapGenerator.GetGrid()[x, y]._type = mouseManager.GetMouseType();
        }
    }
}

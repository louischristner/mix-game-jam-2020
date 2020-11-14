using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VertexType
{
    EMPTY,
    TOWNHALL,
    STRUCTURE,
    ROAD,
    NONE
};

public class Vertex
{
    public VertexType _type { get; set; }
    public Vector2 _position { get; set; }

    public Vertex(VertexType type, Vector2 position)
    {
        _type = type;
        _position = position;
    }
}

public class Grid
{
    public int _width { get; set; }
    public int _height { get; set; }
    private Vertex[,] _grid;

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;

        _grid = new Vertex[width, height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                _grid[x, y] = new Vertex(VertexType.EMPTY, new Vector2(x, y));
            }
        }
    }

    public Vertex this[int x, int y]
    {
        get {
            return _grid[x, y];
        }

        set {
            _grid[x, y] = value;
        }
    }

    public VertexType[] GetAllAdjacentVertexType(int x, int y)
    {
        VertexType[] neighbours = {
            (x > 0) ? _grid[x - 1, y]._type : VertexType.NONE,
            (y < _height - 1) ? _grid[x, y + 1]._type : VertexType.NONE,
            (x < _width - 1) ? _grid[x + 1, y]._type : VertexType.NONE,
            (y > 0) ? _grid[x, y - 1]._type : VertexType.NONE
        };

        return neighbours;
    }
}

public class MapGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject cell = null;

    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(width, height);
        for (int y = 0; y < grid._height; y++) {
            for (int x = 0; x < grid._width; x++) {
                GameObject newCell = Instantiate(cell, grid[x, y]._position, Quaternion.identity);
                newCell.GetComponent<CellManager>().mouseManager = this.GetComponent<MouseManager>();
                newCell.GetComponent<CellManager>().mapGenerator = this.GetComponent<MapGenerator>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Grid GetGrid()
    {
        return grid;
    }
}

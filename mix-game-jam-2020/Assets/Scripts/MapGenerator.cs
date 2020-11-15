using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

public class MusicNotes
{
    public List<float> notesInterval { get; set; }

    public MusicNotes(List<float> notesInterval)
    {
        this.notesInterval = notesInterval;
    }
}

public class Music
{
    public float bps { get; set; }
    public MusicNotes[] notes { get; set; }

    public Music(float bps, MusicNotes[] notes)
    {
        this.bps = bps;
        this.notes = notes;
    }
}

public class MapGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public int money = 100;
    public GameObject cell = null;

    private int buildingCost = 0;
    private float musicLength = 0f;
    private float elapsedTime = 0f;
    private Vertex wantedVertex = null;
    private VertexType wantedVertexType = VertexType.NONE;
    private List<GameObject> cells = new List<GameObject>(){};

    private Grid grid;

    public Text moneyText;
    public GameObject rhythm;

    public GameObject blueSpawner;
    public GameObject redSpawner;
    public GameObject yellowSpawner;
    public GameObject greenSpawner;

    private Music[] musics = new Music[]{
        new Music(60f / (90f * 4), new MusicNotes[] {
            new MusicNotes(new List<float>(){0, 4, 8, 12}),
            new MusicNotes(new List<float>(){3, 6, 11, 14}),
            new MusicNotes(new List<float>(){}),
            new MusicNotes(new List<float>(){})
        }),
        new Music(60f / (100f * 4), new MusicNotes[] {
            new MusicNotes(new List<float>(){0, 8}),
            new MusicNotes(new List<float>(){4, 12}),
            new MusicNotes(new List<float>(){6, 10, 14}),
            new MusicNotes(new List<float>(){})
        }),
        new Music(60f / (60f * 4), new MusicNotes[] {
            new MusicNotes(new List<float>(){0, 3, 8, 11}),
            new MusicNotes(new List<float>(){1, 4, 9, 12}),
            new MusicNotes(new List<float>(){6, 7, 14, 15}),
            new MusicNotes(new List<float>(){})
        })
    };

    // Start is called before the first frame update
    void Start()
    {
        rhythm.SetActive(false);

        grid = new Grid(width, height);
        for (int y = 0; y < grid._height; y++) {
            for (int x = 0; x < grid._width; x++) {
                GameObject newCell = Instantiate(cell, grid[x, y]._position, Quaternion.identity);
                newCell.GetComponent<CellManager>().mouseManager = this.GetComponent<MouseManager>();
                newCell.GetComponent<CellManager>().mapGenerator = this.GetComponent<MapGenerator>();
                cells.Add(newCell);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "Money: $" + money.ToString();

        if (musicLength > 0) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= musicLength) {
                if (GameObject.Find("Arrow(Clone)") == null) {

                    if (money - buildingCost >= 0) {
                        money -= buildingCost;
                        wantedVertex._type = wantedVertexType;
                    }

                    SetActiveRhythm(false);
                    musicLength = 0f;
                    elapsedTime = 0f;
                    buildingCost = 0;
                }
            }
        }
    }

    public Grid GetGrid()
    {
        return grid;
    }

    private void SetActiveRhythm(bool active)
    {
        rhythm.SetActive(active);
        foreach (GameObject cell in cells) {
            cell.SetActive(!active);
        }
    }

    public void ReduceBuildingCost()
    {
        if (buildingCost > 0) {
            buildingCost -= 1;
        }
    }

    public void AskForBuilding(Vertex vertex, VertexType type)
    {
        if (type != VertexType.EMPTY) {

            wantedVertex = vertex;
            wantedVertexType = type;

            SetActiveRhythm(true);

            int index = Random.Range(0, musics.Length);
            blueSpawner.GetComponent<ArrowSpawner>().StartMusic(musics[index].bps, new List<float>(musics[index].notes[0].notesInterval));
            redSpawner.GetComponent<ArrowSpawner>().StartMusic(musics[index].bps, new List<float>(musics[index].notes[1].notesInterval));
            yellowSpawner.GetComponent<ArrowSpawner>().StartMusic(musics[index].bps, new List<float>(musics[index].notes[2].notesInterval));
            greenSpawner.GetComponent<ArrowSpawner>().StartMusic(musics[index].bps, new List<float>(musics[index].notes[3].notesInterval));

            buildingCost = musics[index].notes[0].notesInterval.Count
                + musics[index].notes[1].notesInterval.Count
                + musics[index].notes[2].notesInterval.Count
                + musics[index].notes[3].notesInterval.Count;

            for (int i = 0; i < 4; i++) {
                if (musics[index].notes[i].notesInterval.Count > 0) {
                    if (musics[index].notes[i].notesInterval[musics[index].notes[i].notesInterval.Count - 1] > musicLength) {
                        musicLength = musics[index].notes[i].notesInterval[musics[index].notes[i].notesInterval.Count - 1] * musics[index].bps;
                    }
                }
            }
        }
    }
}

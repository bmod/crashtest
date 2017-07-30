using System.Collections.Generic;
using UnityEngine;


internal struct SpawnPoint
{
    public int Index;
    public Vector2 Pt;
    public GameObject instance;

    public void Destroy()
    {
        GameObject.DestroyImmediate(instance);
    }
}


internal struct Cell
{
    public Rect Rect;
    public SpawnPoint[] SpawnPoints;
    public int Hash;
    public int X { get; set; }
    public int Y { get; set; }

    public void Destroy()
    {
        foreach (var spawnPoint in SpawnPoints)
            spawnPoint.Destroy();
    }
}

internal struct IntBounds
{
    public int XMin, YMin, XMax, YMax;

    public static IntBounds FromRect(Rect rect, float CellSize)
    {
        return new IntBounds
        {
            XMin = Mathf.FloorToInt(rect.xMin / CellSize),
            XMax = Mathf.CeilToInt(rect.xMax / CellSize),
            YMin = Mathf.FloorToInt(rect.yMin / CellSize),
            YMax = Mathf.CeilToInt(rect.yMax / CellSize)
        };
    }

    public static bool operator ==(IntBounds a, IntBounds b)
    {
        return a.XMin == b.XMin &&
               a.XMax == b.XMax &&
               a.YMin == b.YMin &&
               a.YMax == b.YMax;
    }

    public static bool operator !=(IntBounds a, IntBounds b)
    {
        return !(a == b);
    }

    public bool Contains(int x, int y)
    {
        return x >= XMin &&
               x < XMax &&
               y >= YMin &&
               y < YMax;
    }

    public IntBounds Union(IntBounds other)
    {
        return new IntBounds
        {
            XMin = Mathf.Min(XMin, other.XMin),
            XMax = Mathf.Max(XMax, other.XMax),
            YMin = Mathf.Min(YMin, other.YMin),
            YMax = Mathf.Max(YMax, other.YMax)
        };
    }

    public string ToString()
    {
        return "min: " + XMin + ", " + YMin + ", max: " + XMax + ", " + YMax;
    }
}

public class RandomSpawnArea : MonoBehaviour
{
    public GameObject[] gameObjects;
    public float LiveAreaSizeMultiplier = 2;
    public float CellSize = 10;
    public int minSpawnPointsInCell = 0;
    public int maxSpawnPointsInCell = 3;
    public int seed = 100;
    public bool DestroyOutOfBounds = true;

    private Vector2 _anchor;
    private Rect _liveArea;
    private HashFunction _rand;

    private IntBounds _cellBounds;
    private readonly Dictionary<int, Cell> _cells = new Dictionary<int, Cell>();

    private void Awake()
    {
        _rand = new XXHash(seed);
    }

    private void Update()
    {
        _anchor = Camera.main.transform.position;

        var h = Camera.main.orthographicSize * 2;
        _liveArea.height = h * LiveAreaSizeMultiplier;
        _liveArea.width = Camera.main.aspect * h * LiveAreaSizeMultiplier;

        // Center on camera
        _liveArea.position = new Vector2(_anchor.x - _liveArea.width / 2, _anchor.y - _liveArea.height / 2);
        UpdateCells();
    }


    private SpawnPoint[] PointsInCell(int x, int y)
    {
        var pointCount = 0;
        if (minSpawnPointsInCell == maxSpawnPointsInCell)
            pointCount = minSpawnPointsInCell;
        else if (maxSpawnPointsInCell < minSpawnPointsInCell)
            pointCount = 1;
        else
            pointCount = _rand.Range(minSpawnPointsInCell, maxSpawnPointsInCell, x, y);
        
        var pts = new SpawnPoint[pointCount];
        for (var i = 0; i < pointCount; i++)
        {
            pts[i] = CreateSpawnPoint(x, y, i);
        }
        return pts;
    }

    private SpawnPoint CreateSpawnPoint(int x, int y, int i)
    {
        var go = gameObjects[_rand.Range(0, gameObjects.Length, x + 300, y)];
        var sp = new SpawnPoint();
        sp.Pt = new Vector2(_rand.Value(x, y + 100 * i), _rand.Value(x + 200 * i, y));
        sp.instance = Instantiate(go);
        var pos = sp.Pt * CellSize + new Vector2(x, y) * CellSize;
        var rot = _rand.Range(0, 3, x + 30, y) * 90;
        sp.instance.transform.position = pos;
        sp.instance.transform.Rotate(Vector3.forward * rot);
        sp.Index = i;
        return sp;
    }

    private void CreateCell(int x, int y)
    {
        var hash = Hash(x, y);

        if (_cells.ContainsKey(hash))
            return;

        _cells[hash] = new Cell
        {
            Hash = hash,
            X = x,
            Y = y,
            Rect = new Rect(x * CellSize, y * CellSize, CellSize, CellSize),
            SpawnPoints = PointsInCell(x, y)
        };
    }


    private void DestroyCell(int hash)
    {
        if (!DestroyOutOfBounds)
            return;
        if (!_cells.ContainsKey(hash)) return;
        _cells[hash].Destroy();
        _cells.Remove(hash);
    }


    public int Hash(int x, int y)
    {
        if (x == 1) x = 1000000;
        unchecked // integer overflows are accepted here
        {
            int h = 0;
            h = (h * 397) ^ x;
            h = (h * 397) ^ y;
            return h;
        }
    }

    private void UpdateCells()
    {
        var newBounds = IntBounds.FromRect(_liveArea, CellSize);
        if (_cellBounds == newBounds)
            return; // Same cell bounds, do nothing

        var union = _cellBounds.Union(newBounds);

        for (var y = union.YMin; y <= union.YMax; y++)
        {
            for (var x = union.XMin; x <= union.XMax; x++)
            {
                if (_cellBounds.Contains(x, y) && newBounds.Contains(x, y))
                    continue; // Cell is in both rectangles, no need to update

                if (newBounds.Contains(x, y))
                {
                    // New cell, create
                    CreateCell(x, y);
                }
//                if (!newBounds.Contains(x, y) && _cellBounds.Contains(x, y))
//                {
//                    // Old cell, destroy
//                    DestroyCell(x, y);
//                }
            }
        }
        

        // Cleanup old cells, can be optimized (see above)
        var destroy = new List<int>();
        foreach (var cell in _cells.Values)
            if (!newBounds.Contains(cell.X, cell.Y))
                destroy.Add(cell.Hash);
        foreach (var h in destroy)
            DestroyCell(h);

        _cellBounds = newBounds;
    }

    private void OnDrawGizmos()
    {
//        var xf = SceneView.lastActiveSceneView.camera.transform;
//        _anchor = new Vector2(xf.position.x, xf.position.y);

        Gizmos.color = Color.yellow;
        DrawRect(_liveArea);

        foreach (var c in _cells.Values)
        {
            Gizmos.color = Color.blue;
            DrawRect(c.Rect);
            Gizmos.color = Color.green;

            var offset = new Vector2(c.Rect.x, c.Rect.y);
            foreach (var town in c.SpawnPoints)
            {
                var p = offset + town.Pt * CellSize;
                Gizmos.DrawWireSphere(p, 0.2f);
            }
        }
    }

    private void DrawRect(Rect r)
    {
        Vector3[] pts =
        {
            new Vector3(r.xMin, r.yMin, 0),
            new Vector3(r.xMax, r.yMin, 0),
            new Vector3(r.xMax, r.yMax, 0),
            new Vector3(r.xMin, r.yMax, 0),
        };
        Gizmos.DrawLine(pts[0], pts[1]);
        Gizmos.DrawLine(pts[1], pts[2]);
        Gizmos.DrawLine(pts[2], pts[3]);
        Gizmos.DrawLine(pts[3], pts[0]);
    }
}
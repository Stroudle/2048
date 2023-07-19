using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Tile _tilePrefab;

    private Grid2048 _grid;
    private List<Tile> _tiles;

    private const int StartTiles = 2;

    private void Awake()
    {
        _grid = GetComponentInChildren<Grid2048>();
        _tiles = new List<Tile>(16);
    }

    private void Start() 
    {
        SpawnTiles();
    }

    private void OnEnable()
    {
        InputHandler.OnMovementInput += OnMovementInputHandler;
    }

    private void OnDisable() 
    {
        InputHandler.OnMovementInput -= OnMovementInputHandler;
    }

    private void SpawnTiles()
    {
        for(int i = 0; i < StartTiles; i++)
        {
            SpawnTile();
        }
    }

    private void SpawnTile()
    {
        Tile tile = Instantiate(_tilePrefab, _grid.transform);
        tile.Spawn(_grid.GetRandomEmptyCell());
        _tiles.Add(tile);
    }

    //modificar para input unity
    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.W))
    //    {
    //        MoveTiles(Vector2Int.up);
    //    }
    //    else if(Input.GetKeyDown(KeyCode.A))
    //    {
    //        MoveTiles(Vector2Int.left);
    //    }
    //    else if(Input.GetKeyDown(KeyCode.S))
    //    {
    //        MoveTiles(Vector2Int.down);
    //    }
    //    else if(Input.GetKeyDown(KeyCode.D))
    //    {
    //        MoveTiles(Vector2Int.right);
    //    }
    //}

    private void OnMovementInputHandler(Vector2Int input) => MoveTiles(input);

    private void MoveTiles(Vector2Int direction)
    {
        int xStart = direction.x == 1 ? 0 : (direction.x == -1 ? _grid.Width - 1 : 0);
        int xIncrement = direction.x == 1 ? 1 : (direction.x == -1 ? -1 : 1);

        int yStart = direction.y == 1 ? 0 : (direction.y == -1 ? _grid.Height - 1 : 0);
        int yIncrement = direction.y == 1 ? 1 : (direction.y == -1 ? -1 : 1);

        for(int x = xStart; x >= 0 && x < _grid.Width; x += xIncrement)
        {
            for(int y = yStart; y >= 0 && y < _grid.Height; y += yIncrement)
            {
                Cell cell = _grid.GetCell(x, y);

                if(cell.Occupied)
                {
                    MoveTile(cell.Tile, direction);
                }
            }
        }
    }

    private void MoveTile(Tile tile, Vector2Int direction)
    {
        Cell destination = null;
        Cell adjacent = _grid.GetAdjacentCell(tile.Cell, direction);

        while(adjacent != null)
        {
            if(adjacent.Occupied)
            {
                break;
            }

            destination  = adjacent;
            adjacent = _grid.GetAdjacentCell(adjacent, direction);
        }

        if(destination != null)
        {
            tile.MoveTo(destination);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using DG.DemiLib;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 center; // middle Tile of grid
    private Vector3 origin; // left-bottom Tile [0,0] of grid

    public Grid(int width, int height, float cellSize, Vector3 center)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        origin = center;
        origin.x -= cellSize * width * 0.5f;
        origin.y -= cellSize * height * 0.5f;
        
        Debug.Log($"Grid {height}x{width} is created");

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                UtilsClass.CreateWorldText($"{x},{y}",null,origin + GetWorldPosition(x,y) + new Vector3(cellSize,cellSize) * 0.5f,
                                                                                                20,Color.white,TextAnchor.MiddleCenter);
                Debug.DrawLine(origin + GetWorldPosition(x,y), origin + GetWorldPosition(x,y+1),Color.white,100f);
                Debug.DrawLine(origin + GetWorldPosition(x,y), origin + GetWorldPosition(x+1,y),Color.white,100f);
            }
        }
        
        Debug.DrawLine(origin + GetWorldPosition(width,0),origin + GetWorldPosition(width,height), Color.white,100f);
        Debug.DrawLine(origin + GetWorldPosition(0,height),origin + GetWorldPosition(width,height), Color.white,100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public Vector3 GetTilePosition(Tile tile)
    {
        return origin + GetWorldPosition(tile.x, tile.y) + new Vector3(cellSize, cellSize) * 0.5f;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);
    }

    public Tile SelectXY(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.Log($"selected outside grid");
            return new Tile(-1,-1);
        }
        Debug.Log($"selected [{x},{y}]");
        return new Tile(x, y);
    }

    public Tile SelectXY(Vector3 worldPosition)
    {
        GetXY(worldPosition,out int x, out int y);
        return SelectXY(x,y);
    }

    public struct Tile
    {
        public int x;
        public int y;

        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator!=(Tile a, Tile b)
        {
            return !(a == b);
        }

        public static bool operator ==(Tile a, Tile b)
        {
            return (a.x == b.x) && (a.y == b.y);
        }
    }
}

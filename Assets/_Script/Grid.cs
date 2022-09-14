using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using DG.DemiLib;
using UnityEngine.UI;

namespace GridMap
{
    public class Grid<TGridObject>
    {
        private int width { get; }
        private int height { get; }
        private float cellSize { get; }
        private Vector3 origin; // left-bottom Tile [0,0] of grid
        private TGridObject[,] gridArray;

        public Grid(Vector3 center = default, int width = 1, int height = 1, float cellSize = 1)
        {
            origin.x = center.x - cellSize * width * 0.5f;
            origin.y = center.y - cellSize * height * 0.5f;

            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            gridArray = new TGridObject[width, height];

            Debug.Log($"Grid {height}x{width} is created");

            DrawGrid();
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize;
        }

        public Vector3 GetTilePosition(int x, int y)
        {
            return origin + GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f;
        }

        public Vector3 GetTilePosition(Tile tile)
        {
            if (tile == null) return default;
            return GetTilePosition(tile.x, tile.y);
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
                return null;
            }

            return new Tile(x, y);
        }

        public Tile SelectXY(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return SelectXY(x, y);
        }

        public void SetValue(int x, int y, TGridObject value)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }

            gridArray[x, y] = value;
        }
        
        public void SetValue(Tile tile, TGridObject value)
        {
            if (tile == null) return;
            SetValue(tile.x, tile.y, value);
        }

        public TGridObject GetValue(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return default;
            }

            return gridArray[x, y];
        }
        
        public TGridObject GetValue(Tile tile)
        {
            if (tile == null) return default;
            return GetValue(tile.x, tile.y);
        }

        public void SwapValue(Tile tile1, Tile tile2)
        {
            if (tile1 == null || tile2 == null) return;
            var tmpData = GetValue(tile1);
            SetValue(tile1,GetValue(tile2));
            SetValue(tile2,tmpData);
        }

        #region Unimportant methods, just for debugging

        private void DrawGrid(bool drawOutlines = true, bool drawIndexes = true)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (drawIndexes)
                        UtilsClass.CreateWorldText($"{x},{y}", null, GetTilePosition(x, y),
                            20, Color.yellow, TextAnchor.MiddleCenter);

                    if (drawOutlines)
                    {
                        Debug.DrawLine(origin + GetWorldPosition(x, y), origin + GetWorldPosition(x, y + 1),
                            Color.white, 100f);
                        Debug.DrawLine(origin + GetWorldPosition(x, y), origin + GetWorldPosition(x + 1, y),
                            Color.white, 100f);
                    }
                }
            }

            if (drawOutlines)
            {
                Debug.DrawLine(origin + GetWorldPosition(width, 0), origin + GetWorldPosition(width, height),
                    Color.white, 100f);
                Debug.DrawLine(origin + GetWorldPosition(0, height), origin + GetWorldPosition(width, height),
                    Color.white, 100f);
            }
        }
        
        public void PrintGridArray(Text txt)
        {
            string matrix = "";
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    if (gridArray[x, y] == null) matrix += "- ";
                    else matrix += "x ";
                }

                matrix += "\n";
            }
            Debug.Log(matrix);
            txt.text = matrix;
        }

        #endregion

    }

    public class Tile
    {
        public int x { get; }
        public int y { get; }

        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

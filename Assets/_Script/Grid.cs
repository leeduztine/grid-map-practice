using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;

namespace GridMap
{
    public class Grid<TGridObject>
    {
        private int width { get; }
        private int height { get; }
        private float cellSize { get; }
        private TGridObject[,] gridArray;
        
        private readonly Vector3 origin; // position of tile[0,0]'s left-bot corner

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

        private Tile nullTile = new Tile(-1,-1);

        private bool CheckValidTile(Tile tile)
        {
            if (tile.x < 0 || tile.x >= width || tile.y < 0 || tile.y >= height)
            {
                return false;
            }

            return true;
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            // Position of tile's left-bot corner
            //     ______
            //    |     |
            //   (*)----
            return new Vector3(x, y) * cellSize;
        }
        
        public Vector3 TileToWorldPosition(Tile tile)
        {
            // Position of tile's center
            //     ______
            //    | (*) |
            //    ------
            return origin + GetWorldPosition(tile.x, tile.y) + new Vector3(cellSize, cellSize) * 0.5f;
        }

        public Tile WorldPositionToTile(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
            int y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);
            
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                // Debug.Log("selected outside grid");
                return nullTile;
            }

            // Debug.Log($"selected Tile {x},{y}");
            return new Tile(x, y);
        }
        
        public void SetValue(Tile tile, TGridObject value)
        {
            if (!CheckValidTile(tile)) return;
            
            gridArray[tile.x, tile.y] = value;
        }
        
        public TGridObject GetValue(Tile tile)
        {
            if (!CheckValidTile(tile)) return default;

            return gridArray[tile.x, tile.y];
        }

        public List<Tile> GetNearTiles(Tile tile, int range)
        {
            List<Tile> tiles = new List<Tile>();
            int posX = tile.x;
            int posY = tile.y;

            for (int x = posX - range; x <= posX + range; x++)
            {
                for (int y = posY - range; y <= posY + range; y++)
                {
                    if (!CheckValidTile(new Tile(x,y)) || tile == new Tile(x,y))
                    {
                        continue;
                    }
                    
                    tiles.Add(new Tile(x,y));
                }
            }
            
            return tiles;
        }

        public void SwapValue(Tile tile1, Tile tile2)
        {
            if (!CheckValidTile(tile1) || !CheckValidTile(tile2)) return;
            
            var tmpData1 = GetValue(tile1);
            var tmpData2 = GetValue(tile2);
            SetValue(tile1,tmpData2);
            SetValue(tile2,tmpData1);
        }

        #region Unimportant methods, just for debugging

        private void DrawGrid(bool drawOutlines = true, bool drawIndexes = true)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (drawIndexes)
                        UtilsClass.CreateWorldText($"{x},{y}", null, TileToWorldPosition(new Tile(x, y)),
                            20, Color.yellow, TextAnchor.MiddleCenter,TextAlignment.Center,-999);

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
            txt.text = matrix;
        }

        public void PrintNearTiles(Tile tile, int range, Text txt)
        {
            List<Tile> tiles = GetNearTiles(tile, range);
            
            Debug.Log(tiles.Count);
            
            string matrix = "";
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tile.x == x && tile.y == y) matrix += "o ";
                    else
                    {
                        bool isNear = false;
                        tiles.ForEach(t =>
                        {
                            if (t.x == x && t.y == y) isNear = true;
                        });

                        if (isNear) matrix += "x ";
                        else matrix += "- ";
                    }
                }

                matrix += "\n";
            }
            txt.text = matrix;
        }

        #endregion

    }

    public struct Tile
    {
        public int x { get; }
        public int y { get; }

        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static bool operator ==(Tile t1, Tile t2)
        {
            return t1.x == t2.x && t1.y == t2.y;
        }

        public static bool operator !=(Tile t1, Tile t2)
        {
            return !(t1 == t2);
        }
    }
}

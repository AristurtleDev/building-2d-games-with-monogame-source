using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class TileMap
{
    private readonly Sprite[] _cells;
    public int Rows { get; }
    public int Columns { get; }
    public int TileSize { get; }

    public TileMap(int rows, int columns, int tileSize)
    {

    }


    public TileMap(int rows, int columns, int cellSize, Sprite[] cells)
    {
        Rows = rows;
        Columns = columns;
        TileSize = cellSize;
        _cells = cells;
    }

    public void SetTile(Sprite sprite, int tileID)
    {
        _cells[tileID] = sprite;
    }

    public void SetTile(Sprite sprite, int row, int column)
    {
        int tileID = row * Columns + column;
        SetTile(sprite, tileID);
    }

    public Sprite GetTile(int tileID)
    {
        return _cells[tileID];
    }

    public Sprite GetTile(int row, int column)
    {
        int tileID = row * Columns + column;
        return GetTile(tileID);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            Sprite cell = _cells[i];

            if (cell != null)
            {
                int x = i % Columns;
                int y = i / Columns;

                Vector2 position = new Vector2(x * TileSize, y * TileSize);
                cell.Draw(spriteBatch, position);
            }
        }
    }

    public static TileMap FromFile(ContentManager content, string fileName)
    {
        string filePath = Path.Combine(content.RootDirectory, fileName);

        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;

                int cellSize = int.Parse(root.Attribute("cellSize")?.Value ?? "0");
                if (cellSize <= 0)
                {
                    throw new InvalidOperationException("<TileMap> element does not contain a valid 'cellSize' attribute.");
                }

                string textureAtlasDefinition = root.Attribute("textureAtlasDefinition")?.Value ?? string.Empty;
                if (string.IsNullOrEmpty(textureAtlasDefinition))
                {
                    throw new InvalidOperationException("<TileMap> element does not contain a 'textureAtlasDefinition' attribute.");
                }

                // Load the texture atlas
                TextureAtlas atlas = TextureAtlas.FromFile(content, textureAtlasDefinition);

                // Parse the cells data
                XElement cellsElement = root.Element("Cells");
                if (cellsElement == null)
                {
                    throw new InvalidOperationException("Missing <Cells> element in the tilemap file.");
                }

                // Split the cell data into rows
                string[] rows = cellsElement.Value.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
                int rowCount = rows.Length;

                // Check that we have at least 1 row
                if (rowCount < 0)
                {
                    throw new InvalidOperationException("<Cells> element is empty.");
                }

                // Parse the first row to determine the column count
                string[] firstRowCells = rows[0].Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);
                int columnCount = firstRowCells.Length;

                // Create an array to hold the texture regions
                Sprite[] cells = new Sprite[rowCount * columnCount];

                // Process each row
                for (int y = 0; y < rowCount; y++)
                {
                    // Split the row into individual cell ids
                    string[] tileIDs = rows[y].Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);

                    // Validate that it has the expected number of columns
                    if (tileIDs.Length != columnCount)
                    {
                        throw new InvalidOperationException($"Row {y} has {tileIDs.Length} cells, but expected {columnCount}.");
                    }

                    // Process each cell in the row
                    for (int x = 0; x < columnCount; x++)
                    {
                        // Calculate the index of the cell in the array
                        int index = y * columnCount + x;

                        // Parse the tileID
                        int tileID = int.Parse(tileIDs[x]);

                        // Get the texture region from the atlas
                        TextureRegion cellRegion = atlas.GetRegion(tileID);

                        // Create a sprite with it
                        Sprite cell = new Sprite(cellRegion);

                        // Set the scale based on the ratio of region size and cell size
                        float scaleX = cellSize / (float)cellRegion.Width;
                        float scaleY = cellSize / (float)cellRegion.Height;
                        cell.Scale = new Vector2(scaleX, scaleY);

                        // Store it in the regions array at the calculated index.
                        cells[index] = cell;
                    }
                }

                return new TileMap(rowCount, columnCount, cellSize, cells);
            }
        }
    }


}

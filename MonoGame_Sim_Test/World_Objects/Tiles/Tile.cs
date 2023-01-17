using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Sim_Test.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame_Sim_Test
{
    public static class Tile_Controller
    {
        private static Dictionary<string, Texture2D> dic_Tex_Tiles;
        private static Tile[,] tiles;

        public static void Init(int Horizontal_Tiles, int Vertical_Tiles)
        {
            tiles = new Tile[Horizontal_Tiles, Vertical_Tiles];
        }

        public static void Load(ContentManager Content, int TileSize)
        {
            dic_Tex_Tiles = Content_Loader.Load_Content<Texture2D>(Content, "Textures//Tiles");
            Vector2 TileOffset = Get_Tile_Offset(tiles, TileSize);

            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    Vector2 Position = new Vector2(TileOffset.X + x * TileSize, TileOffset.Y + y * TileSize);
                    tiles[x, y] = new Tile(new Rectangle((int)Position.X, (int)Position.Y, TileSize, TileSize),
                        dic_Tex_Tiles.ElementAt(x % dic_Tex_Tiles.Count).Value,
                        Vector2.Zero,
                        Color.White);
                }
            }
        }
        //new Vector2(rectangles[0].Width / 2, rectangles[0].Height / 2),
        private static Vector2 Get_Tile_Offset(Tile[,] tiles, int TileSize)
        {
            return new Vector2(-TileSize * (tiles.GetLength(0) / 2f), -TileSize * (tiles.GetLength(1) / 2f));
        }

        public static void Draw(SpriteBatch spriteBatch, Rectangle Culling_rectangle)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    tiles[x, y].Draw(spriteBatch, Culling_rectangle);
                }
            }
        }
    }

    public class Tile : World_Object
    {
        public Tile(Rectangle rectangle, Texture2D texture, Vector2 origin, Color color, float rotation = 0f)
        {
            this.texture = texture;
            this.rectangle = new Rect(rectangle);
            this.origin = origin;
            this.color = color;
            this.rotation = rotation;

            this.rectangle.position = new Vector2(rectangle.X, rectangle.Y);

            Z_Depth = 10000;

            Init(ID_Manger.Get_Next_ID());
        }
    }
}

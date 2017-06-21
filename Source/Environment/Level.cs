using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bowelbound.Environment
{
	public class Level
	{
		public enum LevelType
		{
			Generic
		}

		public LevelType
			type;
		public Tile[,]
			tiles;

		public int
			width,
			height;

		public Level(int width, int height)
		{
			this.width = width;
			this.height = height;

			this.tiles = new Tile[width, height];
		}

		public void generate(Random random)
		{
			// Set the level type
			this.type = LevelType.Generic;

			// Generate the tiles
			for(int x = 0; x < width; x++)
			{
				for(int y = 0; y < height; y++)
				{
					tiles[x, y] = new Tile(2);
					
					if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
						tiles[x, y] = new Tile(3);
				}
			}
		}

		public void update()
		{

		}

		public void draw(SpriteBatch spriteBatch,
					     Vector2 cameraPosition,
						 Texture2D[] tileTextures,
						 Texture2D[] tileShadowTextures,
						 int displacementX,
						 int displacementY,
						 float tileSize)
		{
			// Draw all tiles & tile shadows
			for(int x = 0; x < width; x++)
			{
				for(int y = 0; y < height; y++)
				{
					// Tile
					tiles[x, y].draw(spriteBatch,
									 cameraPosition,
									 tileTextures,
									 displacementX,
									 displacementY,
									 new Rectangle(x, y, width, height),
									 tileSize);

					// Tile shadows
					#region Tile Shadows

					if (!tiles[x, y].getOpaque())
					{
						if (x > 0 && y > 0)
							if (tiles[x - 1, y - 1].getOpaque()
								&& !tiles[x - 1, y].getOpaque()
								&& !tiles[x, y - 1].getOpaque())
							{
								spriteBatch.Draw(tileShadowTextures[0],
												 new Vector2((float)displacementX - cameraPosition.X + (x * tileSize),
															 (float)displacementY - cameraPosition.Y + (y * tileSize)),
												 null,
												 Color.White,
												 0.0F,
												 new Vector2(0.0F, 0.0F),
												 1.0F,
												 SpriteEffects.None,
												 1.0F - (float)((float)y / (float)(height * 4.0)) - 0.001F);
							}

						if (y > 0)
							if (tiles[x, y - 1].getOpaque())
							{
								spriteBatch.Draw(tileShadowTextures[1],
												 new Vector2((float)displacementX - cameraPosition.X + (x * tileSize),
															 (float)displacementY - cameraPosition.Y + (y * tileSize)),
												 null,
												 Color.White,
												 0.0F,
												 new Vector2(0.0F, 0.0F),
												 1.0F,
												 SpriteEffects.None,
												 1.0F - (float)((float)y / (float)(height * 4.0)) - 0.001F);
							}

						if (y > 0 && x < width - 1)
							if (tiles[x + 1, y - 1].getOpaque()
								&& !tiles[x, y - 1].getOpaque()
								&& !tiles[x + 1, y].getOpaque())
							{
								spriteBatch.Draw(tileShadowTextures[2],
												 new Vector2((float)displacementX - cameraPosition.X + (x * tileSize) + tileSize,
															 (float)displacementY - cameraPosition.Y + (y * tileSize)),
												 null,
												 Color.White,
												 0.0F,
												 new Vector2(16.0F, 0.0F),
												 1.0F,
												 SpriteEffects.None,
												 1.0F - (float)((float)y / (float)(height * 4.0)) - 0.001F);
							}

						if (x > 0)
							if (tiles[x - 1, y].getOpaque())
							{
								spriteBatch.Draw(tileShadowTextures[3],
												 new Vector2((float)displacementX - cameraPosition.X + (x * tileSize),
															 (float)displacementY - cameraPosition.Y + (y * tileSize)),
												 null,
												 Color.White,
												 0.0F,
												 new Vector2(0.0F, 0.0F),
												 1.0F,
												 SpriteEffects.None,
												 1.0F - (float)((float)y / (float)(height * 4.0F)) - 0.001F);
							}

						if (x < width - 1)
							if (tiles[x + 1, y].getOpaque())
							{
								spriteBatch.Draw(tileShadowTextures[4],
												 new Vector2((float)displacementX - cameraPosition.X + (x * tileSize) + tileSize,
															 (float)displacementY - cameraPosition.Y + (y * tileSize)),
												 null,
												 Color.White,
												 0.0F,
												 new Vector2(16.0F, 0.0F),
												 1.0F,
												 SpriteEffects.None,
												 1.0F - (float)((float)y / (float)(height * 4.0F)) - 0.001F);
							}

						if(x > 0 && y < height - 1)
							if(tiles[x - 1, y + 1].getOpaque()
								&& !tiles[x - 1, y].getOpaque()
								&& !tiles[x, y + 1].getOpaque())
							{
								spriteBatch.Draw(tileShadowTextures[5],
												 new Vector2((float)displacementX - cameraPosition.X + (x * tileSize),
															 (float)displacementY - cameraPosition.Y + (y * tileSize) + tileSize),
												 null,
												 Color.White,
												 0.0F,
												 new Vector2(0.0F, 16.0F),
												 1.0F,
												 SpriteEffects.None,
												 1.0F - (float)((float)y / (float)(height * 4.0F)) - 0.001F);
							}

						if(y < height - 1)
							if(tiles[x, y + 1].getOpaque())
							{
								spriteBatch.Draw(tileShadowTextures[6],
												 new Vector2((float)displacementX - cameraPosition.X + (x * tileSize),
															 (float)displacementY - cameraPosition.Y + (y * tileSize) + tileSize),
												 null,
												 Color.White,
												 0.0F,
												 new Vector2(0.0F, 16.0F),
												 1.0F,
												 SpriteEffects.None,
												 1.0F - (float)((float)y / (float)(height * 4.0F)) - 0.001F);
							}

						if(x < width - 1 && y < height - 1)
							if(tiles[x + 1, y + 1].getOpaque()
								&& !tiles[x + 1, y].getOpaque()
								&& !tiles[x, y + 1].getOpaque())
							{
								spriteBatch.Draw(tileShadowTextures[7],
												 new Vector2((float)displacementX - cameraPosition.X + (x * tileSize) + tileSize,
															 (float)displacementY - cameraPosition.Y + (y * tileSize) + tileSize),
												 null,
												 Color.White,
												 0.0F,
												 new Vector2(16.0F, 16.0F),
												 1.0F,
												 SpriteEffects.None,
												 1.0F - (float)((float)y / (float)(height * 4.0F)) - 0.001F);
							}
					}

					#endregion
				}
			}
		}
	}

	public class RoomTemplate
	{
		public Tile[,] tiles;

		public int width, height;

		public RoomTemplate(int width, int height)
		{
			this.width = width;
			this.height = height;

			this.tiles = new Tile[this.width, this.height];
		}
	}
}

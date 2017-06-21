using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bowelbound.Environment
{
	public class Tile
	{
		public int ID;

		public Tile(int ID)
		{
			this.ID = ID;
		}

		/// <summary>
		/// Gets whether or not objects/bullets/characters can pass through the tile.
		/// </summary>
		/// <returns></returns>
		public Boolean getSolid()
		{
			switch(this.ID)
			{
				case 0: return false;
				case 1: return true;
				case 2: return false;
				case 3: return true;
				case 4: return true;
				case 5: return true;

				default: return false;
			}
		}

		/// <summary>
		/// Gets whether or not light can pass through the tile.
		/// </summary>
		public Boolean getOpaque()
		{
			switch(this.ID)
			{
				case 0: return false;
				case 1: return true;
				case 2: return false;
				case 3: return true;
				case 4: return true;
				case 5: return true;

				default: return false;
			}
		}

		/// <summary>
		/// Draw the tile onto the screen.
		/// </summary>
		/// <param name="spriteBatch">The current game spritebatch.</param>
		/// <param name="cameraPosition">The current position of the game camera.</param>
		/// <param name="tileTextures">The array of all the tile textures.</param>
		/// <param name="displacementX">Displacement X</param>
		/// <param name="displacementY">Displacement Y</param>
		/// <param name="tilePositionData">A 'Rectangle' of the following values: X=Tile X, Y=Tile Y, Width=Level width, Height=Level height.</param>
		/// <param name="tileSize">The width of a game tile, in floating-point units. Default is something like 64.0F.</param>
		public void draw(SpriteBatch spriteBatch,
						 Vector2 cameraPosition,
						 Texture2D[] tileTextures,
						 int displacementX,
						 int displacementY,
						 Rectangle tilePositionData,
						 float tileSize)
		{
			spriteBatch.Draw(tileTextures[this.ID],
							 new Vector2((float)(displacementX - cameraPosition.X + (tilePositionData.X * tileSize)),
										 (float)(displacementY - cameraPosition.Y + (tilePositionData.Y * tileSize)) - tileSize),
							 null,
							 Color.White,
							 0.0F,
							 new Vector2(0.0F, 0.0F),
							 4.0F,
							 SpriteEffects.None,
							 1.0F - (float)((float)tilePositionData.Y / (float)(tilePositionData.Height * 4.0)) - (this.getSolid() ? 0.01F : 0.0F));
		}
	}
}

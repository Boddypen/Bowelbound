using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Bowelbound.Entities;

namespace Bowelbound.Environment
{
	public class Universe
	{
		public Level[] levels;

		public int currentLevel = 0;

		public Player player;

		public Universe(Random random)
		{
			this.levels = new Level[1];

			levels[0] = new Level(16, 16);
			levels[0].generate(random);

			this.player = new Player(new Vector2(256.0F, 256.0F),
									 new Vector2(40.0F, 40.0F),
									 new Vector2(0.0F, 0.0F),
									 0.95F);
		}

		public void updateCurrentLevel()
		{
			levels[currentLevel].update();
		}

		public void drawCurrentLevel(SpriteBatch spriteBatch,
									 Vector2 cameraPosition,
									 Texture2D[] tileTextures,
									 Texture2D[] tileShadowTextures,
									 int displacementX,
									 int displacementY,
									 float tileSize)
		{
			levels[currentLevel].draw(spriteBatch,
									  cameraPosition,
									  tileTextures,
									  tileShadowTextures,
									  displacementX,
									  displacementY,
									  tileSize);
		}
	}
}

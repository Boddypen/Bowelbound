using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using Bowelbound.Environment;

namespace Bowelbound.Entities
{
	public class Player : Entity
	{
		public float stepCounter = 50.0F;

		public Player(Vector2 position, Vector2 size, Vector2 velocity, float movementSpeed)
			: base(position, size, velocity, movementSpeed)
		{
			this.position = position;
			this.size = size;

			this.velocity = velocity;

			this.movementSpeed = movementSpeed;
		}

		public override void move(Level currentLevel, float tileSize, KeyboardState k, MouseState m, SoundEffect[] stepSounds, Random random)
		{
			// Determine which direction the player is walking in
			Vector2 movement = Vector2.Zero;

			if (k.IsKeyDown(Keys.W)) movement.Y--;
			if (k.IsKeyDown(Keys.S)) movement.Y++;
			if (k.IsKeyDown(Keys.A)) movement.X--;
			if (k.IsKeyDown(Keys.D)) movement.X++;

			if (movement.X != 0.0F || movement.Y != 0.0F)
				movement.Normalize();

			movement *= movementSpeed;

			velocity += movement;

			// Step Sound Effects

			if (movement.X == 0.0F && movement.Y == 0.0F)
				stepCounter = 25.0F;

			float totalMovement = (float)Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2));
			stepCounter -= totalMovement;
			
			if(stepCounter < 0.0F)
			{
				stepCounter = 75.0F;

				stepSounds[random.Next(4)].Play();
			}

			base.move(currentLevel, tileSize);
		}

		public override void draw(SpriteBatch spriteBatch,
								  Texture2D playerTexture,
								  Vector2 cameraPosition,
								  int displacementX,
								  int displacementY,
								  float tileSize,
								  int levelHeight)
		{
			spriteBatch.Draw(playerTexture,
							 new Vector2((float)displacementX - cameraPosition.X + position.X + (size.X / 2.0F),
										 (float)displacementY - cameraPosition.Y + position.Y + (size.Y / 2.0F)),
							 null,
							 Color.White,
							 0.0F,
							 new Vector2(6.0F, 12.0F),
							 4.0F,
							 SpriteEffects.None,
							 1.0F - (float)((position.Y + (tileSize * 1.0)) / (levelHeight * tileSize * 4.0F)));
		}
	}
}

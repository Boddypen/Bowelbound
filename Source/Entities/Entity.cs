using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Bowelbound.Environment;

namespace Bowelbound.Entities
{
	public class Entity
	{
		public Vector2 position, size;
		public Vector2 velocity;

		public float movementSpeed = 0.25F;
		
		public Entity(Vector2 position, Vector2 size, Vector2 velocity, float movementSpeed)
		{
			this.position = position;
			this.size = size;

			this.velocity = velocity;

			this.movementSpeed = movementSpeed;
		}
		
		/// <summary>
		/// Move the entity, and calculate AABB physics, etc.
		/// </summary>
		/// <param name="currentLevel">The object of the current game level.</param>
		/// <param name="tileSize">The width, in a floating-point unit, of a game tile.</param>
		public virtual void move(Level currentLevel, float tileSize)
		{
			#region AABB

			// First, get the entity's rectangle for use in AABB.
			Vector4 entityRectangle = new Vector4(position.X,
												  position.Y,
												  size.X,
												  size.Y);

			// Then create the rectangle of the entity, one step into the future.
			Vector4 futureRectangle = new Vector4(position.X + velocity.X,
												  position.Y + velocity.Y,
												  size.X,
												  size.Y);

			// Collision booleans
			Boolean
				collides = false,
				collidesHorizontal = false,
				collidesVertical = false;

			// Collision Normals
			Vector2
				horizontalNormal = Vector2.Zero,
				verticalNormal = Vector2.Zero;

			Vector2 newPosition = position;

			// Go through all nearby tiles to find out if the entity will collide or not.
			for (int x = 0; x < currentLevel.width; x++)
			{
				for(int y = 0; y < currentLevel.height; y++)
				{
					if(currentLevel.tiles[x, y].getSolid())
					{
						// Create a temporary variable to hold the tile position data.
						Vector4 tileVector4
							= new Vector4(x * tileSize,
										  y * tileSize,
										  tileSize,
										  tileSize);

						// Check if the entity collides with the tile
						if (floatCollides(futureRectangle, tileVector4))
							collides = true;

						// ... horizontally?
						if (floatCollides(entityRectangle + new Vector4(velocity.X, 0, 0, 0), tileVector4))
						{
							collidesHorizontal = true;

							// Set the normal, so the entity can 'snap' onto the tile.
							horizontalNormal.X = x * tileSize;
							horizontalNormal.Y = tileSize;
						}

						// ... vertically?
						if(floatCollides(entityRectangle + new Vector4(0, velocity.Y, 0, 0), tileVector4))
						{
							collidesVertical = true;

							// Again, set the normal! :P
							verticalNormal.X = y * tileSize;
							verticalNormal.Y = tileSize;
						}
					}
				}
			}

			// If the entity has collided with something, find out the normal of the tile.
			if (collides)
			{
				if(collidesHorizontal)
				{
					if(velocity.X < 0.0F)
					{
						// Moving to the left

						// Snap the entity onto the tile.
						newPosition.X = horizontalNormal.X + horizontalNormal.Y;

						// Stop moving the entity horizontally
						velocity.X = 0.0F;
					}
					else if(velocity.X > 0.0F)
					{
						// Moving to the right

						// Snap the entity onto the tile
						newPosition.X = horizontalNormal.X - size.X;

						// Stop moving the entity horizontally
						velocity.X = 0.0F;
					}
				}

				if(collidesVertical)
				{
					if(velocity.Y < 0.0F)
					{
						// Moving up

						// Snap the entity onto the ceiling
						newPosition.Y = verticalNormal.X + verticalNormal.Y;

						// Stop moving the entity vertically
						velocity.Y = 0.0F;
					}
					else if(velocity.Y > 0.0F)
					{
						// Moving down

						// Snap the entity onto the floor
						newPosition.Y = verticalNormal.X - size.Y;

						// Stop moving the entity
						velocity.Y = 0.0F;
					}
				}

				position = newPosition;
			}

			#endregion

			#region Movement

			// Move the entity
			position += velocity;

			// Slow the entity down
			velocity *= 0.80F;

			#endregion
		}

		/// <summary>
		/// Move the entity, according to the user input.
		/// </summary>
		/// <param name="k">The state of the keyboard.</param>
		/// <param name="m">The state of the mouse.</param>
		public virtual void move(Level currentLevel,
							     float tileSize,
								 KeyboardState k,
							     MouseState m,
								 SoundEffect[] stepSounds,
								 Random random)
		{
			// Adjust entity (player) velocity, then call regular move() method after.
		}

		/// <summary>
		/// Draw the entity.
		/// You must provide a texture to use.
		/// </summary>
		/// <param name="spriteBatch">The game's current spritebatch variable.</param>
		/// <param name="entityTexture">The texture to use for the entity.</param>
		public virtual void draw(SpriteBatch spriteBatch,
								 Texture2D entityTexture,
								 Vector2 cameraPosition,
								 int displacementX,
								 int displacementY,
								 float tileSize,
								 int levelHeight)
		{ }

		/// <summary>
		/// Calculate whether or not two rectangles are colliding.
		/// </summary>
		/// <param name="first">The first rectangle.</param>
		/// <param name="second">The second rectangle.</param>
		/// <returns></returns>
		public static Boolean floatCollides(Vector4 first, Vector4 second)
		{
			Rectangle firstR = new Rectangle((int)first.X, (int)first.Y, (int)first.Z, (int)first.W);
			Rectangle secondR = new Rectangle((int)second.X, (int)second.Y, (int)second.Z, (int)second.W);

			return firstR.Intersects(secondR);
		}
	}
}

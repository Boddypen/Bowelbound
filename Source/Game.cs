using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Bowelbound.Environment;

namespace Bowelbound
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		// Game Information
		public static readonly String
			GAME_NAME = "Bowelbound",
			GAME_VERSION = "Closed Pre-Alpha 0.1",
			GAME_CREDITS = "Credits";

		// Enumerables
		public enum GameState
		{
			Quit,
			Playing,
			Paused,
			Menu
		}
		public GameState
			gameState = GameState.Playing;

		// XNA Stuff
		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;

		// Input
		public KeyboardState k;
		public MouseState m;

		// Game Classes
		public Universe universe;
		public RoomTemplate[] roomTemplates;

		// XNA Data
		public SpriteFont
			uiFont,
			mainFont,
			mainSmallFont;
		public Texture2D[]
			tileTextures,
			tileShadowTextures;
		public Texture2D
			shadeTexture,
			playerTexture,
			cursorTexture;
		public SoundEffect[]
			stepSounds;
		public SoundEffect
			deathSound;

		// Regular Data
		public String
			testString = "Prepare to shit yourself!";
		public Random
			random;

		public Vector2
			cameraPosition;
		public float
			tileSize = 64.0F;
		public int
			displacementX, displacementY;
		
		public Game()
		{
			// Set up the graphics device
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1024;
			graphics.PreferredBackBufferHeight = 768;
			graphics.IsFullScreen = false;

			// Set the displacements
			displacementX = graphics.PreferredBackBufferWidth / 2;
			displacementY = graphics.PreferredBackBufferHeight / 2;

			// Set the window title to the game's name and version
			Window.Title = GAME_NAME + "  •  " + GAME_VERSION;

			// Make the mouse visible
			IsMouseVisible = true;

			// Set the root directory for the game content
			Content.RootDirectory = "Content";
		}
		
		protected override void Initialize()
		{
			// Init Logic

			// Random Number Generator
			random = new Random();

			// Vectors
			cameraPosition = new Vector2(0.0F, 0.0F);

			// Set up the UNIVERSE!
			universe = new Universe(random);

			// Set up the room templates
			roomTemplates = new RoomTemplate[2];
			for(int i = 0; i < roomTemplates.Length; i++)
			{
				roomTemplates[i] = new RoomTemplate(16, 16);

				try
				{
					
				}
				catch(Exception ex)
				{
					Console.Error.WriteLine(ex.Message);
					Console.Error.WriteLine(ex.StackTrace);
					this.Exit();
				}
			}

			base.Initialize();
		}
		
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Fonts
			uiFont = Content.Load<SpriteFont>("spritefont\\ui");
			mainFont = Content.Load<SpriteFont>("spritefont\\main");
			mainSmallFont = Content.Load<SpriteFont>("spritefont\\main_small");

			// Sound Effects
			stepSounds = new SoundEffect[4];
			for (int i = 0; i < stepSounds.Length; i++)
				stepSounds[i] = Content.Load<SoundEffect>("wav\\movement\\step" + i);
			
			// Textures

			// Player Texture
			playerTexture = Content.Load<Texture2D>("png\\entity\\player\\d");

			// Cursor Texture
			cursorTexture = Content.Load<Texture2D>("png\\interface\\cursor");

			#region Tile Textures

			// Set up the list of tiles
			tileTextures = new Texture2D[7];
			tileShadowTextures = new Texture2D[8];

			// Load in the individual tile textures
			tileTextures[0] = Content.Load<Texture2D>("png\\terrain\\tile");
			tileTextures[1] = Content.Load<Texture2D>("png\\terrain\\brick_wall");
			tileTextures[2] = Content.Load<Texture2D>("png\\terrain\\slab");
			tileTextures[3] = Content.Load<Texture2D>("png\\terrain\\blue_plaster_wall");
			tileTextures[4] = Content.Load<Texture2D>("png\\terrain\\wood_plaster_wall");
			tileTextures[5] = Content.Load<Texture2D>("png\\terrain\\blue_plaster_wall_painting");
			tileTextures[6] = Content.Load<Texture2D>("png\\terrain\\blue_plaster_desk");

			// Load the tile shadow textures
			tileShadowTextures[0] = Content.Load<Texture2D>("png\\shadow\\ul");
			tileShadowTextures[1] = Content.Load<Texture2D>("png\\shadow\\u");
			tileShadowTextures[2] = Content.Load<Texture2D>("png\\shadow\\ur");
			tileShadowTextures[3] = Content.Load<Texture2D>("png\\shadow\\l");
			tileShadowTextures[4] = Content.Load<Texture2D>("png\\shadow\\r");
			tileShadowTextures[5] = Content.Load<Texture2D>("png\\shadow\\dl");
			tileShadowTextures[6] = Content.Load<Texture2D>("png\\shadow\\d");
			tileShadowTextures[7] = Content.Load<Texture2D>("png\\shadow\\dr");

			#endregion

			shadeTexture = Content.Load<Texture2D>("png\\shadow\\shade");
		}
		
		protected override void UnloadContent() { }
		
		protected override void Update(GameTime gameTime)
		{
			// Update Logic

			// Get the keyboard and mouse state
			k = Keyboard.GetState();
			m = Mouse.GetState();

			// Allow the game to exit
			if (IsActive)
			{
				if (k.IsKeyDown(Keys.Escape))
					gameState = GameState.Quit;
			}
			else
			{
				gameState = GameState.Paused;
			}

			// Main switch
			switch(gameState)
			{
				case GameState.Quit:

					// Quit the game
					this.Exit();

					break;


				case GameState.Paused:

					if (!IsActive) break;

					// Allow the player to un-pause the game
					if (k.IsKeyDown(Keys.Enter))
						gameState = GameState.Playing;

					break;


				case GameState.Playing:

					if (!IsActive) break;

					// Move the camera towards the player's position
					cameraPosition.X += (universe.player.position.X + (universe.player.size.X / 2) - cameraPosition.X) / 4.0F;
					cameraPosition.Y += (universe.player.position.Y + (universe.player.size.Y / 2) - cameraPosition.Y) / 4.0F;
					
					// Update the universe
					universe.updateCurrentLevel();
					universe.player.move(universe.levels[universe.currentLevel], tileSize, k, m, stepSounds, random);

					break;
			}

			base.Update(gameTime);
		}
		
		protected override void Draw(GameTime gameTime)
		{
			// Clear the screen
			GraphicsDevice.Clear(Color.Black);

			// Begin the spritebatch
			spriteBatch.Begin(SpriteSortMode.BackToFront,
							  BlendState.AlphaBlend,
							  SamplerState.PointClamp,
							  DepthStencilState.None,
							  RasterizerState.CullCounterClockwise);

			// Main Drawing Switch
			switch (gameState)
			{
				case GameState.Playing:
				case GameState.Paused:
					
					// Draw the world
					universe.drawCurrentLevel(spriteBatch,
											  cameraPosition,
											  tileTextures,
											  tileShadowTextures,
											  displacementX,
											  displacementY,
											  tileSize);

					// Draw the player
					universe.player.draw(spriteBatch,
										 playerTexture,
										 cameraPosition,
										 displacementX,
										 displacementY,
										 tileSize,
										 universe.levels[universe.currentLevel].height);

					// Draw the shade
					spriteBatch.Draw(shadeTexture,
									 new Vector2(displacementX, displacementY),
									 null,
									 Color.White,
									 0.0F,
									 new Vector2(960, 540),
									 1.0F,
									 SpriteEffects.None,
									 0.5F);

					// Draw the test string
					spriteBatch.DrawString(mainFont,
										   testString,
										   new Vector2(20.0F, 20.0F),
										   Color.White);
					spriteBatch.DrawString(mainSmallFont,
										   GAME_NAME + " " + GAME_VERSION,
										   new Vector2(20.0F, graphics.PreferredBackBufferHeight - 50.0F),
										   Color.DarkGray);

					break;
			}

			if(gameState.Equals(GameState.Paused))
			{
				spriteBatch.DrawString(mainSmallFont,
									   "Paused!",
									   new Vector2(20.0F, graphics.PreferredBackBufferHeight - 80.0F),
									   new Color(1.0F, 1.0F, 0.0F));
			}
			
			// End the spritebatch
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

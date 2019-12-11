using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Xml;
using YetAnotherPlatformer.Utils;

namespace YetAnotherPlatformer
{

	public enum BackgroundIDs : int
	{
		BLUE=0,
		GREEN=1,
		GRAY=2,
		PURPLE=3,
		YELLOW=4,
		PINK=5,
		BROWN=6
	}

	public class Main : Game
	{

		public static float SCREEN_SCALE_FACTOR = 2f;

		public Texture2D[] backgrounds;

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Map map;

		public BackgroundIDs background = BackgroundIDs.GREEN;
		string creator = "Anonymous";
		string mapname = "YAP Level";
		Vector2 spawnpoint = new Vector2(5, 5);


		public Tiles tileManager;

		FrameCounter frameCounter;

		private Matrix screenTransform;

		Player player;

		Texture2D playerTexture;
		Texture2D terrainTexture;
		Texture2D rect;

		string mapToLoad;
		

		SpriteFont font;

		double heartbeatTiming;
		int heartbeat = 10;
		
		public Main(string map) {

			// bit of a gay ugly hack
			mapToLoad = map;

			graphics = new GraphicsDeviceManager(this) {
				PreferredBackBufferWidth = 1280,
				PreferredBackBufferHeight = 768,
				SynchronizeWithVerticalRetrace = false,
				IsFullScreen = false,
			};
			
			Content.RootDirectory = "Content";

			this.IsFixedTimeStep = false;
			IsMouseVisible = true;
		}

		protected override void Initialize() {

			Window.Title = "JAAAAAAME";
			IsFixedTimeStep = false;

			frameCounter = new FrameCounter();
			//-----------------
			// load map file

			XmlDocument doc = new XmlDocument();
			doc.Load(mapToLoad);

			XmlNode node = doc.SelectSingleNode("map");

			XmlNode properties = node.SelectSingleNode("properties");

			XmlNodeList props = properties.SelectNodes("property");

			foreach (XmlElement prop in props) {
				string propname = prop.GetAttribute("name");
				string propval = prop.GetAttribute("value");
				
				if (propname == "background") {
					Enum.TryParse(propval, out background);
				}
				if (propname == "creator") {
					creator = propval;
				}
				if (propname == "mapname") {
					mapname = propval;
				}

				// FUCK THIS
				if (propname == "spawnpoint") {
					string[] xy = propval.Split(',');
					spawnpoint = new Vector2( Int32.Parse(xy[0]) * 16, Int32.Parse(xy[1]) * 16);
				}
				// FUCK IT WITH FIRE
			}

			XmlNode subnode = node.SelectSingleNode("layer");
			XmlNode data = subnode.SelectSingleNode("data");
			//Console.WriteLine("POO" + data.InnerText);

			string text = data.InnerText;

			string[] split = text.Split(',');

			byte[] tiles = new byte[Definitions.MAP_WIDTH * Definitions.MAP_HEIGHT];

			
			for (int index = 0; index < tiles.Length; index++) {

				tiles[index] = Byte.Parse(split[index]);
			}
				
			//---------------------------------------
			map = new Map(tiles);

			// screen scale factor
			screenTransform = Matrix.CreateScale(SCREEN_SCALE_FACTOR);

			player = new Player();
			player.nextPosition = spawnpoint;
			player.position = spawnpoint;

			int playerTexWidth = (int) (player.boundingBox.X*2);
			int playerTexHeight = (int) (player.boundingBox.Y*2);

			playerTexture = new Texture2D(this.GraphicsDevice, playerTexWidth, playerTexHeight);

			Color[] colordata = new Color[playerTexWidth * playerTexHeight];
			for (int i = 0; i < (playerTexWidth * playerTexHeight); i++) {
				colordata[i] = Color.White;
			}
			playerTexture.SetData<Color>(colordata);

			rect = new Texture2D(this.GraphicsDevice, 1, 1);
			rect.SetData<Color>(new Color[1]{ Color.GhostWhite });

			heartbeatTiming = 0;

			base.Initialize();
		}

		void LoadBackgroundTextures() {
			backgrounds = new Texture2D[7];

			backgrounds[(int)BackgroundIDs.BLUE] = this.Content.Load<Texture2D>("Blue");
			backgrounds[(int)BackgroundIDs.BROWN] = this.Content.Load<Texture2D>("Brown");
			backgrounds[(int)BackgroundIDs.GRAY] = this.Content.Load<Texture2D>("Gray");
			backgrounds[(int)BackgroundIDs.GREEN] = this.Content.Load<Texture2D>("Green");
			backgrounds[(int)BackgroundIDs.PINK] = this.Content.Load<Texture2D>("Pink");
			backgrounds[(int)BackgroundIDs.PURPLE] = this.Content.Load<Texture2D>("Purple");
			backgrounds[(int)BackgroundIDs.YELLOW] = this.Content.Load<Texture2D>("Yellow");
		}

		protected override void LoadContent() {
		
			spriteBatch = new SpriteBatch(GraphicsDevice);

			terrainTexture = this.Content.Load<Texture2D>("terrain");

			tileManager = new Tiles(terrainTexture);

			font = this.Content.Load<SpriteFont>("Font");

			LoadBackgroundTextures();
		}


		protected override void UnloadContent() {

		}

		protected override void Update(GameTime gameTime) {

			frameCounter.Update(gameTime);

			double dt = gameTime.ElapsedGameTime.TotalSeconds;

			float delta = (float)dt;

			// if game is not focused, then don't do any further updates.
			if (!IsActive)
				return;

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			heartbeatTiming += gameTime.ElapsedGameTime.TotalMilliseconds;

			int count = 0;
			while (heartbeatTiming >= (heartbeat)) {
				count++;
				heartbeatTiming -= (heartbeat);

				// heartbeat updates
				player.Heartbeat();

				for (int x = 0; x < Definitions.MAP_WIDTH; x++) {
					for (int y = 0; y < Definitions.MAP_HEIGHT; y++) {
						byte tileid = map.tiles[x, y];

						if (tileid != 0) {

							float tileCenterX = (x* Definitions.TILE_SIZE)+(Definitions.TILE_SIZE/2);
							float tileCenterY = (y* Definitions.TILE_SIZE)+(Definitions.TILE_SIZE/2);
							float tileHalfWidth = (Definitions.TILE_SIZE / 2);

							bool isCollision = Physics.CheckCollisionAABB(
								player.nextPosition.X, player.nextPosition.Y, player.boundingBox.X, player.boundingBox.Y,
								tileCenterX, tileCenterY, tileHalfWidth, tileHalfWidth
							);

							if (isCollision) {

								Vector2 separation = Physics.GetSeparationAABB(
									player.nextPosition.X, player.nextPosition.Y, player.boundingBox.X, player.boundingBox.Y,
									tileCenterX, tileCenterY, tileHalfWidth, tileHalfWidth
								);


								if ((separation.X != 0.0f) || (separation.Y != 0.0f)) {
									Vector2 normal = Physics.GetNormalAABB(separation, player.velocity.X, player.velocity.Y);
									if (tileid == 41) {
										if (player.falling == true && normal.Y == -1) {
											player.falling = false;
											player.velocity.Y = 0;
											player.nextPosition += separation;
										}
									} else {
										player.nextPosition += separation;
										
										if ((normal.Y != 0.0f)) {
											if (separation.Y > 1 || separation.Y < -1)
												if (normal.Y == 1)
													player.velocity.Y = 0;

											if (normal.Y == -1) {
												player.falling = false;
												player.velocity.Y = 0;
											}

										} else if (normal.X != 0.0f) {
											if (separation.X > 1 || separation.X < -1) {
												if (normal.X == 1) {
													player.velocity.X = 0;
												}

												if (normal.X == -1) {
													player.velocity.X = 0;
												}
											}
										}
									}
									
								}
							}
						}
					}
				}
				player.position = player.nextPosition;
			}

			// regular updates
			player.Update(delta);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, screenTransform);

			


			// draw backgrounds

			for (int x = 0; x < 16; x++) {
				for (int y = 0; y < 16; y++) {
					spriteBatch.Draw(backgrounds[(int)background], new Vector2(x * 64, y * 64), Color.White);
				}
			}

			spriteBatch.Draw(playerTexture, player.GetRenderPosition(), Color.White);


			for (int x = 0; x < Definitions.MAP_WIDTH; x++) {
				for (int y = 0; y < Definitions.MAP_HEIGHT; y++) {
					int tileid = (int)map.tiles[x, y];
					//weird temporary gay hack
					tileid--;

					if (tileid > 0) {
						Rectangle quad = tileManager.GetRectangle((byte)tileid);
						spriteBatch.Draw(terrainTexture, new Vector2(x * Definitions.TILE_SIZE, y * Definitions.TILE_SIZE), quad, Color.White);
					}
				}
			}


			spriteBatch.DrawString(font, mapname + "\nby " + creator, new Vector2(480, 3), Color.White);

			//spriteBatch.Draw(floorTexture, floor.getRenderPosition());

			double framerate = frameCounter.GetExactFramerate();
			double average = frameCounter.GetAverageFramerate();

			spriteBatch.DrawString(font, "fps: " + Math.Floor(framerate).ToString() + " avg: " + Math.Floor(average).ToString(), new Vector2(0, 0), Color.White);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

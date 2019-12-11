using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace YetAnotherPlatformer
{
	public class Player
	{
		public Vector2 position;
		public Vector2 nextPosition;
		public Vector2 boundingBox;
		public Vector2 velocity;
		bool jumping;
		int jumpspring = 0;
		private int fallingJumpAttemptForgiveness = 0;
		public bool falling = true;


		public Player() {
			nextPosition = new Vector2(100, 100);
		//	position = new Vector2(50, 50);
			boundingBox = new Vector2(8, 8);
		}

		public Vector2 GetRenderPosition() {
			return new Vector2(position.X - boundingBox.X, position.Y - boundingBox.Y);
		}

		public void Update(float delta) {

		}

		public void Heartbeat() {

			bool moveLeft = Keyboard.GetState().IsKeyDown(Keys.A);
			bool moveRight = Keyboard.GetState().IsKeyDown(Keys.D);
			bool moveUp = Keyboard.GetState().IsKeyDown(Keys.W);
			bool moveDown = Keyboard.GetState().IsKeyDown(Keys.S);


			if (moveLeft) {
				if (velocity.X > -Definitions.PLAYER_MAX_HORIZONTAL_VELOCITY) {
					velocity.X -= Definitions.PLAYER_HORIZONTAL_ACCELLERATION;
				}
			}

			if (moveRight) {
				if (velocity.X < Definitions.PLAYER_MAX_HORIZONTAL_VELOCITY) {
					velocity.X += Definitions.PLAYER_HORIZONTAL_ACCELLERATION;
				}
			}

			//if (moveDown)
				//velocity.Y += Definitions.PLAYER_HORIZONTAL_ACCELLERATION;

			if (!moveLeft && !moveRight) {
				if (!falling) {
					velocity.X *= 0.6f;
				} else {
					velocity.X *= 0.93f;
				}
			}

			if (falling)
			{
				fallingJumpAttemptForgiveness--;
				if (velocity.Y < Definitions.TERMINAL_VELOCITY) { // terminal velocity
					velocity.Y += Definitions.GRAVITY; // gravity?
				}
			} else
			{
				fallingJumpAttemptForgiveness = 5;
				jumpspring = 0;
			}
			if (moveUp) {
				if (fallingJumpAttemptForgiveness > 0) {
					jumping = true;
				}
			} else {
				jumping = false;
			}

			if (jumping) {
				jumpspring++;

				if (moveUp) {
					if (jumpspring < 10) {
						velocity.Y -= Definitions.PLAYER_VERTICAL_ACCELLERATION;
					} else {
						velocity.Y -= Definitions.PLAYER_VERTICAL_ACCELLERATION/4;
					}
				}
			}

			nextPosition += velocity;

			falling = true;


			// if player is out of bounds of level, then they beat the level.
			// TODO: make a constant for screen size and width
			// TODO: make some autistic sparkles or whatever like fireworks when player beats the level
			if (position.X < 0 || position.X > 1280 || position.Y < 0 || position.Y > 720) {
				Console.WriteLine("U ARE WINRAR!!!!!!!!");
			}

		}

		public void Draw() {

		}
	}
}

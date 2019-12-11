using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace YetAnotherPlatformer
{
	public static class Physics
	{

		static Physics() {

		}

		public static bool CheckCollisionAABB(float x1, float y1, float width1, float height1, float x2, float y2, float width2, float height2) {
			float distanceX = x1 - x2;
			float distanceY = y1 - y2;

			float absoluteDistanceX = Math.Abs(distanceX);
			float absoluteDistanceY = Math.Abs(distanceY);

			float sumWidth = width1 + width2;
			float sumHeight = height1 + height2;

			if (absoluteDistanceY >= sumHeight || absoluteDistanceX >= sumWidth) {
				return false;
			}
			return true;
		}

		// !: the x and y arguments should be the middle of whatever rectangle you're testing.
		// width and height are the distance from the rectangle center that get tested.
		public static Vector2 GetSeparationAABB(float x1, float y1, float width1, float height1, float x2, float y2, float width2, float height2) {

			float distanceX = x1 - x2;
			float distanceY = y1 - y2;

			float absoluteDistanceX = Math.Abs(distanceX);
			float absoluteDistanceY = Math.Abs(distanceY);

			float sumWidth = width1 + width2;
			float sumHeight = height1 + height2;

			if (absoluteDistanceY >= sumHeight || absoluteDistanceX >= sumWidth) {
				return new Vector2(0.0f, 0.0f);
			}

			float sx = sumWidth - absoluteDistanceX;
			float sy = sumHeight - absoluteDistanceY;

			if (sx > sy) {
				if (sy > 0) {
					sx = 0;
				}
			} else {
				if (sx > 0) {
					sy = 0;
				}
			}

			if (distanceX < 0) {
				sx = -sx;
			}

			if (distanceY < 0 ) {
				sy = -sy;
			}

			return new Vector2(sx, sy);
		}


		public static Vector2 GetNormalAABB(Vector2 separation, double velx, double vely) {

			float d = (float)Math.Sqrt(separation.X * separation.X + separation.Y * separation.Y);

			float nx = separation.X / d;
			float ny = separation.Y / d;

			// penetration speed
			double ps = velx * nx + vely * ny;

			if (ps <= 0) {
				return new Vector2(nx, ny);
			}
			return new Vector2(0.0f, 0.0f);

		}


	}
}

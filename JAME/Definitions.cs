using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherPlatformer
{
	public static class Definitions
	{
		// map properties
		public const int TILE_SIZE  = 16;
		public const int MAP_WIDTH  = 40;
		public const int MAP_HEIGHT = 24;

		// physics properties
		public const float GRAVITY                         = 0.35f;
		public const float PLAYER_MAX_HORIZONTAL_VELOCITY  = 3.5f;
		public const float PLAYER_HORIZONTAL_ACCELLERATION = 0.3f;
		public const float PLAYER_VERTICAL_ACCELLERATION   = 0.8f;
		public const float TERMINAL_VELOCITY               = 8f;
	}
}

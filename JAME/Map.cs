using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherPlatformer
{

	

	public class Map
	{
		public byte[,] tiles;

		public Map(byte[] tileinput) {
			tiles = new byte[Definitions.MAP_WIDTH, Definitions.MAP_HEIGHT];

			int index = 0;

			for (int y = 0; y < Definitions.MAP_HEIGHT; y++) {
				for (int x = 0; x < Definitions.MAP_WIDTH; x++) {

					//Console.WriteLine(tileinput[index]);
					tiles[x, y] = tileinput[index];
					index++;


					//tiles[x, y] = 5;

					//if (x == 0) {
					//	tiles[x, y] = 107;
					//}

					//--if (x == 79) {
					//	tiles[x, y] = 105;
					//}

					//if (y == 40) {
					//	tiles[x, y] = 7;
					// brb gotta poop
					//}
					//if (y > 40) {
					//	tiles[x, y] = 22 + 7;
					//}
				}
			}
		}

		

		public void Update(float delta) {

		}

		public void Draw() {

		}
	}
}

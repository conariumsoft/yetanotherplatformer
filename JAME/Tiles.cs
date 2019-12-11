using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherPlatformer
{
	public class Tiles
	{

		Rectangle[] rects;

		public Tiles(Texture2D terrainTexture) {
			// generate quads for terrain sprite
			int numSectionsX = terrainTexture.Width / Definitions.TILE_SIZE;
			int numSectionsY = terrainTexture.Height / Definitions.TILE_SIZE;

			rects = new Rectangle[numSectionsX * numSectionsY];

			int inc = 0;
			for (int y = 0; y < numSectionsY; y++) {
				for (int x = 0; x < numSectionsX; x++) {
					Rectangle rect = new Rectangle(x * Definitions.TILE_SIZE, y * Definitions.TILE_SIZE, Definitions.TILE_SIZE, Definitions.TILE_SIZE);
					rects[inc] = rect;
					inc++;
				}
			}
		}

		public Rectangle GetRectangle(byte id) {
			return rects[id];
		}
		
	}
}

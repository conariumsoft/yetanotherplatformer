using System;

namespace YetAnotherPlatformer
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

			string map = "Maps/test.xml";

			if (args.Length >= 2) {
				if (args[0] == "-map") {
					map = args[1];
				}
			}

            using (var game = new Main(map))
                game.Run();
        }
    }
}

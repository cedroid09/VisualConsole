using System;
using System.Collections.Generic;
using System.IO;

namespace VisualConsole {

	public class GameBuffer {

		ConsoleColor DEFAULT_FOREGROUND_COLOR { get; set; }
		ConsoleColor DEFAULT_BACKGROUND_COLOR { get; set; }

		int WIDTH { get; set; }
		int HEIGHT { get; set; }

		ConsoleChar[,] LAST_BUFFER_ARRAY;

		/********************
		 * PUBLIC FUNCTIONS *
		 ********************/

		public void WriteCharToPosition(ConsoleChar write_char, Vector2 position) {

			// Error checking
			if (position.x >= WIDTH)
				throw new ArgumentOutOfRangeException("x");
			if (position.y >= HEIGHT)
				throw new ArgumentOutOfRangeException("y");

			// Write the character to the position
			Console.SetCursorPosition(position.x, position.y);
			Console.ForegroundColor = write_char.text_color;
			Console.BackgroundColor = write_char.bg_color;
			Console.Write(write_char.character);

			ResetCursor();

		}

		public void WriteScreen(ConsoleChar[,] screen_array) {

			// Rewrites the buffer intelligently, only rewriting things that change

			if (screen_array.GetLength(0) > WIDTH)
				throw new ArgumentOutOfRangeException("screen_array width (dimension 0)");
			if (screen_array.GetLength(1) > HEIGHT)
				throw new ArgumentOutOfRangeException("screen_array height (dimension 1)");

			List<Vector2> changepoints = CheckBufferAgainstLast(screen_array, LAST_BUFFER_ARRAY);

			for (int i = 0; i < changepoints.Count; i++) {

				WriteCharToPosition(screen_array[changepoints[i].x, changepoints[i].y], changepoints[i]);

			}

			CopyBuffer(screen_array, LAST_BUFFER_ARRAY);

		}

		public void CompleteWriteScreen(ConsoleChar[,] screen_array) {

			// Completely rewrites the buffer. Very slow.

			if (screen_array.GetLength(0) > WIDTH)
				throw new ArgumentOutOfRangeException("screen_array width (dimension 0)");
			if (screen_array.GetLength(1) > HEIGHT)
				throw new ArgumentOutOfRangeException("screen_array height (dimension 1)");

			for (int x = 0; x < WIDTH; x++) {

				for (int y = 0; y < HEIGHT; y++) {

					WriteCharToPosition(screen_array[x, y], new Vector2(x, y));

				}

			}

			CopyBuffer(screen_array, LAST_BUFFER_ARRAY);

		}

		public ConsoleChar[,] LoadScreenFromFile(string file_name, Vector2 size) {

			ConsoleChar[,] screen_array = new ConsoleChar[size.x, size.y];

			using (FileStream file_stream = new FileStream(file_name, FileMode.Open)) {

				// Go through each row rather than each column so that the array is initialized properly
				for (int y = 0; y < size.y; y++) {

					for (int x = 0; x < size.x; x++) {

						screen_array[x, y] = new ConsoleChar((char)file_stream.ReadByte(), DEFAULT_FOREGROUND_COLOR, DEFAULT_BACKGROUND_COLOR);

					}

					if (file_stream.Position == file_stream.Length)
						break;

					while ((char)file_stream.ReadByte() != '\n') ;

				}

			}

			return screen_array;

		}

		/********************
		 * STATIC FUNCTIONS *
		 ********************/

		public static void CopyBuffer(ConsoleChar[,] source, ConsoleChar[,] destination) {

			// Thanks to arrays being passed by reference, we need to use this function

			if (source.GetLength(0) != destination.GetLength(0))
				throw new ArgumentOutOfRangeException("source and destination must have the same lengths");
			if (source.GetLength(1) != destination.GetLength(1))
				throw new ArgumentOutOfRangeException("source and destination must have the same lengths");

			for (int x = 0; x < source.GetLength(0); x++) {

				for (int y = 0; y < source.GetLength(1); y++) {

					destination[x, y] = source[x, y];

				}

			}

		}

		/*********************
		 * PRIVATE FUNCTIONS *
		 *********************/

		List<Vector2> CheckBufferAgainstLast(ConsoleChar[,] next_array, ConsoleChar[,] last_array) {

			// Compares two buffers. This is an O(n) operation

			List<Vector2> changepoints = new List<Vector2>();

			for (int x = 0; x < WIDTH; x++) {

				for (int y = 0; y < HEIGHT; y++) {

					if (next_array[x, y] != last_array[x, y]) {

						changepoints.Add(new Vector2(x, y));

					}


				}

			}

			return changepoints;

		}

		/*******
		 * RWs *
		 *******/

		void ResetCursor() {
			Console.SetCursorPosition(0, HEIGHT);
			Console.ForegroundColor = DEFAULT_FOREGROUND_COLOR;
			Console.BackgroundColor = DEFAULT_BACKGROUND_COLOR;
		}

		public ConsoleKeyInfo WaitForKey() {
			ConsoleKeyInfo key = Console.ReadKey();
			ResetCursor();
			return key;
		}

		/**************************
		 * SETUP AND CONSTRUCTORS * 
		 **************************/

		public GameBuffer(int width, int height) {

			this.WIDTH = width;
			this.HEIGHT = height;

			SetupBuffer();

		}

		public GameBuffer(Vector2 size) {

			this.WIDTH = size.x;
			this.HEIGHT = size.y;

			SetupBuffer();

		}

		void SetupBuffer() {

			// Makes the cursor invisible
			Console.CursorVisible = false;

			// Sets the actual window height and width
			Console.WindowWidth = WIDTH;
			Console.BufferWidth = WIDTH;

			// Set the text buffer to match the window height and width
			Console.WindowHeight = HEIGHT + 1;
			Console.BufferHeight = HEIGHT + 1;

			// Puts the cursor in the non-use position
			Console.SetCursorPosition(0, HEIGHT);

			// Initializes LAST_BUFFER_ARRAY
			LAST_BUFFER_ARRAY = new ConsoleChar[WIDTH, HEIGHT];

			DEFAULT_FOREGROUND_COLOR = ConsoleColor.White;
			DEFAULT_BACKGROUND_COLOR = ConsoleColor.Black;

		}

	}

}

using System;
using System.Collections.Generic;

namespace VisualConsole {

	public struct ConsoleChar {

		public char character;
		public ConsoleColor text_color;
		public ConsoleColor bg_color;

		public ConsoleChar(char character, ConsoleColor text_color, ConsoleColor bg_color) {

			this.character = character;
			this.text_color = text_color;
			this.bg_color = bg_color;

		}

		public static bool operator ==(ConsoleChar a, ConsoleChar b) {

			return (a.character == b.character && a.text_color == b.text_color && a.bg_color == b.bg_color);
				

		}

		public static bool operator !=(ConsoleChar a, ConsoleChar b) {

			return !(a.character == b.character && a.text_color == b.text_color && a.bg_color == b.bg_color);
				

		}

	}

	public struct BufferArray {

		public List<List<ConsoleChar>> array;
		public int width;
		public int height;

		public BufferArray(int width, int height) {

			this.array = new List<List<ConsoleChar>>();
			this.width = width;
			this.height = height;

		}

	}

	public struct Vector2 {

		public int x;
		public int y;

		public Vector2(int x, int y) {

			this.x = x;
			this.y = y;

		}

	}

}

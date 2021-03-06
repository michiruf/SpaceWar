﻿namespace SpaceWar.Game {

	public static class GameDebug {

		public static bool Enabled { get; set; }
		
		private static bool SHADER_DISABLED;
		public static bool ShaderDisabled {
			// NoFormat
			get => Enabled && SHADER_DISABLED;
			set => SHADER_DISABLED = value;
		}

		private static bool IMMEDIATELY_PLAY;
		public static bool ImmediatelyPlay {
			// NoFormat
			get => Enabled && IMMEDIATELY_PLAY;
			set => IMMEDIATELY_PLAY = value;
		}

		private static bool UN_DIEABLE;
		public static bool UnDieable {
			// NoFormat
			get => Enabled && UN_DIEABLE;
			set => UN_DIEABLE = value;
		}

		private static int INITIAL_POINTS;
		public static int InitialPoints {
			// NoFormat
			get => !Enabled ? 0 : INITIAL_POINTS;
			set => INITIAL_POINTS = value;
		}
	}

}

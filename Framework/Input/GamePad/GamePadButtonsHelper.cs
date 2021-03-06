﻿using System.Collections.Generic;
using OpenTK.Input;
using OTKGamePad = OpenTK.Input.GamePad;

namespace Framework.Input.GamePad {

	public static class GamePadButtonsHelper {

		public static List<Buttons> GetPressedButtons(int controllerIndex) {
			return GetPressedButtons(OTKGamePad.GetState(controllerIndex));
		}

		public static List<Buttons> GetPressedButtons(GamePadState state) {
			var r = new List<Buttons>();
			if (state.Buttons.A == ButtonState.Pressed) r.Add(Buttons.A);
			if (state.Buttons.B == ButtonState.Pressed) r.Add(Buttons.B);
			if (state.Buttons.X == ButtonState.Pressed) r.Add(Buttons.X);
			if (state.Buttons.Y == ButtonState.Pressed) r.Add(Buttons.Y);
			if (state.Buttons.Back == ButtonState.Pressed) r.Add(Buttons.Back);
			if (state.Buttons.Start == ButtonState.Pressed) r.Add(Buttons.Start);
			if (state.Buttons.BigButton == ButtonState.Pressed) r.Add(Buttons.BigButton);
			if (state.Buttons.LeftShoulder == ButtonState.Pressed) r.Add(Buttons.LeftShoulder);
			if (state.Buttons.RightShoulder == ButtonState.Pressed) r.Add(Buttons.RightShoulder);
			if (state.Buttons.LeftStick == ButtonState.Pressed) r.Add(Buttons.LeftStick);
			if (state.Buttons.RightStick == ButtonState.Pressed) r.Add(Buttons.RightStick);
			return r;
		}

		public static List<Buttons> GetPressedDPadButtons(int controllerIndex) {
			return GetPressedDPadButtons(OTKGamePad.GetState(controllerIndex));
		}

		public static List<Buttons> GetPressedDPadButtons(GamePadState state) {
			var r = new List<Buttons>();
			if (state.DPad.IsUp) r.Add(Buttons.DPadUp);
			if (state.DPad.IsDown) r.Add(Buttons.DPadDown);
			if (state.DPad.IsLeft) r.Add(Buttons.DPadLeft);
			if (state.DPad.IsRight) r.Add(Buttons.DPadRight);
			return r;
		}
	}

}

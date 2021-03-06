﻿using System;
using System.Collections.Generic;
using Framework.Input;
using OpenTK.Input;

namespace SpaceWar.Game {

	[Obsolete]
	public class Keymap : InputProvider {

		protected override void GetDefaultInputs(Dictionary<Enum, object> inputs) {
			// Keyboard
			inputs.Add(Key.W, InputActions.MoveUp);
			inputs.Add(Key.S, InputActions.MoveDown);
			inputs.Add(Key.A, InputActions.MoveLeft);
			inputs.Add(Key.D, InputActions.MoveRight);
			inputs.Add(Key.Space, InputActions.Fire);
			// Temporary keyboard
			inputs.Add(Key.Up, InputActions.SimpleFireUp);
			inputs.Add(Key.Down, InputActions.SimpleFireDown);
			inputs.Add(Key.Left, InputActions.SimpleFireLeft);
			inputs.Add(Key.Right, InputActions.SimpleFireRight);
			
			// Controller
			inputs.Add(Buttons.DPadUp, InputActions.MoveUp);
			inputs.Add(Buttons.DPadDown, InputActions.MoveDown);
			inputs.Add(Buttons.DPadLeft, InputActions.MoveLeft);
			inputs.Add(Buttons.DPadRight, InputActions.MoveRight);
			inputs.Add(Buttons.A, InputActions.Fire);
		}
	}

}

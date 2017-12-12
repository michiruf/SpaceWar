﻿using System;
using Framework;
using Framework.Object;
using Framework.Utilities;
using OpenTK;
using OpenTK.Input;

namespace SpaceWar.Game.Play.Player {

	public class PlayerShotController : Component, UpdateComponent {

		private readonly LimitedRateTimer shotTimer = new LimitedRateTimer();

		public float ShotRate { get; private set; } = Player.INITIAL_SHOT_RATE;

		private Player player;

		public override void OnStart() {
			base.OnStart();
			player = GameObject as Player;
		}

		public void Update() {
			shotTimer.DoOnlyEvery(ShotRate, Shoot);
		}

		void Shoot() {
			// Detect keyboard first only for the first player
			if (player.PlayerIndex == 0) {
				var keyboardAxis = Vector2.Zero;
				if (Keyboard.GetState().IsKeyDown(Key.Up)) {
					keyboardAxis.Y++;
				}
				if (Keyboard.GetState().IsKeyDown(Key.Down)) {
					keyboardAxis.Y--;
				}
				if (Keyboard.GetState().IsKeyDown(Key.Left)) {
					keyboardAxis.X--;
				}
				if (Keyboard.GetState().IsKeyDown(Key.Right)) {
					keyboardAxis.X++;
				}
				if (keyboardAxis != Vector2.Zero) {
					var simpleDirection = (float) Math.Atan2(keyboardAxis.Y, keyboardAxis.X);
					Scene.Current.Spawn(new Shot.Shot(simpleDirection, GameObject.Transform.WorldPosition,
						() => player.Attributes.OnEnemyKill()));
					return;
				}
			}

			// Detect gamepad and skip if no inputs are given by using the correct float comparison
			var gamepadAxis = GamePad.GetState(player.PlayerIndex).ThumbSticks.Right;
			if (Math.Abs(gamepadAxis.X) >= Options.CONTROLLER_THRESHOLD ||
			    Math.Abs(gamepadAxis.Y) >= Options.CONTROLLER_THRESHOLD) {
				var direction = (float) Math.Atan2(gamepadAxis.Y, gamepadAxis.X);
				Scene.Current.Spawn(new Shot.Shot(direction, GameObject.Transform.WorldPosition,
					() => player.Attributes.OnEnemyKill()));
			}
		}
	}

}

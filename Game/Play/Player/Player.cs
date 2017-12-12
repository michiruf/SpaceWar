﻿using System.Drawing;
using Framework;
using Framework.Collision;
using Framework.Render;

namespace SpaceWar.Game.Play.Player {

	public class Player : GameObject {

		// Logic constants
		public const int MAX_LIFES = 5;
		public const float INITIAL_SHOT_RATE = 0.15f;
		public const float INITIAL_SPEED = 0.45f;
		public const float CAMERA_SPEED = 0.5f;
		public const float CAMERA_MIN_SPEED = 0.05f;

		// Visual constants
		public const float PLAYER_SIZE = 0.05f;

		// Containers
		public PlayerAttributes Attributes { get; }
		public PlayerMovementController MovementController { get; }
		public PlayerShotController ShotController { get; }
		public PlayerCollisionController CollisionController { get; }
		public FollowingCameraController FollowingCameraController { get; }
		public RenderBoxComponent RenderBoxComponent { get; }
		public BoxCollider BoxCollider { get; }

		// Properties
		public int PlayerIndex { get; }

		public Player(int playerIndex) {
			Attributes = new PlayerAttributes();
			MovementController = new PlayerMovementController();
			ShotController = new PlayerShotController();
			CollisionController = new PlayerCollisionController();
			FollowingCameraController = new FollowingCameraController();
			RenderBoxComponent = new RenderBoxComponent(PLAYER_SIZE, PLAYER_SIZE).Fill(Color.White);
			BoxCollider = new BoxCollider(PLAYER_SIZE, PLAYER_SIZE);

			AddComponent(Attributes);
			AddComponent(MovementController);
			AddComponent(ShotController);
			AddComponent(CollisionController);
			AddComponent(FollowingCameraController);
			AddComponent(RenderBoxComponent);
			AddComponent(BoxCollider);

			PlayerIndex = playerIndex;
		}
	}

}

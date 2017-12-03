﻿using System.Drawing;
using Framework;
using Framework.Render;
using SpaceWar.Resources;

namespace SpaceWar.Game {

	public class DummyGameObject : GameObject {

		public DummyGameObject() {
			AddComponent(new DummyController());
			AddComponent(new RenderTextureComponent(Resource.background, 0.2f, 0.2f));
			AddComponent(new RenderLineComponent(new PointF(-0.2f, -0.2f), new PointF(0.2f, 0.2f), Color.White, 2f));
			AddComponent(new RenderLineComponent(new PointF(0.2f, -0.2f), new PointF(0f, 0f), Color.GreenYellow, 2f));
		}
	}

}

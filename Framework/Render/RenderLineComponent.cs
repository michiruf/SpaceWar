﻿using System;
using System.Drawing;
using Framework.Debug;
using Framework.Object;
using Framework.Utilities;
using OpenTK.Graphics.OpenGL;

namespace Framework.Render {

	[Obsolete("Recommendation Daniel Scherzer: we should not use lines with a width because its hardware dependent" +
	          "how they get rendered")]
	public class RenderLineComponent : Component, RenderComponent {

		private readonly PointF from;
		private readonly PointF to;
		private readonly Color color;
		private readonly float lineWidth;

		public RenderLineComponent(PointF from, PointF to, Color color, float lineWidth = 1f) {
			this.from = from;
			this.to = to;
			this.color = color;
			this.lineWidth = lineWidth;
		}

		public void Render() {
			// Enable blending for transparency
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend);
			
			GL.Color4(color);
			GL.LineWidth(lineWidth);
			
			var matrix = GameObject?.Transform?.GetTransformationMatrixCached(!GameObject.IsUiElement) ?? 
			             System.Numerics.Matrix3x2.Identity;
			var fromPoint = FastVector2Transform.Transform(from.X, from.Y, matrix);
			var toPoint = FastVector2Transform.Transform(to.X, to.Y, matrix);

			GL.Begin(PrimitiveType.Lines);
			GL.Vertex2(fromPoint);
			GL.Vertex2(toPoint);
			GL.End();

			if (FrameworkDebug.Enabled) {
				GL.Color4(Color.Red);
				GL.PointSize(lineWidth);
				GL.Begin(PrimitiveType.Points);
				GL.Vertex2(fromPoint);
				GL.Vertex2(toPoint);
				GL.End();
			}
		}
	}

}

﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using Framework.Algorithms;
using Framework.Debug;
using Framework.Object;
using OpenTK.Graphics.OpenGL4;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;
using GL = OpenTK.Graphics.OpenGL.GL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

namespace Framework.Render {

	public class RenderTextComponent : Component, RenderComponent {

		private ITexture texture;

		private string text;
		private Font font;
		private Brush brush;
		private Box2D rect;

		public string Text {
			get => text;
			set {
				text = value;
				Invalidate();
			}
		}
		public Font Font {
			get => font;
			set {
				font = value;
				Invalidate();
			}
		}
		public Brush Brush {
			get => brush;
			set {
				brush = value;
				Invalidate();
			}
		}
		public Box2D Rect {
			get => rect;
			set {
				rect = value;
				Invalidate();
			}
		}

		public RenderTextComponent(string text, Font font, Brush brush, Box2D rect) {
			// Assign stuff
			Text = text;
			Font = font;
			Brush = brush;
			Rect = rect;
		}

		protected void MayInitialize() {
			if (texture == null) {
				Initialize();
			}
		}

		protected void Initialize() {
			// Calculate the size
			SizeF size;
			using (var tmpBmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb)) {
				var gfx = Graphics.FromImage(tmpBmp);
				size = gfx.MeasureString(Text, Font);
			}

			using (var bmp = new Bitmap(
				(int) Math.Ceiling(size.Width),
				(int) Math.Ceiling(size.Height),
				PixelFormat.Format32bppArgb)) {
				// Create graphics to draw on bitmap
				using (var gfx = Graphics.FromImage(bmp)) {
					gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
					gfx.DrawString(Text, Font, Brush, new PointF(0f, 0f));
				}

				// Create texture from bitmap
				var bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
					ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

				var texture2DGl = new Texture2dGL();
				texture2DGl.LoadPixels(
					bitmapData.Scan0,
					bmp.Width, bmp.Height,
					PixelInternalFormat.Rgba,
					OpenTK.Graphics.OpenGL4.PixelFormat.Rgba,
					PixelType.UnsignedByte);
				// Next line makes the rendered stuff match nearest pixels (no aliasing?)
				texture2DGl.Filter = TextureFilterMode.Nearest;

				bmp.UnlockBits(bitmapData);
				texture = texture2DGl;
			}
		}

		protected void Invalidate() {
			texture?.Dispose();
			texture = null;
		}

		public void Render() {
			MayInitialize();

			GL.Enable(EnableCap.Texture2D);

			texture.Activate();
			GL.Begin(PrimitiveType.Quads);

			// Color is multiplied with the texture color
			// White means no color change in the texture will be applied
			GL.Color3(Color.White);
			if (FrameworkDebugMode.IsEnabled) {
				GL.Color3(Color.LightGray);
			}

			// Note that max and min Y is inverted
			var matrix = GameObject.Transform.GetTransformationMatrixCached(!GameObject.IsUiElement);
			var minXmaxY = FastVector2Transform.Transform(Rect.MinX, Rect.MinY, matrix);
			var maxXmaxY = FastVector2Transform.Transform(Rect.MaxX, Rect.MinY, matrix);
			var maxXminY = FastVector2Transform.Transform(Rect.MaxX, Rect.MaxY, matrix);
			var minXminY = FastVector2Transform.Transform(Rect.MinX, Rect.MaxY, matrix);

			GL.TexCoord2(0.0f, 0.0f);
			GL.Vertex2(minXminY);
			GL.TexCoord2(1.0f, 0.0f);
			GL.Vertex2(maxXminY);
			GL.TexCoord2(1.0f, 1.0f);
			GL.Vertex2(maxXmaxY);
			GL.TexCoord2(0.0f, 1.0f);
			GL.Vertex2(minXmaxY);

			GL.End();
			texture.Deactivate();

			GL.Disable(EnableCap.Texture2D);
		}

	}

}
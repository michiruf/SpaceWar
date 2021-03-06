﻿using System;
using System.Drawing;
using System.Text;
using Framework.Camera;
using Framework.Object;
using Framework.Utilities;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Zenseless.Geometry;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace Framework.Render {

	// TODO: GLSL Lang Compiler benutzen um den Shader zu kompilieren -> generiert am meisten Warnings/Errors

	public class RenderShaderComponent : Component, RenderComponent {

		private readonly IShader shader;

		public Box2D Rect { get; set; }

		public RenderShaderComponent(string vertex, string fragment, Box2D rect) {
			try {
				shader = ShaderLoader.FromStrings(vertex, fragment);
			} catch (ShaderCompileException e) {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.ShaderLog);
			}
			Rect = rect;
		}

		public RenderShaderComponent(string vertex, string fragment, float width, float height) :
			this(vertex, fragment, new Box2D(-width / 2, -height / 2, width, height)) {
		}

		[Obsolete]
		public RenderShaderComponent SetAttribute(string name, Action<int> setAction) {
			shader.Activate();
			var position = shader.GetResourceLocation(ShaderResourceType.Attribute, name);
			// Do only if found
			if (position != -1) {
				setAction?.Invoke(position);
			}
			shader.Deactivate();
			return this;
		}

		[Obsolete]
		public RenderShaderComponent SetAttribute(string name, Vector2 value) {
			return SetAttribute(name, position => GL.VertexAttrib2(position, value));
		}

		// NOTE Maybe extract the methods into a Binder-class?

		public RenderShaderComponent SetUniform(string name, Action<int> setAction) {
			shader.Activate();
			var position = shader.GetResourceLocation(ShaderResourceType.Uniform, name);
			// Do only if found
			if (position != -1) {
				setAction?.Invoke(position);
			}
			shader.Deactivate();
			return this;
		}

		public RenderShaderComponent SetUniform(string name, float value) {
			return SetUniform(name, position => GL.Uniform1(position, value));
		}

		public RenderShaderComponent SetUniform(string name, Vector2 value) {
			return SetUniform(name, position => GL.Uniform2(position, value));
		}

		public RenderShaderComponent SetUniform(string name, Vector2[] value) {
			return SetUniform(name, position => {
				// TODO This does not work yet
				var values = new float[value.Length * 2];
				for (var i = 0; i < value.Length; i++) {
					values[2 * i] = value[i].X;
					values[2 * i + 1] = value[i].Y;
				}
				GL.Uniform2(position, value.Length, values);
			});
		}

		public RenderShaderComponent SetUniform(string name, Vector3 value) {
			return SetUniform(name, position => GL.Uniform3(position, value));
		}

		public override void OnDestroy() {
			base.OnDestroy();
			shader?.Dispose();
		}

		public void Render() {
			// Enable blending for transparency in textures
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Blend);

			// Color is multiplied with the texture color
			// White means no color change in the texture will be applied
			GL.Color4(Color.White);

			var matrix = GameObject?.Transform?.GetTransformationMatrixCached(!GameObject.IsUiElement) ??
			             System.Numerics.Matrix3x2.Identity;
			var minXminY = FastVector2Transform.Transform(Rect.MinX, Rect.MinY, matrix);
			var maxXminY = FastVector2Transform.Transform(Rect.MaxX, Rect.MinY, matrix);
			var maxXmaxY = FastVector2Transform.Transform(Rect.MaxX, Rect.MaxY, matrix);
			var minXmaxY = FastVector2Transform.Transform(Rect.MinX, Rect.MaxY, matrix);

			shader.Activate();
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0.0f, 0.0f);
			GL.Vertex2(minXminY);
			GL.TexCoord2(1.0f, 0.0f);
			GL.Vertex2(maxXminY);
			GL.TexCoord2(1.0f, 1.0f);
			GL.Vertex2(maxXmaxY);
			GL.TexCoord2(0.0f, 1.0f);
			GL.Vertex2(minXmaxY);
			GL.End();
			shader.Deactivate();

			GL.Disable(EnableCap.Blend);
		}
	}

}

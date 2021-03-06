﻿using Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using Matrix3x2 = System.Numerics.Matrix3x2;
using Vector2 = OpenTK.Vector2;

namespace FrameworkUnitTests {

	[TestClass]
	public class TransformTest {

		[TestMethod]
		public void ConstructorTest() {
			var t = new Transform();
			Assert.IsNull(t.GameObject);
			Assert.AreEqual(t.LocalToWorld, Matrix3x2.Identity);
			Assert.AreEqual(t.WorldToLocal, Matrix3x2.Identity);
		}

		[TestMethod]
		public void TestIdentityInversableLocalAndWorldConversion() {
			var t = new Transform();
			var tpInput = new Vector2(1, 2);
			var tpLocal = t.TransformPoint(tpInput, Space.Local);
			var tpWorld = t.TransformPoint(tpLocal, Space.World);
			Assert.AreEqual(tpInput, tpWorld);
		}

		[TestMethod]
		public void TestTranslationInversableLocalAndWorldConversion() {
			var t = new Transform();
			t.Translate(new Vector2(1, 2), Space.Local);
			var tpInput = new Vector2(1, 2);
			var tpLocal = t.TransformPoint(tpInput, Space.Local);
			var tpWorld = t.TransformPoint(tpLocal, Space.World);
			Assert.AreEqual(tpInput, tpWorld);
		}

		[TestMethod]
		public void TestRotationInversableLocalAndWorldConversion() {
			var t = new Transform();
			t.Rotate(81);
			var rpInput = new Vector2(-11, 2);
			var rpLocal = t.TransformPoint(rpInput, Space.Local);
			var rpWorld = t.TransformPoint(rpLocal, Space.World);
			Assert.AreEqual(rpInput, rpWorld);
		}

		[TestMethod]
		public void TestScalingInversableLocalAndWorldConversion() {
			var t = new Transform();
			t.Scale(0.2f, 1.5f, Space.Local);
			var spInput = new Vector2(40, 20f);
			var spLocal = t.TransformPoint(spInput, Space.Local);
			var spWorld = t.TransformPoint(spLocal, Space.World);
			Assert.AreEqual(spInput, spWorld);
		}

		[TestMethod]
		public void TestComplexInversableLocalAndWorldConversion_Local() {
			var t = new Transform();

			t.Translate(new Vector2(1, 2), Space.Local);
			var tpInput = new Vector2(1, 2);
			var tpLocal = t.TransformPoint(tpInput, Space.Local);
			var tpWorld = t.TransformPoint(tpLocal, Space.World);
			Assert.AreEqual(tpInput, tpWorld);

			t.Rotate(90);
			var rpInput = new Vector2(-11, 2);
			var rpLocal = t.TransformPoint(rpInput, Space.Local);
			var rpWorld = t.TransformPoint(rpLocal, Space.World);
			Assert.AreEqual(rpInput, rpWorld);

			t.Scale(3f, 2f, Space.Local);
			var spInput = new Vector2(40, 20);
			var spLocal = t.TransformPoint(spInput, Space.Local);
			var spWorld = t.TransformPoint(spLocal, Space.World);
			Assert.AreEqual(spInput, spWorld);
		}

		[TestMethod]
		public void TestComplexInversableLocalAndWorldConversion_World() {
			var t = new Transform();

			t.Translate(new Vector2(1, 2), Space.World);
			var tpInput = new Vector2(1, 2);
			var tpLocal = t.TransformPoint(tpInput, Space.Local);
			var tpWorld = t.TransformPoint(tpLocal, Space.World);
			Assert.AreEqual(tpInput, tpWorld);

			t.Rotate(90);
			var rpInput = new Vector2(-11, 2);
			var rpLocal = t.TransformPoint(rpInput, Space.Local);
			var rpWorld = t.TransformPoint(rpLocal, Space.World);
			Assert.AreEqual(rpInput, rpWorld);

			t.Scale(3f, 2f, Space.World);
			var spInput = new Vector2(40, 20);
			var spLocal = t.TransformPoint(spInput, Space.Local);
			var spWorld = t.TransformPoint(spLocal, Space.World);
			Assert.AreEqual(spInput, spWorld);
		}

		[TestMethod]
		public void TestWorldToLocalLocalToWorldMultiplication_TranslateLocal() {
			var tt = new Transform();
			tt.Translate(1f, 2f, Space.Local);
			Assert.AreEqual(tt.WorldToLocal * tt.LocalToWorld, Matrix3x2.Identity);
		}

		[TestMethod]
		public void TestWorldToLocalLocalToWorldMultiplication_RotateLocal() {
			var tt = new Transform();
			tt.Rotate(90);
			Assert.AreEqual(tt.WorldToLocal * tt.LocalToWorld, Matrix3x2.Identity);
		}

		[TestMethod]
		public void TestWorldToLocalLocalToWorldMultiplication_ScaleLocal() {
			var tt = new Transform();
			tt.Scale(2f, 3f, Space.Local);
			Assert.AreEqual(tt.WorldToLocal * tt.LocalToWorld, Matrix3x2.Identity);
		}

		[TestMethod]
		public void TestWorldToLocalLocalToWorldMultiplication_TranslateWorld() {
			var tt = new Transform();
			tt.Translate(1f, 2f, Space.World);
			Assert.AreEqual(tt.WorldToLocal * tt.LocalToWorld, Matrix3x2.Identity);
		}

		[TestMethod]
		public void TestWorldToLocalLocalToWorldMultiplication_RotateWorld() {
			var tt = new Transform();
			tt.Rotate(90);
			Assert.AreEqual(tt.WorldToLocal * tt.LocalToWorld, Matrix3x2.Identity);
		}

		[TestMethod]
		public void TestWorldToLocalLocalToWorldMultiplication_ScaleWorld() {
			var tt = new Transform();
			tt.Scale(2f, 3f, Space.Local);
			Assert.AreEqual(tt.WorldToLocal * tt.LocalToWorld, Matrix3x2.Identity);
		}

		[TestMethod]
		public void TranslateLocalTest() {
			var t = new Transform();
			t.Rotate(90);
			t.Translate(1, 0, Space.Local); // (X, Y)
			Assert.AreEqual(t.WorldToLocal.M31, 0); // X
			Assert.AreEqual(t.WorldToLocal.M32, 1); // Y
		}

		[TestMethod]
		public void TranslateGlobalTest() {
			var t = new Transform();
			t.Rotate(90);
			t.Translate(1, 0, Space.World); // (X, Y)
			Assert.AreEqual(t.WorldToLocal.M31, 1); // X
			Assert.AreEqual(t.WorldToLocal.M32, 0); // Y

			t.Translate(1, 0, Space.World); // (X, Y)
			Assert.AreEqual(t.WorldToLocal.M31, 2); // X
			Assert.AreEqual(t.WorldToLocal.M32, 0); // Y

			// World position
			var t2 = new Transform();
			t2.Translate(1, 0, Space.World);
			t2.Translate(1, 0, Space.World);
			Assert.AreEqual(t2.WorldPosition, new Vector2(2, 0));

			// Local position
			var t3 = new Transform();
			t3.Translate(1, 0, Space.World);
			t3.Translate(1, 0, Space.World);
			Assert.AreEqual(t3.LocalPosition, new Vector2(2, 0));
		}

		[TestMethod]
		public void TranslateGlobalCalculaltionWorldTest() {
		}

		[TestMethod]
		public void TranslateGlobalCalculaltionLocalTest() {
		}

		[TestMethod]
		public void RotateTest() {
			var t = new Transform();

			// Assert the matrixes are the identity matrixes
			Assert.AreEqual(t.LocalToWorld, Matrix3x2.Identity);
			Assert.AreEqual(t.WorldToLocal, Matrix3x2.Identity);

			// Test identity rotation
			t.Rotate(90);
			Assert.AreEqual(t.LocalToWorld, Matrix3x2.CreateRotation(MathHelper.DegreesToRadians(-90)));
			Assert.AreEqual(t.WorldToLocal, Matrix3x2.CreateRotation(MathHelper.DegreesToRadians(90)));
		}

		[TestMethod]
		public void ScaleLocalTest() {
			// TODO
		}

		[TestMethod]
		public void ScaleGlobalTest() {
			// TODO
		}
	}

}

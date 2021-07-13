using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common.Core.Numerics;

namespace UnityEngine
{

    public static class MatrixExtension
    {

        public static Matrix4x4 ToMatrix4x4(this Matrix4x4f m)
        {
            Matrix4x4 mat = new Matrix4x4();

            mat.m00 = m.m00; mat.m01 = m.m01; mat.m02 = m.m02; mat.m03 = m.m03;
            mat.m10 = m.m10; mat.m11 = m.m11; mat.m12 = m.m12; mat.m13 = m.m13;
            mat.m20 = m.m20; mat.m21 = m.m21; mat.m22 = m.m22; mat.m23 = m.m23;
            mat.m30 = m.m30; mat.m31 = m.m31; mat.m32 = m.m32; mat.m33 = m.m33;

            return mat;
        }

        public static Matrix4x4 ToMatrix4x4(this Matrix4x4d m)
        {
            Matrix4x4 mat = new Matrix4x4();

            mat.m00 = (float)m.m00; mat.m01 = (float)m.m01; mat.m02 = (float)m.m02; mat.m03 = (float)m.m03;
            mat.m10 = (float)m.m10; mat.m11 = (float)m.m11; mat.m12 = (float)m.m12; mat.m13 = (float)m.m13;
            mat.m20 = (float)m.m20; mat.m21 = (float)m.m21; mat.m22 = (float)m.m22; mat.m23 = (float)m.m23;
            mat.m30 = (float)m.m30; mat.m31 = (float)m.m31; mat.m32 = (float)m.m32; mat.m33 = (float)m.m33;

            return mat;
        }

        public static Matrix4x4f ToMatrix4x4f(this Matrix4x4 m)
        {
            Matrix4x4f mat = new Matrix4x4f();

            mat.m00 = m.m00; mat.m01 = m.m01; mat.m02 = m.m02; mat.m03 = m.m03;
            mat.m10 = m.m10; mat.m11 = m.m11; mat.m12 = m.m12; mat.m13 = m.m13;
            mat.m20 = m.m20; mat.m21 = m.m21; mat.m22 = m.m22; mat.m23 = m.m23;
            mat.m30 = m.m30; mat.m31 = m.m31; mat.m32 = m.m32; mat.m33 = m.m33;

            return mat;
        }

        public static Matrix4x4d ToMatrix4x4d(this Matrix4x4 m)
        {
            Matrix4x4d mat = new Matrix4x4d();

            mat.m00 = m.m00; mat.m01 = m.m01; mat.m02 = m.m02; mat.m03 = m.m03;
            mat.m10 = m.m10; mat.m11 = m.m11; mat.m12 = m.m12; mat.m13 = m.m13;
            mat.m20 = m.m20; mat.m21 = m.m21; mat.m22 = m.m22; mat.m23 = m.m23;
            mat.m30 = m.m30; mat.m31 = m.m31; mat.m32 = m.m32; mat.m33 = m.m33;

            return mat;
        }

    }

}

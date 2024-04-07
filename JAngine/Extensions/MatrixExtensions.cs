// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;

namespace JAngine.Extensions;

public static class MatrixExtensions
{
    public static void TryDecomposeTRS(this Matrix4x4 matrix, 
        out Vector3 translation, out Quaternion rotation, out Vector3 scale)
    {
        // Copied from opentk.
        translation = new Vector3(matrix.M14, matrix.M24, matrix.M34);
        Vector3 row1 = new Vector3(matrix.M11, matrix.M12, matrix.M13);
        Vector3 row2 = new Vector3(matrix.M21, matrix.M22, matrix.M23);
        Vector3 row3 = new Vector3(matrix.M31, matrix.M32, matrix.M33);
        scale = new Vector3(row1.Length(), row2.Length(), row3.Length());
        row1 /= scale.X;
        row2 /= scale.Y;
        row3 /= scale.Z;
        rotation = Quaternion.Identity;
        double trace = 0.25 * (scale.X + scale.Y + scale.Z + 1.0);

        if (trace > 0)
        {
            var sq = Math.Sqrt(trace);

            rotation.W = (float)sq;
            sq = 1.0 / (4.0 * sq);
            rotation.X = (float)((row2[2] - row3[1]) * sq);
            rotation.Y = (float)((row3[0] - row1[2]) * sq);
            rotation.Z = (float)((row1[1] - row2[0]) * sq);
        }
        else if (row1[0] > row2[1] && row1[0] > row3[2])
        {
            var sq = 2.0 * Math.Sqrt(1.0 + row1[0] - row2[1] - row3[2]);

            rotation.X = (float)(0.25 * sq);
            sq = 1.0 / sq;
            rotation.W = (float)((row3[1] - row2[2]) * sq);
            rotation.Y = (float)((row2[0] + row1[1]) * sq);
            rotation.Z = (float)((row3[0] + row1[2]) * sq);
        }
        else if (row2[1] > row3[2])
        {
            var sq = 2.0 * Math.Sqrt(1.0 + row2[1] - row1[0] - row3[2]);

            rotation.Y = (float)(0.25 * sq);
            sq = 1.0 / sq;
            rotation.W = (float)((row3[0] - row1[2]) * sq);
            rotation.X = (float)((row2[0] + row1[1]) * sq);
            rotation.Z = (float)((row3[1] + row2[2]) * sq);
        }
        else
        {
            var sq = 2.0 * Math.Sqrt(1.0 + row3[2] - row1[0] - row2[1]);

            rotation.Z = (float)(0.25 * sq);
            sq = 1.0 / sq;
            rotation.W = (float)((row2[0] - row1[1]) * sq);
            rotation.X = (float)((row3[0] + row1[2]) * sq);
            rotation.Y = (float)((row3[1] + row2[2]) * sq);
        }

        float l = rotation.Length();
        rotation.X /= l;
        rotation.Y /= l;
        rotation.Z /= l;
        rotation.W /= l;
    }
}

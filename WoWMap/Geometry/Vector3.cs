﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace WoWMap.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(BinaryReader br)
            : this(br.ReadSingle(), br.ReadSingle(), br.ReadSingle())
        { }

        private float x;
        private float y;
        private float z;

        public float GetDistance(Vector3 location)
        {
            return GetDistance(location.x, location.y, location.z);
        }

        public float GetDistance(float x, float y, float z)
        {
            float dx = this.x - x;
            float dy = this.y - y;
            float dz = this.z - z;

            return (float)Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
        }

        public float GetXRotation(Vector3 location)
        {
            return GetXRotation(location.y, location.z);
        }

        public float GetXRotation(float y, float z)
        {
            float dy = y - this.y;
            float dz = z - this.z;

            return (float)Math.Atan2(dz, dy);
        }

        public float GetYRotation(Vector3 location)
        {
            return GetYRotation(location.x, location.z);
        }

        public float GetYRotation(float x, float z)
        {
            float dx = x - this.x;
            float dz = z - this.z;

            return (float)Math.Atan2(dz, dx);
        }

        public float GetZRotation(Vector3 location)
        {
            return GetZRotation(location.x, location.y);
        }

        public float GetZRotation(float x, float y)
        {
            float dx = x - this.x;
            float dy = y - this.y;

            return (float)Math.Atan2(dy, dx);
        }

        public float GetHorizontalAngle(Vector3 location)
        {
            return GetZRotation(location.x, location.y);
        }

        public float GetHorizontalAngle(float x, float y)
        {
            return GetZRotation(x, y);
        }

        public float GetVerticalAngle(Vector3 location)
        {
            return GetVerticalAngle(location.x, location.y, location.z);
        }

        public float GetVerticalAngle(float x, float y, float z)
        {
            x = this.x - x;
            y = this.y - y;
            z = this.z - z;

            double a = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double b = z;

            double result = -Math.Atan(b / a);

            if (result < -Math.PI / 2)
                result += (Math.PI * 2);

            return (float)result;
        }

        public Vector3 Cross(Vector3 vector)
        {
            var result = new Vector3();

            result.x = y * vector.Z - z * vector.Y;
            result.y = z * vector.X - x * vector.Z;
            result.z = x * vector.Y - y * vector.X;

            return result;
        }

        public float Dot(Vector3 vector)
        {
            if (Length <= 0 || vector.Length <= 0)
                return 0.0f;

            return (x) * (vector.X) + (y) * (vector.Y) + (z) * (vector.Z);
        }

        // WoW -> R+D: x,y,z -> -y,z,-x
        public Vector3 ToRecast()
        {
            return new Vector3(-y, z, -x);
        }

        public Vector3 ToWoW()
        {
            return new Vector3(-z, -x, y);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3 && (Vector3)obj == this;
        }

        public override string ToString()
        {
            return string.Concat(x.ToString("0.##"), ", ", y.ToString("0.##"), ", ", z.ToString("0.##"));
        }

        #region Properties

        public float Length
        {
            get { return (float)Math.Sqrt(x * x + y * y + z * z); }
        }

        public Vector3 Normal
        {
            get
            {
                float length = Length;
                if (length == 0)
                    return Zero;

                return new Vector3(x / length, y / length, z / length);
            }
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        #endregion

        #region Static members

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator *(Vector3 v1, float value)
        {
            return new Vector3(v1.x * value, v1.y * value, v1.z * value);
        }

        public static Vector3 operator /(Vector3 v1, float value)
        {
            return new Vector3(v1.x / value, v1.y / value, v1.z / value);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 Up = new Vector3(0, 0, 1);

        #endregion

        public static Vector3 CrossProduct(Vector3 v1, Vector3 v2)
        {
            return
            (
               new Vector3
               (
                  v1.Y * v2.Z - v1.Z * v2.Y,
                  v1.Z * v2.X - v1.X * v2.Z,
                  v1.X * v2.Y - v1.Y * v2.X
               )
            );
        }

        public static Vector3 TriangleNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            var ab = (b - a);
            var ac = (c - a);
            return Vector3.CrossProduct(ab, ac).Normal;
        }
    }
}

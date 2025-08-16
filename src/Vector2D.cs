using System;
using Godot;
    public class Vector2D
    {
        Vector2 vector;

        public Vector2 getVector2() { return vector; }

        public virtual double X { get { return vector.X; } set { vector.X = (float)value; } }
        public virtual double Y { get { return vector.Y; } set { vector.Y = (float)value; } }



        public double getAngle() { return Math.Atan2(Y, X); }

        public Vector2D()
        {
            vector = new Vector2(0, 0);
        }

        internal static double Rad2Deg(double radians)
        {
            return radians * (180 / Math.PI);
        }

        public Vector2D(double X, double Y)
        {
            vector = new Vector2((float)X, (float)Y);
        }

        public Vector2D(Vector2D vector)
        {
            this.vector = new Vector2((float)vector.X, (float)vector.Y);
        }

        public static Vector2D getVectorWithLengthAndAngle(double length, double angle)
        {
            var X = length * Math.Cos(angle);
            var Y = length * Math.Sin(angle);
            return new Vector2D(X, Y);
        }

        //public double Length() { return vector.Length(); }
        public double Length() 
        {
            //var ret =  Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            return vector.Length();
        }

        public double dot(Vector2D otherVector) { return X * otherVector.X + Y * otherVector.Y; }

        public void Normalize()
        {
            if (vector.Length() != 0)
            {
                vector.Normalized();
                X = vector.X;
                Y = vector.Y;
            }
        }

        public Vector2D rotateVector(double angle)
        {
            Vector2D rotationCenter = new Vector2D(0, 0);
            var x_rotated = (X - rotationCenter.X) * Math.Cos(angle) - (Y - rotationCenter.Y) * Math.Sin(angle) + rotationCenter.X;
            var y_rotated = (X - rotationCenter.X) * Math.Sin(angle) + (Y - rotationCenter.Y) * Math.Cos(angle) + rotationCenter.Y;
            return new Vector2D(x_rotated, y_rotated);
        }

        public Vector2D rotateVector(double angle, Vector2D rotationCenter)
        {

            var x_rotated = (X - rotationCenter.X) * Math.Cos(angle) - (Y - rotationCenter.Y) * Math.Sin(angle) + rotationCenter.X;
            var y_rotated = (X - rotationCenter.X) * Math.Sin(angle) + (Y - rotationCenter.Y) * Math.Cos(angle) + rotationCenter.Y;
            return new Vector2D(x_rotated, y_rotated);
        }

        public void setMagnitude(double magnitude) 
        {
            this.Normalize();
            this.X = this.X * magnitude;
            this.Y = this.Y * magnitude;
        }


        public void limitMagnitude(double maxMagnitude)
        {
            var length = Length();
            var angle = Math.Atan2(Y, X);
            length = Math.Min(length, maxMagnitude);
            X = length * Math.Cos(angle);
            Y = length * Math.Sin(angle);
        }


        public static double Deg2Rad(double degree)
        {
            return degree * (Math.PI / 180);
        }



        public double getAngle(Vector2D v)
        {

            var nominator = X * v.X + Y * v.Y;
            var denominator = Length() * v.Length();
            return Math.Acos(nominator / denominator) - Math.PI / 2;
        }

        public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.X + b.X, a.Y + b.Y);
        public static Vector2D operator *(double a, Vector2D b) => new Vector2D(a * b.X, a * b.Y);
        public static Vector2D operator *(Vector2D b, double a) => new Vector2D(a * b.X, a * b.Y);
        public static Vector2D operator /(double a, Vector2D b) => new Vector2D(b.X / a, b.Y / a);
        public static Vector2D operator /(int a, Vector2D b) => new Vector2D(b.X / a, b.Y / a);

        public static Vector2D operator /(Vector2D b, int a) => new Vector2D(b.X / a, b.Y / a);
        public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator -(Vector2D a) => new Vector2D(-a.X, -a.Y);
        //public static bool operator ==(Vector2D a, Vector2D b) => a.X == b.X && a.Y == b.Y;

        public static bool operator ==(Vector2D a, Vector2D b)
        {
            if (a is null)
            {
                return a is null;
            }



            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Vector2D a, Vector2D b) => a.X != b.X || a.Y != b.Y;

        public override bool Equals(object o)
        {
            if (o == null)
                return false;

            var second = o as Vector2D;

            return vector.X == second.X && vector.Y == second.Y;
        }

        public override int GetHashCode()
        {
            return vector.GetHashCode();
        }
    }


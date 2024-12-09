using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingAdvent
{
    public class Point3DLong
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }

        public long vX { get; set; }
        public long vY { get; set; }
        public long vZ { get; set; }

        public Point3DLong()
        {
        }

        public Point3DLong(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3DLong(long x, long y, long z, long vx, long vy, long vz)
        {
            X = x;
            Y = y;
            Z = z;
            vX = vx;
            vY = vy;
            vZ = vz;
        }

        public void MoveTo(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Move()
        {
            X += vX;
            Y += vY;
            Z += vZ;
        }

        public long DistanceTo(Point3DLong p2)
        {
            return (long) Math.Sqrt((X - p2.X) * (X - p2.X) +
              (Y - p2.Y) * (Y - p2.Y) +
              (Z - p2.Z) * (Z - p2.Z));
        }

        public bool DoLinesIntersect(Point3DLong p2, long low, long high)
        {
            // ax + c = bx + d
            double a = ((double) vY) / ((double)vX);
            double b = ((double) p2.vY) / ((double) p2.vX);
            double c = Y + a * X * -1;
            double d = p2.Y + b * p2.X * -1;

            double ab = a - b;
            if (ab != 0)
            {
                double x = (d - c) / ab;
                double y = a * (d - c) / ab + c;

                double t1 = (x - X) / vX;
                double t2 = (x - p2.X) / p2.vX;

                if (t1 >= 0 && t2 >= 0)
                {
                    if (x > low && x < high &&
                        y > low && y < high)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool DoXLinesIntersectInIntegerTime(Point3DLong p2)
        {
            // ax + c = bx + d

            // vX * time + X == p2.Vx * time + p2.X   <=>
            // time * (vX - p2.Vx) = p2.X - X
            // time = (p2.X - X) / (vX - p2.Vx)

            double denom = (vX - p2.vX);
            if (denom != 0)
            {
                double intersectTime = (p2.X - X) / denom;
                if (intersectTime > 0 && (intersectTime % 1) == 0)
                    return true;
            }
            else
            {
                return p2.X == X;
            }
            return false;
        }

        public bool DoYLinesIntersectInIntegerTime(Point3DLong p2)
        {
            // ax + c = bx + d

            // vX * time + X == p2.Vx * time + p2.X   <=>
            // time * (vX - p2.Vx) = p2.X - X
            // time = (p2.X - X) / (vX - p2.Vx)

            double denom = (vY - p2.vY);
            if (denom != 0)
            {
                double intersectTime = (p2.Y - Y) / denom;
                if (intersectTime > 0 && (intersectTime % 1) == 0)
                    return true;
            }
            else
            {
                return p2.Y == Y;
            }
            return false;
        }

        public bool DoZLinesIntersectInIntegerTime(Point3DLong p2)
        {
            // ax + c = bx + d

            // vX * time + X == p2.Vx * time + p2.X   <=>
            // time * (vX - p2.Vx) = p2.X - X
            // time = (p2.X - X) / (vX - p2.Vx)

            double denom = (vZ - p2.vZ);
            if (denom != 0)
            {
                double intersectTime = (p2.Z - Z) / denom;
                if (intersectTime > 0 && (intersectTime % 1) == 0)
                    return true;
            }
            else
            {
                return p2.Z == Z;
            }
            return false;
        }
    }
}

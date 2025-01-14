﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using L2dotNET.Game.world;

namespace L2dotNET.Game.tools
{
    class Calcs
    {
        public static bool checkIfInRange(int range, L2Object obj1, int x, int y, int z, bool includeZAxis)
        {
            if (obj1 == null)
                return false;
            if (range == -1)
                return true; // not limited

            double rad = obj1.Radius;

            double dx = obj1.X - x;
            double dy = obj1.Y - y;

            if (includeZAxis)
            {
                double dz = obj1.Z - z;
                double d = dx * dx + dy * dy + dz * dz;

                return d <= range * range + 2 * range * rad + rad * rad;
            }
            else
            {
                double d = dx * dx + dy * dy;

                return d <= range * range + 2 * range * rad + rad * rad;
            }
        }

        public static bool checkIfInRange(int range, L2Object obj1, L2Object obj2, bool includeZAxis)
        {
            if (obj1 == null || obj2 == null) return false;
            if (range == -1) return true; // not limited

            double rad = obj1.Radius + obj2.Radius;
            double dx = obj1.X - obj2.X;
            double dy = obj1.Y - obj2.Y;

            if (includeZAxis)
            {
                double dz = obj1.Z - obj2.Z;
                double d = dx * dx + dy * dy + dz * dz;

                return d <= range * range + 2 * range * rad + rad * rad;
            }
            else
            {
                double d = dx * dx + dy * dy;

                return d <= range * range + 2 * range * rad + rad * rad;
            }
        }

        public static double calculateDistance(int x1, int y1, int z1, int x2, int y2) 
        { 
            return calculateDistance(x1, y1, 0, x2, y2, 0, false); 
        }

        public static double calculateDistance(int x1, int y1, int z1, int x2, int y2, int z2, bool includeZAxis)
        {
            double dx = (double)x1 - x2;
            double dy = (double)y1 - y2;

            if (includeZAxis)
            {
                double dz = z1 - z2;
                return Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
            }
            else
                return Math.Sqrt((dx * dx) + (dy * dy));
        }

        public static double calculateDistance(L2Object obj1, L2Object obj2, bool includeZAxis)
        {
            return calculateDistance(obj1.X, obj1.Y, obj1.Z, obj2.X, obj2.Y, obj2.Z, includeZAxis);
        }


    }
}

﻿using System;

namespace DamageMeter
{
    public class Utils
    {
        public static long Now()
        {
            return DateTime.UtcNow.Ticks/10000000;
        }
    }
}
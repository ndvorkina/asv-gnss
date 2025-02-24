﻿using System;

namespace Asv.Gnss
{
    public static class GpsSubFrameFactory
    {
        public static GpsSubframeBase Create(uint[] navBits)
        {
            if (GpsRawHelper.CheckPreamble(navBits) == false)  throw new Exception("Preamble error");
            var subframeId = GpsRawHelper.GetSubframeId(navBits);
            var tow = GpsRawHelper.GetTow15epoch(navBits);
            var data = GpsRawHelper.GetRawDataWithoutParity(navBits);
            GpsSubframeBase subframe;
            switch (subframeId)
            {
                case 1:
                    subframe = new GpsSubframe1();
                    break;
                case 2:
                    subframe = new GpsSubframe2();
                    break;
                case 3:
                    subframe = new GpsSubframe3();
                    break;
                case 4:
                    subframe = new GpsSubframe4();
                    break;
                case 5:
                    subframe = new GPSSubFrame5();
                    break;
                default:
                    throw new Exception($"Unknown GPS subframe ID:{Convert.ToString(subframeId, 2),-8}");
            }
            subframe.Deserialize(data);
            if (tow != subframe.TOW1_5Epoh) throw new Exception("Something goes wrong with byte conversion from uint to byte array.");
            return subframe;
        }
    }
}
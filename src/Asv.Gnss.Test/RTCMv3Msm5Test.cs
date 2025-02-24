﻿using System;
using System.Linq;
using System.Reactive.Linq;
using Xunit;

namespace Asv.Gnss.Test
{
    public class RTCMv3Msm5Test
    {
        [Fact]
        public void TestMsg1075()
        {
            var array = new byte[]
            {
                0xd3,0x00,0x22,0x43,0x30,0x0d,0x2f,0xaf,0xb3,0xe2,0x00,0x20,0x00,0x00,0x04,0x00,0x00,0x00,0x00,0x00,
                0x20,0x00,0x00,0x00,0x55,0x02,0x1e,0xf9,0xf0,0x68,0xc8,0x3a,0x68,0x34,0x78,0x52,0x48,0xb3,0x8e,0xf7
            };
            var parser = new RtcmV3Parser().RegisterExtendedMessages();
            RtcmV3MessageBase msg = null;
            parser.OnMessage.Cast<RtcmV3MessageBase>().Subscribe(_ => msg = _);
            foreach (var p in array)
            {
                parser.Read(p);
            }
            Assert.NotNull(msg);
            Assert.Equal("1075", msg.MessageStringId);
            var msg1075 = msg as RtcmV3Msm5Msg1075;
            Assert.Equal((uint)200011000, msg1075.EpochTimeTow);
            Assert.Equal(1, msg1075.MultipleMessageBit);
            Assert.Equal(0, msg1075.Iods);
            Assert.Equal(0, msg1075.Reserved);
            Assert.Equal((uint)1, msg1075.ClockSteeringIndicator);
            Assert.Equal((uint)0, msg1075.ExternalClockIndicator);
            Assert.Equal((uint)0, msg1075.SmoothingIndicator);
            Assert.Equal((uint)0, msg1075.SmoothingInterval);

            Assert.Single(msg1075.Satellites);
            Assert.Single(msg1075.Satellites[0].Signals);

            var firstSat = msg1075.Satellites[0];
            Assert.Equal(21, firstSat.SatellitePrn);

            Assert.Equal("L1C", firstSat.Signals[0].RinexCode);
            Assert.Equal(1, firstSat.Signals[0].ObservationCode);

            var freq = RtcmV3Helper.Code2Freq(NavigationSystemEnum.SYS_GPS, 1);

            Assert.Equal((84 + 0.529296875 + 0.00019985437393188477) * RtcmV3Helper.RANGE_MS, firstSat.Signals[0].PseudoRange);
            Assert.Equal(10, firstSat.Signals[0].LockTime);
            Assert.Equal(0, firstSat.Signals[0].HalfCycle);
            Assert.Equal(30 + 0.5, firstSat.Signals[0].Cnr);
            Assert.True(((-388 + 0.26330000000000003) * freq / RtcmV3Helper.CLIGHT - firstSat.Signals[0].PhaseRangeRate) < double.Epsilon);
            Assert.True(((84 + 0.0002228040248155594) * freq / RtcmV3Helper.CLIGHT - firstSat.Signals[0].CarrierPhase) < double.Epsilon);

        }

        [Fact]
        public void TestMsg1085()
        {
            var array = new byte[]
            {
                0xd3,0x01,0x7c,0x43,0xd0,0x0d,0x49,0x0e,0xe8,0xa2,0x00,0x20,0x70,0xb8,0x1c,0x00,0x00,0x00,0x00,0x00,
                0x30,0xc2,0x00,0x00,0x7b,0xde,0xf6,0x3d,0xef,0x7b,0xe8,0xa8,0x69,0xea,0x09,0x28,0x08,0xe9,0x89,0x2a,
                0x30,0x79,0xa0,0xed,0x53,0x67,0x53,0x1d,0xbd,0x5b,0xeb,0xe0,0x52,0x90,0x76,0x9e,0x84,0xd4,0x12,0xd7,
                0xd9,0x7e,0x6e,0x86,0xbe,0x14,0x88,0x07,0x7e,0xcf,0x83,0x21,0xf3,0xd7,0x99,0x20,0x95,0x41,0x15,0x02,
                0xa3,0x06,0x0f,0xd7,0x3b,0xae,0x37,0x61,0x6e,0xc2,0xc4,0x69,0x88,0xc1,0x12,0x01,0x23,0xf1,0xc4,0xef,
                0x8a,0x3f,0x2f,0x8e,0x5f,0x46,0x8e,0x4d,0x00,0x96,0xd4,0x2d,0xb6,0x5c,0x14,0xb7,0xa0,0x09,0x80,0x12,
                0xc0,0x49,0xc0,0x90,0x86,0xe4,0x0d,0xf4,0x21,0x78,0x3e,0x17,0x1d,0x0e,0x33,0x9c,0xa2,0xb9,0x3a,0xe4,
                0x03,0xc9,0x39,0x96,0xe7,0x35,0xd6,0x34,0x70,0x06,0x3b,0x00,0x24,0xb3,0x00,0x40,0x3c,0x00,0xb0,0x9f,
                0xa9,0x6e,0xbe,0xa0,0x7b,0xfa,0xb2,0x97,0xe9,0xf2,0x90,0x8a,0x3d,0x82,0x16,0xae,0x08,0x9b,0x7c,0x22,
                0x02,0x4f,0x8e,0xd3,0x3d,0xac,0xe7,0xf7,0x45,0x47,0xd8,0xfd,0xa1,0x2b,0x79,0x03,0x33,0xf7,0x0b,0xb8,
                0x9c,0x2e,0x4f,0xb0,0xbd,0x00,0x02,0xf5,0xac,0xff,0xb5,0xb8,0x04,0x14,0x00,0x14,0xbe,0x40,0x59,0xb6,
                0x03,0xbf,0x5c,0x0f,0xf8,0xb0,0x40,0xad,0x01,0x07,0xc4,0xfa,0x19,0xab,0xe7,0xfe,0x1f,0xa3,0x61,0x3e,
                0x9c,0xac,0xf1,0xc1,0x4f,0xca,0x5f,0xdf,0x2c,0x41,0xbc,0xa5,0x40,0xf2,0x7a,0xd3,0xff,0xff,0xff,0xff,
                0x77,0x77,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0x2f,0x2e,0xc0,0x00,0x00,0x00,0x00,
                0x61,0x85,0x54,0x69,0xa6,0x17,0xc5,0x1c,0x91,0xd1,0x44,0xf2,0xe1,0x86,0x9a,0x69,0x9e,0x59,0x5d,0x6c,
                0x71,0xd5,0x4c,0xd3,0x59,0x64,0x71,0xc5,0x0c,0x9f,0xb1,0x7f,0x62,0xfe,0xcb,0xfd,0xb5,0xcc,0xf7,0x99,
                0xef,0x31,0x2e,0x63,0xbc,0xa6,0x79,0x4c,0xf2,0x12,0xe4,0x8f,0xef,0xaf,0xdf,0x5f,0xc5,0x8f,0x8c,0x3e,
                0x09,0x7c,0x12,0x8a,0x1a,0x14,0x34,0x27,0x5c,0x4e,0x27,0x37,0xce,0x6f,0x9c,0xe5,0x39,0xcf,0x6c,0xff,
                0xd9,0xff,0xb4,0xef,0x6a,0x30,0xef,0xc1,0xdf,0x83,0xb9,0x07,0x7f,0x92,0xf7,0x25,0xee,0x48,0x98,0x99,
                0xc9,0x27,0xc0,0xa2,0xae,0x98
            };
            var parser = new RtcmV3Parser().RegisterExtendedMessages();
            RtcmV3MessageBase msg = null;
            parser.OnMessage.Cast<RtcmV3MessageBase>().Subscribe(_ => msg = _);
            foreach (var p in array)
            {
                parser.Read(p);
            }
            Assert.NotNull(msg);
            Assert.Equal("1085", msg.MessageStringId);
            var msg1085 = msg as RtcmV3Msm5Msg1085;

            Assert.Equal(1, msg1085.MultipleMessageBit);
            Assert.Equal(0, msg1085.Iods);
            Assert.Equal(0, msg1085.Reserved);
            Assert.Equal((uint)1, msg1085.ClockSteeringIndicator);
            Assert.Equal((uint)0, msg1085.ExternalClockIndicator);
            Assert.Equal((uint)0, msg1085.SmoothingIndicator);
            Assert.Equal((uint)0, msg1085.SmoothingInterval);

            Assert.Equal(10, msg1085.Satellites.Length);
            Assert.Equal(5, msg1085.Satellites.Select(s => s.Signals).Max(ss => ss.Length));
            Assert.Equal(39, msg1085.Satellites.Select(s => s.Signals).Sum(ss => ss.Length));

        }

        [Fact]
        public void TestMsg1095()
        {
            var array = new byte[]
            {
                0xd3,0x01,0xa6,0x44,0x70,0x0d,0x2f,0xaf,0xb3,0xe2,0x00,0x20,0x35,0x88,0x00,0xc0,0x40,0x00,0x00,0x00,
                0x04,0x10,0x88,0x80,0x7f,0xff,0xff,0xff,0xff,0xfd,0x49,0x45,0x75,0x79,0x45,0x5d,0x65,0x39,0x70,0x00,
                0x00,0x00,0x00,0x3a,0xc7,0x6c,0x87,0x6a,0x79,0x0f,0x18,0xda,0x6c,0xaf,0xef,0x8f,0x81,0x54,0x09,0x6f,
                0xdc,0x8f,0xc8,0xbe,0x7d,0x07,0xa8,0x01,0xd0,0x10,0xce,0x4e,0x1c,0x9f,0x38,0x8c,0x71,0xb8,0xe7,0x68,
                0x20,0x70,0x3a,0x80,0x7c,0x01,0x05,0x02,0x7f,0xce,0x41,0x9d,0xff,0x3f,0xce,0x82,0x4d,0x15,0xde,0xa1,
                0xfd,0x34,0x7a,0x80,0xf5,0x51,0xec,0x77,0x06,0x3e,0x0e,0x0c,0x04,0x58,0x0f,0xf0,0x57,0x6e,0x29,0xdb,
                0x45,0xb7,0x8b,0x70,0x96,0xe8,0xcd,0xf7,0x7b,0xf2,0x37,0xb0,0xef,0x98,0xe0,0x45,0xa9,0x8f,0x4f,0xb6,
                0xa2,0x3d,0x47,0x7a,0xac,0x0b,0x96,0x97,0x6e,0x2e,0x1e,0x5c,0xcc,0xbd,0x90,0x7a,0xcd,0x61,0xe4,0x85,
                0x07,0x9c,0x52,0x1e,0xa4,0x18,0x7a,0x0b,0xa0,0x2c,0x0e,0x00,0x7c,0x0e,0x01,0xf2,0x58,0x07,0x98,0x00,
                0x1e,0x7a,0xf9,0x40,0x41,0xe4,0x3c,0x8f,0x8f,0xe5,0x9e,0x42,0x60,0xf8,0xf8,0x71,0xfb,0x9b,0x6f,0xe7,
                0x91,0x5f,0x9c,0xee,0x7e,0x6f,0xf3,0xf9,0xcd,0x7f,0x80,0xd7,0x5d,0xfd,0x01,0xf7,0xee,0xdd,0xdf,0xad,
                0xb7,0x7e,0xf1,0x3f,0x17,0xf9,0xfb,0xd0,0x37,0xef,0x4b,0x07,0xbc,0x2b,0xbe,0xf4,0x95,0x7b,0xbd,0x91,
                0xee,0x54,0x2f,0xb9,0x03,0xfe,0xe2,0x27,0x7b,0x8b,0x7b,0xeb,0xac,0xbf,0xaa,0x58,0x7e,0xa9,0x59,0x7a,
                0xb0,0xbf,0xea,0x9c,0x08,0x61,0x77,0x81,0x7a,0x09,0x85,0xea,0x56,0x17,0x9e,0xd8,0x5e,0xfb,0x9f,0xbf,
                0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,
                0xfe,0x00,0x00,0x00,0x00,0x00,0x0c,0x6e,0xd7,0x8d,0x73,0xc7,0x7e,0xb6,0xb2,0x8b,0xf1,0xba,0x18,0x23,
                0x96,0x6c,0xf1,0xdf,0xad,0xe8,0xa6,0xdb,0xec,0xc2,0xfc,0x74,0xc7,0x4d,0x39,0xef,0x8a,0x6a,0xc3,0x2b,
                0xc4,0x59,0x08,0xc8,0x11,0xb2,0x23,0x90,0x46,0xe9,0x32,0x62,0x60,0x44,0xc1,0x89,0x88,0x12,0xf9,0x0d,
                0x5c,0x19,0x40,0x34,0x28,0x68,0xa0,0xd1,0x9e,0xbe,0xfd,0xbb,0xfb,0xb1,0xf7,0x35,0xef,0x50,0x53,0x48,
                0xa5,0xc1,0x4a,0xa2,0x9b,0x05,0x35,0x0e,0xaf,0x1c,0xac,0x38,0xa4,0x72,0xf8,0xe6,0x20,0x3a,0x00,0x5f,
                0xc0,0xc4,0x01,0x8b,0x02,0xe3,0xc6,0xef,0x8b,0x67,0x17,0xde,0x32,0x3c,0x60,0xf8,0xee,0xf2,0x12,0xe4,
                0x09,0xc8,0xeb,0x91,0x70,0xb2,0xc1,0xdb
            };
            var parser = new RtcmV3Parser().RegisterExtendedMessages();
            RtcmV3MessageBase msg = null;
            parser.OnError.Subscribe(_ =>
            {
                Console.WriteLine("ERR:" + _.Message);
            });
            parser.OnMessage.Cast<RtcmV3MessageBase>().Subscribe(_ => msg = _);
            foreach (var p in array)
            {
                parser.Read(p);
            }
            Assert.NotNull(msg);
            Assert.Equal("1095", msg.MessageStringId);
            var msg1095 = msg as RtcmV3Msm5Msg1095;

            Assert.Equal(1, msg1095.MultipleMessageBit);
            Assert.Equal(0, msg1095.Iods);
            Assert.Equal(0, msg1095.Reserved);
            Assert.Equal((uint)1, msg1095.ClockSteeringIndicator);
            Assert.Equal((uint)0, msg1095.ExternalClockIndicator);
            Assert.Equal((uint)0, msg1095.SmoothingIndicator);
            Assert.Equal((uint)0, msg1095.SmoothingInterval);

            Assert.Equal(9, msg1095.Satellites.Length);
            Assert.Equal(5, msg1095.Satellites.Select(s => s.Signals).Max(ss => ss.Length));
            Assert.Equal(45, msg1095.Satellites.Select(s => s.Signals).Sum(ss => ss.Length));

        }

        [Fact]
        public void TestMsg1125()
        {
            var array = new byte[]
            {
                0xd3,0x01,0xc0,0x46,0x50,0x0d,0x2f,0xae,0xd9,0x22,0x00,0x20,0x01,0x20,0x19,0x00,0x04,0x92,0x00,0x00,
                0x20,0x82,0x08,0x90,0xf0,0xe1,0xbf,0x7e,0xfd,0xfb,0xf7,0xef,0x7d,0x7b,0x4e,0x4e,0x4f,0x48,0x7d,0x52,
                0x51,0x00,0x00,0x00,0x00,0x07,0x06,0xac,0x42,0xa6,0x25,0x3b,0x81,0xaa,0xbc,0x36,0x88,0x05,0x00,0x00,
                0xff,0x39,0x05,0xec,0x1c,0xcf,0xf3,0x00,0x84,0xfb,0xf7,0xdd,0xaf,0x61,0x3e,0x4a,0xbc,0xae,0x84,0x44,
                0x06,0x08,0x0d,0x0c,0x09,0x3f,0xf7,0x80,0x09,0xa0,0x29,0x00,0x1b,0x00,0x8a,0xf8,0xf9,0xe9,0x67,0xd8,
                0xf7,0xb7,0x4f,0x60,0xbe,0xfc,0x36,0x1c,0x69,0xe4,0xd4,0xe9,0xab,0x1f,0x52,0xbe,0xba,0xaf,0x6e,0x5e,
                0xdb,0x7d,0xd1,0x7b,0xf1,0xf7,0x13,0xec,0x04,0xf4,0xa1,0xda,0xe3,0xb8,0x67,0x88,0x4e,0xd2,0x1e,0x0b,
                0x20,0xb6,0x3f,0xd0,0x82,0x39,0x09,0xc2,0x03,0x03,0xfa,0x8d,0xba,0x9c,0x6d,0x38,0x66,0x72,0x38,0xe0,
                0x49,0xb5,0xef,0xc6,0x4a,0xff,0x34,0xa9,0xfc,0xf6,0xec,0x07,0xbd,0xe0,0x19,0xef,0xc0,0xb8,0x98,0x00,
                0x23,0xcb,0xfd,0xa1,0x6f,0xfb,0x22,0x7f,0xef,0x8b,0xff,0xb7,0x94,0x00,0x9a,0xcf,0xe8,0xf3,0x7e,0xf1,
                0xd0,0xfc,0x5c,0xcb,0xf3,0x28,0xbf,0xc4,0x1e,0x3f,0x5d,0x8b,0xf5,0xbd,0x67,0xd1,0xc2,0x4f,0x4d,0xc3,
                0x7d,0x4b,0xf5,0xf4,0xe8,0x47,0xd7,0x6b,0x6f,0xeb,0x37,0xbf,0x84,0xef,0xfe,0x20,0x3b,0xf8,0x2c,0xaf,
                0xe0,0x76,0x3f,0x77,0x8c,0x0f,0x15,0x9c,0x39,0xe2,0x00,0xe9,0x01,0x03,0xb8,0xf4,0x0e,0x79,0x40,0x3d,
                0x1f,0xb0,0xa4,0x6f,0x82,0x67,0xb5,0x0a,0x0a,0x34,0x28,0x78,0x40,0x9d,0xe4,0xc2,0x5b,0xc8,0x0f,0x2d,
                0x80,0x3b,0x9c,0x40,0xf1,0x4e,0x83,0xe7,0xbc,0x0e,0xe5,0x04,0x3a,0x07,0xaf,0xff,0xff,0xff,0xff,0xff,
                0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xf0,0x00,
                0x00,0x00,0x00,0x00,0x0b,0x6e,0xc6,0xdb,0xb2,0xb6,0xed,0xf4,0xd2,0xdc,0x70,0xe3,0x6d,0x71,0xc3,0x2e,
                0x35,0xdb,0x0c,0xf3,0xf3,0x9e,0x72,0xbe,0xfd,0xb3,0xce,0xeb,0xef,0xcf,0x0c,0xad,0xba,0xdd,0xb3,0xce,
                0xe1,0x68,0xa2,0xd8,0x45,0xfe,0x93,0x06,0x26,0xb0,0x4c,0xd3,0xf5,0xd7,0xec,0x5f,0xe6,0x3f,0xcc,0x3f,
                0x92,0xff,0x02,0x07,0x1a,0x0d,0x30,0x1a,0x38,0x32,0xe0,0x66,0x00,0xb5,0x49,0x63,0x92,0x27,0x25,0xb2,
                0x4b,0x2c,0x95,0x79,0x33,0x2e,0x01,0xdc,0x0b,0xf8,0x06,0x70,0x15,0xe0,0x61,0xc0,0x3f,0xa8,0x8f,0x49,
                0x2e,0x9e,0x3d,0x25,0xfa,0x58,0x74,0x3a,0x1c,0xa4,0x36,0xb0,0x6d,0xf8,0xdb,0x01,0xca,0x63,0x88,0xf8,
                0x26,0x71,0x8b,0xe1,0x6d,0xc1,0xd3,0x84,0x27,0x05,0x60,0xd6,0x12,0x66
            };

            var parser = new RtcmV3Parser().RegisterExtendedMessages();
            RtcmV3MessageBase msg = null;
            parser.OnError.Subscribe(_ =>
            {
                Console.WriteLine("ERR:" + _.Message);
            });
            parser.OnMessage.Cast<RtcmV3MessageBase>().Subscribe(_ => msg = _);
            foreach (var p in array)
            {
                parser.Read(p);
            }
            Assert.NotNull(msg);
            Assert.Equal("1125", msg.MessageStringId);
            var msg1125 = msg as RtcmV3Msm5Msg1125;

            Assert.Equal(1, msg1125.MultipleMessageBit);
            Assert.Equal(0, msg1125.Iods);
            Assert.Equal(0, msg1125.Reserved);
            Assert.Equal((uint)1, msg1125.ClockSteeringIndicator);
            Assert.Equal((uint)0, msg1125.ExternalClockIndicator);
            Assert.Equal((uint)0, msg1125.SmoothingIndicator);
            Assert.Equal((uint)0, msg1125.SmoothingInterval);

            Assert.Equal(9, msg1125.Satellites.Length);
            Assert.Equal(6, msg1125.Satellites.Select(s => s.Signals).Max(ss => ss.Length));
            Assert.Equal(48, msg1125.Satellites.Select(s => s.Signals).Sum(ss => ss.Length));

        }

    }
}
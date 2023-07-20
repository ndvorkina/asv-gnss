﻿using System;
using System.Reactive.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Asv.Gnss.Test
{
    public class RTCMv3Test
    {
        private readonly ITestOutputHelper _output;

        public RTCMv3Test(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestMsg1()
        {
            var array = new byte[]
            {
                0xD3, 0x00, 0x66, 0x43, 0x50, 0x00, 0x58, 0xBE, 0xDF, 0x42, 
                0x00, 0x00, 0x00, 0x20, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 
                0x28, 0x20, 0x81, 0x00, 0x0F, 0xE8, 0x89, 0x60, 0x0D, 0x3C, 
                0xAA, 0x02, 0x27, 0xF9, 0xFF, 0x3B, 0xDD, 0xF0, 0x29, 0xC3, 
                0x35, 0x24, 0x33, 0x24, 0x63, 0x58, 0x14, 0x35, 0xB6, 0xA3, 
                0x47, 0xED, 0xFD, 0x1E, 0x6D, 0xFC, 0x51, 0x82, 0x0C, 0x9F, 
                0xB0, 0x0C, 0x9F, 0xB0, 0x0D, 0xA2, 0x58, 0x0D, 0x79, 0xAC, 
                0x0D, 0x68, 0x8C, 0xEF, 0x3C, 0x0F, 0x13, 0xC2, 0xF0, 0xBC, 
                0x4F, 0x10, 0x08, 0x86, 0x12, 0x94, 0x9E, 0x77, 0x9E, 0x52, 
                0xA1, 0x3E, 0xC5, 0x80, 0xB1, 0xE8, 0x77, 0xD0, 0xEF, 0xA1, 
                0xB7, 0x5E, 0x6E, 0xAD, 0xE0, 0x10, 0x3D, 0x08,
                
                
            };
            var parser = new RtcmV3Parser().RegisterDefaultMessages();
            RtcmV3MessageBase msg = null;
            parser.OnMessage.Cast<RtcmV3MessageBase>().Subscribe(_ => msg = _);
            foreach (var p in array)
            {
                parser.Read(p);
            }
            Assert.NotNull(msg);
            msg = null;
            foreach (var p in array)
            {
                parser.Read(p);
            }
            Assert.NotNull(msg);
        }

        [Fact]
        public void TestFromData()
        {
            
            var parser = new RtcmV3Parser().RegisterDefaultMessages();
            parser.OnError.Subscribe(_ =>
            {
                //_output.WriteLine("ERR:"+_.Message);
            });
            parser.OnMessage.Subscribe(_ =>
            {
                _output.WriteLine($"[{_.MessageStringId}]=> {_.Name}");
            });
            foreach (var b in TestData.test_rtcm3)
            {
                parser.Read(b);
            }
        }

        [Fact]
        public void TestFromData2()
        {
            var parser = new RtcmV3Parser().RegisterDefaultMessages();
            parser.OnError.Subscribe(_ =>
            {
               //_output.WriteLine("ERR:"+_.Message);
            });
            parser.OnMessage.Subscribe(_ =>
            {
                _output.WriteLine($"[{_.MessageStringId}]=> {_.Name}");
            });
            foreach (var b in TestData.testglo_rtcm3)
            {
                parser.Read(b);
            }
        }

        [Fact]
        public void TestRtcm1020()
        {
            var data = new byte[]
            {
                0xD3, 0x00, 0x2D, 0x3F, 0xC1, 0xD9, 0x8B, 0x98, 0xAF, 0x99, 0xB6, 0x26, 0x88, 0xE1, 0x3C, 0x41, 0x20,
                0xED, 0x31, 0x41, 0x83, 0x4D, 0xC0, 0xA0, 0x1D, 0x36, 0x4A, 0x95, 0x81, 0xD4, 0x80, 0x0D, 0x05, 0xAD,
                0x88, 0x80, 0x18, 0x10, 0xE0, 0x38, 0x00, 0x00, 0x00, 0x11, 0xC0, 0x00, 0x33, 0x80, 0x6D, 0xFB, 0x3A,
                0xD3, 0x00, 0xBA, 0x43, 0x30, 0x00, 0x6E, 0x6C, 0x01, 0x62, 0x00, 0x20, 0x10, 0x00, 0x82, 0x65, 0x80,
                0x00, 0x00, 0x00, 0x20, 0x20, 0x00, 0x00, 0x7F, 0xFF, 0xA6, 0x28, 0x22, 0x25, 0xA3, 0x27, 0xA1, 0xA3,
                0x80, 0x00, 0x00, 0x00, 0x3B, 0x27, 0x90, 0x62, 0xCE, 0x66, 0x6D, 0x1F, 0xEA, 0x04, 0x82, 0x72, 0x17,
                0xA8, 0x03, 0x9E, 0xD2, 0x03, 0xEE, 0x0A, 0x27, 0xFA, 0x5F, 0x56, 0x97, 0x8E, 0x32, 0x85, 0xEB, 0x5F,
                0xE2, 0x97, 0x39, 0xAE, 0x88, 0x7D, 0x23, 0xBA, 0xFF, 0x07, 0x4E, 0x11, 0xA3, 0x91, 0xD3, 0x2C, 0xC6,
                0x2C, 0x3C, 0x6C, 0x7D, 0x1B, 0x3A, 0xD2, 0x8B, 0x31, 0xDC, 0x2C, 0xCE, 0x38, 0x09, 0x71, 0xA0, 0x29,
                0x20, 0x7F, 0x02, 0xDF, 0xFC, 0x08, 0xD7, 0x59, 0xA4, 0x1D, 0x89, 0xCE, 0x01, 0x2C, 0x6A, 0x04, 0xBC,
                0x57, 0x1F, 0x1A, 0x1F, 0xC9, 0x27, 0xF8, 0xB0, 0xE7, 0xE2, 0xCE, 0x0F, 0xD4, 0x18, 0xFF, 0x50, 0xDC,
                0xF7, 0x77, 0x77, 0x77, 0x77, 0x66, 0x77, 0x77, 0x00, 0x00, 0x5D, 0x75, 0x93, 0xE3, 0x8D, 0xD5, 0xE5,
                0x95, 0x94, 0x65, 0x96, 0x9A, 0x04, 0x7A, 0x08, 0xFC, 0x60, 0x78, 0xC1, 0xE0, 0xCD, 0xC1, 0x9B, 0xBE,
                0x14, 0x3C, 0x26, 0x26, 0x73, 0x4C, 0xF0, 0x67, 0x50, 0xCE, 0xAD, 0x93, 0xCB, 0x27, 0xD6, 0xAA, 0xAD,
                0x54, 0x80, 0xCB, 0x03, 0x2B, 0xD3, 0x00, 0x6C, 0x43, 0xD0, 0x00, 0xA9, 0xFE, 0xE6, 0x22, 0x00, 0x20,
                0x03, 0xE0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x40, 0x00, 0x00, 0x5F, 0xC8, 0x48, 0x29, 0x88,
                0x08, 0xE7, 0x9A, 0xA1, 0x2A, 0x15, 0x57, 0x4C, 0xC7, 0xCA, 0xFC, 0x68, 0x09, 0xB8, 0x5E, 0x60, 0x05,
                0x04, 0x2F, 0xA3, 0x2F, 0x9B, 0x6F, 0x3C, 0x2C, 0xB4, 0x99, 0xA0, 0x81, 0x21, 0x82, 0xA2, 0xE5, 0x3D,
                0xE3, 0xA4, 0x77, 0xE6, 0xA3, 0x5F, 0x9A, 0x53, 0x7A, 0x29, 0x85, 0xDA, 0x67, 0x47, 0xAB, 0x0B, 0x7E,
                0xAC, 0x80, 0x75, 0x20, 0x61, 0xDD, 0xDB, 0xBD, 0xDC, 0x01, 0x56, 0x58, 0xD1, 0x66, 0x59, 0x5B, 0xBE,
                0x10, 0xBA, 0x31, 0x74, 0x60, 0x80, 0xA1, 0x03, 0xC3, 0x58, 0x06, 0xB1, 0x1A, 0x70, 0x7E, 0xA3, 0xEB,
                0xD3, 0x00, 0xAA, 0x46, 0x50, 0x00, 0x6E, 0x6B, 0x26, 0xA0, 0x00, 0x20, 0x71, 0x20, 0x89, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x20, 0x02, 0x00, 0x00, 0x7F, 0xFD, 0x3F, 0x3F, 0x3E, 0xBF, 0xC1, 0x3C, 0x25, 0x26,
                0x80, 0x00, 0x00, 0x00, 0x64, 0xCC, 0xF0, 0x2C, 0xCF, 0x61, 0x95, 0x34, 0x08, 0x42, 0x7F, 0xF7, 0xFF,
                0xFF, 0xFF, 0x20, 0x8D, 0x82, 0x12, 0x01, 0x00, 0x20, 0x5F, 0x40, 0x84, 0x26, 0x09, 0xF9, 0xC8, 0xB3,
                0x93, 0x96, 0xAE, 0x1D, 0x64, 0x45, 0x40, 0x0A, 0xA7, 0x09, 0x05, 0x14, 0x92, 0x09, 0xFC, 0x1D, 0xA7,
                0x5A, 0x1F, 0xE1, 0x9F, 0xE7, 0x80, 0xFF, 0x8A, 0x9F, 0xE1, 0xD2, 0xFF, 0x87, 0x42, 0x9D, 0x15, 0x61,
                0x74, 0x56, 0xFE, 0x2C, 0xF1, 0x10, 0xB9, 0x10, 0x00, 0x86, 0x86, 0x02, 0x1C, 0x58, 0x17, 0xA0, 0x38,
                0x5E, 0x99, 0x20, 0x80, 0x68, 0x7C, 0x13, 0x1D, 0x77, 0xDD, 0xDD, 0xDD, 0x55, 0xDD, 0xDC, 0x00, 0x05,
                0xF5, 0xD7, 0x6D, 0xF6, 0xDD, 0x6D, 0x54, 0xE3, 0x7E, 0x59, 0x5C, 0x3E, 0xBA, 0xBF, 0xD8, 0x5B, 0xB2,
                0x30, 0x05, 0xDF, 0xF0, 0xE5, 0x98, 0x8B, 0x4E, 0x14, 0x94, 0x2A, 0x40, 0x21, 0x1C, 0x41, 0x30, 0xC7,
                0x2B, 0x2F, 0x60, 0x62, 0xE3, 0x75,
            };

            var parser = new RtcmV3Parser().RegisterDefaultMessages();
            RtcmV3MessageBase msg = null;
            parser.OnMessage.Cast<RtcmV3MessageBase>().Subscribe(_ => msg = _);
            for (var index = 0; index < data.Length; index++)
            {
                var p = data[index];
                parser.Read(p);
            }

            Assert.NotNull(msg);
        }
    }
}
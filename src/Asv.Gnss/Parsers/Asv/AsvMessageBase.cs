﻿using System;
using Asv.IO;

namespace Asv.Gnss
{
    public abstract class AsvMessageBase:GnssMessageBase<ushort>
    {
        public override string ProtocolId => AsvMessageParser.GnssProtocolId;

        public ushort Sequence { get; set; }
        public byte TargetId { get; set; }
        public byte SenderId { get; set; }

        public override void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            var crcSpan = buffer;

            var sync1 = BinSerialize.ReadByte(ref buffer);
            var sync2 = BinSerialize.ReadByte(ref buffer);
            if (sync1 != AsvMessageParser.Sync1 || sync2 != AsvMessageParser.Sync2)
            {
                throw new Exception($"Error to deserialize {ProtocolId}.{Name}");
            }
            var length = BinSerialize.ReadUShort(ref buffer);
            var crc = AsvCrc16.Calc(crcSpan, length + 10);
            crcSpan = crcSpan.Slice(length + 10);
            var crcOrigin = BinSerialize.ReadUShort(ref crcSpan);
            if (crc != crcOrigin)
            {
                throw new Exception($"Error to deserialize {ProtocolId}.{Name}: CRC error. Want {crc}. Got {crcOrigin}");
            }

            Sequence = BinSerialize.ReadUShort(ref buffer);
            SenderId = BinSerialize.ReadByte(ref buffer);
            TargetId = BinSerialize.ReadByte(ref buffer);
            var msgId = BinSerialize.ReadUShort(ref buffer);
            if (MessageId != msgId)
            {
                throw new Exception($"Error to deserialize {ProtocolId}.{Name}: Message id not equals. Want '{MessageId}. Got '{msgId}''");
            }
            var dataSpan = buffer.Slice(0, length);
            InternalContentDeserialize(ref dataSpan);
            buffer = buffer.Slice(length + 2 /*CRC16*/);
        }

        public override void Serialize(ref Span<byte> buffer)
        {
            var originSpan = buffer;
            BinSerialize.WriteByte(ref buffer, AsvMessageParser.Sync1);
            BinSerialize.WriteByte(ref buffer, AsvMessageParser.Sync2);
            var length = (ushort)InternalGetContentByteSize();
            BinSerialize.WriteUShort(ref buffer, length);
            BinSerialize.WriteUShort(ref buffer, Sequence);
            BinSerialize.WriteByte(ref buffer, SenderId);
            BinSerialize.WriteByte(ref buffer, TargetId);
            BinSerialize.WriteUShort(ref buffer, MessageId);
            InternalContentSerialize(ref buffer);
            var crc = AsvCrc16.Calc(originSpan, length + 10 /*from sync1 to end of data*/);
            BinSerialize.WriteUShort(ref buffer, crc);
        }

        protected abstract void InternalContentDeserialize(ref ReadOnlySpan<byte> buffer);
        protected abstract void InternalContentSerialize(ref Span<byte> buffer);
        protected abstract int InternalGetContentByteSize();

        public override int GetByteSize() => 10 /*HEADER*/  + InternalGetContentByteSize() + 2 /*CRC*/;

        public abstract void Randomize(Random random);

    }
}
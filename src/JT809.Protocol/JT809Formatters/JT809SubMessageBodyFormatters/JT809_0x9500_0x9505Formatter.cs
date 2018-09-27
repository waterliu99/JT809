﻿using JT809.Protocol.JT809Extensions;
using JT809.Protocol.JT809SubMessageBody;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace JT809.Protocol.JT809Formatters.JT809SubMessageBodyFormatters
{
    public class JT809_0x9500_0x9505Formatter : IJT809Formatter<JT809_0x9500_0x9505>
    {
        public JT809_0x9500_0x9505 Deserialize(ReadOnlySpan<byte> bytes, out int readSize)
        {
            int offset = 0;
            JT809_0x9500_0x9505 jT809_0X9500_0X9505 = new JT809_0x9500_0x9505();
            jT809_0X9500_0X9505.AuthenticationCode= JT809BinaryExtensions.ReadBCDLittle(bytes, ref offset,10);
            jT809_0X9500_0X9505.AccessPointName = JT809BinaryExtensions.ReadStringLittle(bytes, ref offset, 20);
            jT809_0X9500_0X9505.UserName = JT809BinaryExtensions.ReadStringLittle(bytes, ref offset,49);
            jT809_0X9500_0X9505.Password = JT809BinaryExtensions.ReadStringLittle(bytes, ref offset,22);
            jT809_0X9500_0X9505.ServerIP = JT809BinaryExtensions.ReadStringLittle(bytes, ref offset, 32);
            jT809_0X9500_0X9505.TcpPort = JT809BinaryExtensions.ReadUInt16Little(bytes, ref offset);
            jT809_0X9500_0X9505.UdpPort = JT809BinaryExtensions.ReadUInt16Little(bytes, ref offset);
            jT809_0X9500_0X9505.EndTime = JT809BinaryExtensions.ReadUTCDateTimeLittle(bytes, ref offset);
            readSize = offset;
            return jT809_0X9500_0X9505;
        }

        public int Serialize(IMemoryOwner<byte> memoryOwner, int offset, JT809_0x9500_0x9505 value)
        {
            offset += JT809BinaryExtensions.WriteBCDLittle(memoryOwner, offset, value.AuthenticationCode,10,20);
            offset += JT809BinaryExtensions.WriteStringLittle(memoryOwner, offset, value.AccessPointName,20);
            offset += JT809BinaryExtensions.WriteStringLittle(memoryOwner, offset, value.UserName, 49);
            offset += JT809BinaryExtensions.WriteStringLittle(memoryOwner, offset, value.Password, 22);
            offset += JT809BinaryExtensions.WriteStringLittle(memoryOwner, offset, value.ServerIP, 32);
            offset += JT809BinaryExtensions.WriteUInt16Little(memoryOwner, offset, value.TcpPort);
            offset += JT809BinaryExtensions.WriteUInt16Little(memoryOwner, offset, value.UdpPort);
            offset += JT809BinaryExtensions.WriteUTCDateTimeLittle(memoryOwner, offset, value.EndTime);
            return offset;
        }
    }
}

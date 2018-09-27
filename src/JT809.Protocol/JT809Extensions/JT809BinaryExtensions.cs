﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Buffers.Binary;
using System.Buffers;

namespace JT809.Protocol.JT809Extensions
{
    public static  partial class JT809BinaryExtensions
    {
        public static string ReadStringLittle(ReadOnlySpan<byte> read, ref int offset, int len)
        {
            string value = encoding.GetString(read.Slice(offset, len).ToArray());
            offset += len;
            return value.Trim('\0');
        }

        public static string ReadStringLittle(ReadOnlySpan<byte> read, ref int offset)
        {
            string value = encoding.GetString(read.Slice(offset).ToArray());
            offset += value.Length;
            return value.Trim('\0');
        }

        public static int ReadInt32Little(ReadOnlySpan<byte> read, ref int offset)
        {
            int value = (read[offset] << 24) | (read[offset + 1] << 16) | (read[offset + 2] << 8) | read[offset + 3];
            offset = offset + 4;
            return value;
        }

        public static uint ReadUInt32Little(ReadOnlySpan<byte> read, ref int offset)
        {
            uint value = (uint)((read[offset] << 24) | (read[offset + 1] << 16) | (read[offset + 2] << 8) | read[offset + 3]);
            offset = offset + 4;
            return value;
        }

        public static ulong ReadUInt64Little(ReadOnlySpan<byte> read, ref int offset)
        {
            ulong value = (ulong)(
                (read[offset] << 56) |
                (read[offset + 1] << 48) |
                (read[offset + 2] << 40) |
                (read[offset + 3] << 32) |
                (read[offset + 4] << 24) |
                (read[offset + 5] << 16) |
                (read[offset + 6] << 8) |
                 read[offset + 7]);
            offset = offset + 8;
            return value;
        }

        public static ushort ReadUInt16Little(ReadOnlySpan<byte> read, ref int offset)
        {
            ushort value = (ushort)((read[offset] << 8) | (read[offset + 1]));
            offset = offset + 2;
            return value;
        }

        public static byte ReadByteLittle(ReadOnlySpan<byte> read, ref int offset)
        {
            byte value = read[offset];
            offset = offset + 1;
            return value;
        }

        public static byte[] ReadBytesLittle(ReadOnlySpan<byte> read, ref int offset, int len)
        {
            ReadOnlySpan<byte> temp = read.Slice(offset, len);
            offset = offset + len;
            return temp.ToArray();
        }

        public static byte[] ReadBytesLittle(ReadOnlySpan<byte> read, ref int offset)
        {
            ReadOnlySpan<byte> temp = read.Slice(offset);
            offset = offset + temp.Length;
            return temp.ToArray();
        }

        /// <summary>
        /// 数字编码 大端模式、高位在前
        /// </summary>
        /// <param name="read"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ReadBigNumberLittle(ReadOnlySpan<byte> read, ref int offset, int len)
        {
            ulong result = 0;
            for (int i = 0; i < len; i++)
            {
                ulong currentData = (ulong)read[offset+i] << (8 * (len - i - 1));
                result += currentData;
            }
            offset += len;
            return result.ToString();
        }

        /// <summary>
        /// 数字编码 小端模式、低位在前
        /// </summary>
        /// <param name="read"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ReadLowNumberLittle(ReadOnlySpan<byte> read, ref int offset, int len)
        {
            ulong result = 0;
            for (int i = 0; i < len; i++)
            {
                ulong currentData = (ulong)read[offset+i] << (8 * i);
                result += currentData;
            }
            offset += len;
            return result.ToString();
        }

        public static int WriteInt32Little(IMemoryOwner<byte> memoryOwner, int offset, int data)
        {
            memoryOwner.Memory.Span[offset] = (byte)(data >> 24);
            memoryOwner.Memory.Span[offset + 1] = (byte)(data >> 16);
            memoryOwner.Memory.Span[offset + 2] = (byte)(data >> 8);
            memoryOwner.Memory.Span[offset + 3] = (byte)data;
            return 4;
        }

        public static int WriteUInt32Little(IMemoryOwner<byte> memoryOwner, int offset, uint data)
        {
            memoryOwner.Memory.Span[offset] = (byte)(data >> 24);
            memoryOwner.Memory.Span[offset + 1] = (byte)(data >> 16);
            memoryOwner.Memory.Span[offset + 2] = (byte)(data >> 8);
            memoryOwner.Memory.Span[offset + 3] = (byte)data;
            return 4;
        }

        public static int WriteUInt64Little(IMemoryOwner<byte> memoryOwner, int offset, ulong data)
        {
            memoryOwner.Memory.Span[offset] = (byte)(data >> 56);
            memoryOwner.Memory.Span[offset + 1] = (byte)(data >> 48);
            memoryOwner.Memory.Span[offset + 2] = (byte)(data >> 40);
            memoryOwner.Memory.Span[offset + 3] = (byte)(data >> 32);
            memoryOwner.Memory.Span[offset + 4] = (byte)(data >> 24);
            memoryOwner.Memory.Span[offset + 5] = (byte)(data >> 16);
            memoryOwner.Memory.Span[offset + 6] = (byte)(data >> 8);
            memoryOwner.Memory.Span[offset + 7] = (byte)data;
            return 8;
        }

        public static int WriteUInt16Little(IMemoryOwner<byte> memoryOwner, int offset, ushort data)
        {
            memoryOwner.Memory.Span[offset] = (byte)(data >> 8);
            memoryOwner.Memory.Span[offset + 1] = (byte)data;
            return 2;
        }

        public static int WriteByteLittle(IMemoryOwner<byte> memoryOwner, int offset, byte data)
        {
            memoryOwner.Memory.Span[offset] = data;
            return 1;
        }

        public static int WriteBytesLittle(IMemoryOwner<byte> memoryOwner, int offset, byte[] data)
        {
            CopyTo(data, memoryOwner.Memory.Span, offset);
            return data.Length;
        }

        public static int WriteStringLittle(IMemoryOwner<byte> memoryOwner, int offset, string data)
        {
            byte[] codeBytes = encoding.GetBytes(data);
            CopyTo(codeBytes, memoryOwner.Memory.Span, offset);
            return codeBytes.Length;
        }

        public static int WriteStringLittle(IMemoryOwner<byte> memoryOwner, int offset, string data, int len)
        {
            byte[] bytes = null;
            if (string.IsNullOrEmpty(data))
            {
                bytes = new byte[0];
            }
            else
            {
                bytes = encoding.GetBytes(data);
            }
            byte[] rBytes = new byte[len];
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i >= len) break;
                rBytes[i] = bytes[i];
            }
            CopyTo(rBytes, memoryOwner.Memory.Span, offset);
            return rBytes.Length;
        }

        public static int WriteStringPadLeftLittle(IMemoryOwner<byte> memoryOwner, int offset, string data, int len)
        {
            data = data.PadLeft(len, '\0');
            byte[] codeBytes = encoding.GetBytes(data);
            CopyTo(codeBytes, memoryOwner.Memory.Span, offset);
            return codeBytes.Length;
        }

        public static int WriteStringPadRightLittle(IMemoryOwner<byte> memoryOwner, int offset, string data, int len)
        {
            data = data.PadRight(len, '\0');
            byte[] codeBytes = encoding.GetBytes(data);
            CopyTo(codeBytes, memoryOwner.Memory.Span, offset);
            return codeBytes.Length;
        }

        /// <summary>
        /// 数字编码 大端模式、高位在前
        /// </summary>
        /// <param name="memoryOwner"></param>
        /// <param name="offset"></param>
        /// <param name="data"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int WriteBigNumberLittle(IMemoryOwner<byte> memoryOwner, int offset, string data, int len)
        {
            ulong number = string.IsNullOrEmpty(data) ? 0 : (ulong)double.Parse(data);
            for (int i = len - 1; i >= 0; i--)
            {
                memoryOwner.Memory.Span[offset+i] = (byte)(number & 0xFF);  //取低8位
                number = number >> 8;
            }
            return len;
        }

        /// <summary>
        /// 数字编码 小端模式、低位在前
        /// </summary>
        /// <param name="memoryOwner"></param>
        /// <param name="offset"></param>
        /// <param name="data"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int WriteLowNumberLittle(IMemoryOwner<byte> memoryOwner, int offset, string data, int len)
        {
            ulong number = string.IsNullOrEmpty(data) ? 0 : (ulong)double.Parse(data);
            for (int i = 0; i < len; i++)
            {
                memoryOwner.Memory.Span[offset + i] = (byte)(number & 0xFF); //取低8位
                number = number >> 8;
            }
            return len;
        }

        public static IEnumerable<byte> ToBytes(this string data, Encoding coding)
        {
            return coding.GetBytes(data);
        }

        public static IEnumerable<byte> ToBytes(this string data)
        {
            return ToBytes(data, encoding);
        }

        public static IEnumerable<byte> ToBytes(this int data, int len)
        {
            List<byte> bytes = new List<byte>();
            int n = 1;
            for (int i = 0; i < len; i++)
            {
                bytes.Add((byte)(data >> 8 * (len - n)));
                n++;
            }
            return bytes;
        }

        public static byte ToBcdByte(this byte buf)
        {
            return (byte)Convert.ToInt32(buf.ToString(), 16);
        }

        /// <summary>
        /// 字节数组字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="separator">默认 " "</param>
        /// <returns></returns>
        public static string ToHexString(this byte[] bytes, string separator = " ")
        {
            return string.Join(separator, bytes.Select(s => s.ToString("X2")));
        }

        /// <summary>
        /// 经纬度
        /// </summary>
        /// <param name="latlng"></param>
        /// <returns></returns>
        public static double ToLatLng(this int latlng)
        {
            return Math.Round(latlng / Math.Pow(10, 6), 6);
        }

        public static void CopyTo(Span<byte> source, Span<byte> destination, int offset)
        {
            for (int i = 0; i < source.Length; i++)
            {
                destination[offset + i] = source[i];
            }
        }
    }
}

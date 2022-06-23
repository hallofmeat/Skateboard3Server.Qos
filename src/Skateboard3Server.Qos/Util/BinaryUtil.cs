using System;
using System.Net;
using System.Text;

namespace Skateboard3Server.Qos.Util;

public static class BinaryUtil
{
    public static byte[] GetBigEndianBytes(short value)
    {
        var valueBytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(valueBytes); //big endian
        }

        return valueBytes;
    }

    public static byte[] GetBigEndianBytes(int value)
    {
        var valueBytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(valueBytes); //big endian
        }

        return valueBytes;
    }

    public static byte[] GetBigEndianIpAddress(IPAddress value)
    {
        //Looks like this is normally in big endian
        var valueBytes = value.GetAddressBytes();
        return valueBytes;
    }

    public static string FormatHex(byte[] buffer, int offset, int size)
    {
        var temp = new byte[size];
        Array.Copy(buffer, offset, temp, 0, size);

        var builder = new StringBuilder($"[{temp.Length}] ");

        // Write the hex
        foreach (var b in temp)
        {
            builder.Append(b.ToString("X2"));
            builder.Append(" ");
        }

        return builder.ToString();
    }
}
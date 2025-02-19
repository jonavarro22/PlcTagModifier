using System;
using System.Threading.Tasks;
using libplctag; // Ensure you have a reference to the managed libplctag assembly

namespace PlcTagModifier
{
    public static class PlcTagCommunicator
    {
        /// <summary>
        /// Reads a PLC tag and returns its value as a string.
        /// The value is retrieved based on the tag type specified in the config.
        /// </summary>
        public static async Task<string> ReadTagAsync(PlcConfig config, string tagName)
        {
            // Create and configure the tag.
            var tag = new Tag()
            {
                Name = tagName,
                Gateway = config.PlcIpAddress,
                Path = $"{config.Channel},{config.Slot}",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };

            try
            {
                // Wrap the blocking call in Task.Run to keep the UI responsive.
                await Task.Run(() => tag.Read());

                // Retrieve the value based on the tag type.
                switch (config.TagType)
                {
                    case PlcTagType.Bit:
                        return tag.GetBit(0).ToString();
                    case PlcTagType.Float32:
                        return tag.GetFloat32(0).ToString();
                    case PlcTagType.Float64:
                        return tag.GetFloat64(0).ToString();
                    case PlcTagType.Int8:
                        return tag.GetInt8(0).ToString();
                    case PlcTagType.Int16:
                        return tag.GetInt16(0).ToString();
                    case PlcTagType.Int32:
                        return tag.GetInt32(0).ToString();
                    case PlcTagType.Int64:
                        return tag.GetInt64(0).ToString();
                    case PlcTagType.UInt8:
                        return tag.GetUInt8(0).ToString();
                    case PlcTagType.UInt16:
                        return tag.GetUInt16(0).ToString();
                    case PlcTagType.UInt32:
                        return tag.GetUInt32(0).ToString();
                    case PlcTagType.UInt64:
                        return tag.GetUInt64(0).ToString();
                    case PlcTagType.String:
                        return tag.GetString(0);
                    default:
                        throw new Exception("Unsupported tag type");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading tag: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Writes a value to a PLC tag.
        /// The provided value is parsed based on the tag type specified in the config.
        /// </summary>
        public static async Task WriteTagAsync(PlcConfig config, string tagName, string value)
        {
            // Create and configure the tag.
            var tag = new Tag()
            {
                Name = tagName,
                Gateway = config.PlcIpAddress,
                Path = $"{config.Channel},{config.Slot}",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };

            try
            {

                tag.Initialize();

                // Wrap the write operation in Task.Run to keep the UI responsive.
                await Task.Run(() =>
                {
                    // Set the tag value based on the tag type.
                    switch (config.TagType)
                    {
                        case PlcTagType.Bit:
                            if (bool.TryParse(value, out bool boolVal))
                            {
                                tag.SetBit(0, boolVal);
                            }
                            else
                            {
                                throw new Exception("Invalid boolean value");
                            }
                            break;
                        case PlcTagType.Float32:
                            if (float.TryParse(value, out float float32Val))
                            {
                                tag.SetFloat32(0, float32Val);
                            }
                            else
                            {
                                throw new Exception("Invalid float32 value");
                            }
                            break;
                        case PlcTagType.Float64:
                            if (double.TryParse(value, out double float64Val))
                            {
                                tag.SetFloat64(0, float64Val);
                            }
                            else
                            {
                                throw new Exception("Invalid float64 value");
                            }
                            break;
                        case PlcTagType.Int8:
                            if (sbyte.TryParse(value, out sbyte int8Val))
                            {
                                tag.SetInt8(0, int8Val);
                            }
                            else
                            {
                                throw new Exception("Invalid int8 value");
                            }
                            break;
                        case PlcTagType.Int16:
                            if (short.TryParse(value, out short int16Val))
                            {
                                tag.SetInt16(0, int16Val);
                            }
                            else
                            {
                                throw new Exception("Invalid int16 value");
                            }
                            break;
                        case PlcTagType.Int32:
                            if (int.TryParse(value, out int int32Val))
                            {
                                tag.SetInt32(0, int32Val);
                            }
                            else
                            {
                                throw new Exception("Invalid int32 value");
                            }
                            break;
                        case PlcTagType.Int64:
                            if (long.TryParse(value, out long int64Val))
                            {
                                tag.SetInt64(0, int64Val);
                            }
                            else
                            {
                                throw new Exception("Invalid int64 value");
                            }
                            break;
                        case PlcTagType.UInt8:
                            if (byte.TryParse(value, out byte uint8Val))
                            {
                                tag.SetUInt8(0, uint8Val);
                            }
                            else
                            {
                                throw new Exception("Invalid uint8 value");
                            }
                            break;
                        case PlcTagType.UInt16:
                            if (ushort.TryParse(value, out ushort uint16Val))
                            {
                                tag.SetUInt16(0, uint16Val);
                            }
                            else
                            {
                                throw new Exception("Invalid uint16 value");
                            }
                            break;
                        case PlcTagType.UInt32:
                            if (uint.TryParse(value, out uint uint32Val))
                            {
                                tag.SetUInt32(0, uint32Val);
                            }
                            else
                            {
                                throw new Exception("Invalid uint32 value");
                            }
                            break;
                        case PlcTagType.UInt64:
                            if (ulong.TryParse(value, out ulong uint64Val))
                            {
                                tag.SetUInt64(0, uint64Val);
                            }
                            else
                            {
                                throw new Exception("Invalid uint64 value");
                            }
                            break;
                        case PlcTagType.String:
                            tag.SetString(0, value);
                            break;
                        default:
                            throw new Exception("Unsupported tag type");
                    }

                    // Write the new value to the PLC.
                    tag.Write();
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing tag: {ex.Message}", ex);
            }
        }
    }
}

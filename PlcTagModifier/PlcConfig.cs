namespace PlcTagModifier
{
    public static class PlcTagType
    {
        public const string Bit = "Bit";
        public const string Float32 = "Float32";
        public const string Float64 = "Float64";
        public const string Int8 = "Int8";
        public const string Int16 = "Int16";
        public const string Int32 = "Int32";
        public const string Int64 = "Int64";
        public const string UInt8 = "UInt8";
        public const string UInt16 = "UInt16";
        public const string UInt32 = "UInt32";
        public const string UInt64 = "UInt64";
        public const string String = "String";
    }

    public class PlcConfig
    {
        public string PlcIpAddress { get; set; } = "10.0.241.12";
        public int Port { get; set; } = 44818;
        public int Channel { get; set; } = 1;
        public int Slot { get; set; } = 1;
        public string DefaultTag { get; set; } = "HMI.PLCWatchdogCounter";
        public int Timeout { get; set; } = 10000;
        public string TagType { get; set; } = PlcTagType.Int32;
    }
}

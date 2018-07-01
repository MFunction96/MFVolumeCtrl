// ReSharper disable InconsistentNaming

namespace PInvoke.Enums
{
    public enum WAIT_STATUS : uint
    {
        WAIT_ABANDONED = 0x00000080,
        WAIT_IO_COMPLETION = 0x000000C0,
        WAIT_OBJECT_0 = 0x00000000,
        WAIT_TIMEOUT = 0x00000102,
        WAIT_FAILED = 0xFFFFFFFF
    }
}

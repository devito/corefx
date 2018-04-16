
using System.Runtime.InteropServices;

namespace System
{
    [System.Security.SuppressUnmanagedCodeSecurityAttribute()]
    internal static class UnsafeNclNativeMethods
    {
#if FEATURE_PAL
#if !PLATFORM_UNIX
        internal const String DLLPREFIX = "";
        internal const String DLLSUFFIX = ".dll";
#else // !PLATFORM_UNIX
#if __APPLE__
        internal const String DLLPREFIX = "lib";
        internal const String DLLSUFFIX = ".dylib";
#elif _AIX
        internal const String DLLPREFIX = "lib";
        internal const String DLLSUFFIX = ".a";
#elif __hppa__ || IA64
        internal const String DLLPREFIX = "lib";
        internal const String DLLSUFFIX = ".sl";
#else
        internal const String DLLPREFIX = "lib";
        internal const String DLLSUFFIX = ".so";
#endif
#endif // !PLATFORM_UNIX

        internal const String ROTOR_PAL   = DLLPREFIX + "rotor_pal" + DLLSUFFIX;
        internal const String ROTOR_PALRT = DLLPREFIX + "rotor_palrt" + DLLSUFFIX;
        private const String KERNEL32 = ROTOR_PAL;
#else
        private const string KERNEL32 = "kernel32.dll";
#endif // !FEATURE_PAL

#if !FEATURE_PAL
        private const string WS2_32 = "ws2_32.dll";
#else
        private const string WS2_32 = ExternDll.Kernel32; // Resolves to rotor_pal
#endif // !FEATURE_PAL

        private const string SECUR32 = "secur32.dll";
        private const string CRYPT32 = "crypt32.dll";
        private const string ADVAPI32 = "advapi32.dll";
        private const string HTTPAPI = "httpapi.dll";
        private const string SCHANNEL = "schannel.dll";
        private const string RASAPI32 = "rasapi32.dll";
        private const string WININET = "wininet.dll";
        private const string WINHTTP = "winhttp.dll";
        private const string BCRYPT = "bcrypt.dll";
        private const string USER32 = "user32.dll";
        private const string TOKENBINDING = "tokenbinding.dll";

#if !FEATURE_PAL
        private const string OLE32 = "ole32.dll";
#endif

#if !FEATURE_PAL
        internal const int CLSCTX_SERVER = 0x15;
        [DllImport(OLE32, PreserveSig = false)]
        public static extern void CoCreateInstance(
            [In] ref System.Guid clsid,
            System.IntPtr pUnkOuter,
            int context,
            [In] ref System.Guid iid,
            [MarshalAs(UnmanagedType.IUnknown)] out System.Object o);
#endif // !FEATURE_PAL

#pragma warning disable BCL0015 // Disable Pinvoke analyzer errors.
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr Memmove(System.IntPtr dest, System.IntPtr src, System.UIntPtr count);
#pragma warning restore BCL0015
    }
}

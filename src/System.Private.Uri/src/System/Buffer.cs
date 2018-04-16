
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
    internal class LegacyBuffer
    {
        // This is tricky to get right AND fast, so lets make it useful for the whole Fx.
        // E.g. System.Runtime.WindowsRuntime!WindowsRuntimeBufferExtensions.MemCopy uses it.

        // This method has a slightly different behavior on arm and other platforms.
        // On arm this method behaves like memcpy and does not handle overlapping buffers.
        // While on other platforms it behaves like memmove and handles overlapping buffers.
        // This behavioral difference is unfortunate but intentional because
        // 1. This method is given access to other internal dlls and this close to release we do not want to change it.
        // 2. It is difficult to get this right for arm and again due to release dates we would like to visit it later.
        [System.Security.SecurityCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
#if ARM
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal unsafe static extern void Memcpy(byte* dest, byte* src, int len);
#else // ARM
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        internal unsafe static void Memcpy(byte* dest, byte* src, int len)
        {
            Contract.Assert(len >= 0, "Negative length in memcopy!");
            Memmove(dest, src, (uint)len);
        }
#endif // ARM

        // This method has different signature for x64 and other platforms and is done for performance reasons.
        [System.Security.SecurityCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
#if WIN64
        internal unsafe static void Memmove(byte* dest, byte* src, ulong len)
#else
        internal unsafe static void Memmove(byte* dest, byte* src, uint len)
#endif
        {
            // P/Invoke into the native version when the buffers are overlapping and the copy needs to be performed backwards
            // This check can produce false positives for lengths greater than Int32.MaxInt. It is fine because we want to use PInvoke path for the large lengths anyway.
#if WIN64
            if ((ulong)dest - (ulong)src < len) goto PInvoke;
#else
            if (((uint)dest - (uint)src) < len) goto PInvoke;
#endif
            //
            // This is portable version of memcpy. It mirrors what the hand optimized assembly versions of memcpy typically do.
            //
            // Ideally, we would just use the cpblk IL instruction here. Unfortunately, cpblk IL instruction is not as efficient as
            // possible yet and so we have this implementation here for now.
            //

            switch (len)
            {
                case 0:
                    return;
                case 1:
                    *dest = *src;
                    return;
                case 2:
                    *(short*)dest = *(short*)src;
                    return;
                case 3:
                    *(short*)dest = *(short*)src;
                    *(dest + 2) = *(src + 2);
                    return;
                case 4:
                    *(int*)dest = *(int*)src;
                    return;
                case 5:
                    *(int*)dest = *(int*)src;
                    *(dest + 4) = *(src + 4);
                    return;
                case 6:
                    *(int*)dest = *(int*)src;
                    *(short*)(dest + 4) = *(short*)(src + 4);
                    return;
                case 7:
                    *(int*)dest = *(int*)src;
                    *(short*)(dest + 4) = *(short*)(src + 4);
                    *(dest + 6) = *(src + 6);
                    return;
                case 8:
#if WIN64
                *(long*)dest = *(long*)src;
#else
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
#endif
                    return;
                case 9:
#if WIN64
                *(long*)dest = *(long*)src;
#else
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
#endif
                    *(dest + 8) = *(src + 8);
                    return;
                case 10:
#if WIN64
                *(long*)dest = *(long*)src;
#else
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
#endif
                    *(short*)(dest + 8) = *(short*)(src + 8);
                    return;
                case 11:
#if WIN64
                *(long*)dest = *(long*)src;
#else
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
#endif
                    *(short*)(dest + 8) = *(short*)(src + 8);
                    *(dest + 10) = *(src + 10);
                    return;
                case 12:
#if WIN64
                *(long*)dest = *(long*)src;
#else
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
#endif
                    *(int*)(dest + 8) = *(int*)(src + 8);
                    return;
                case 13:
#if WIN64
                *(long*)dest = *(long*)src;
#else
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
#endif
                    *(int*)(dest + 8) = *(int*)(src + 8);
                    *(dest + 12) = *(src + 12);
                    return;
                case 14:
#if WIN64
                *(long*)dest = *(long*)src;
#else
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
#endif
                    *(int*)(dest + 8) = *(int*)(src + 8);
                    *(short*)(dest + 12) = *(short*)(src + 12);
                    return;
                case 15:
#if WIN64
                *(long*)dest = *(long*)src;
#else
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
#endif
                    *(int*)(dest + 8) = *(int*)(src + 8);
                    *(short*)(dest + 12) = *(short*)(src + 12);
                    *(dest + 14) = *(src + 14);
                    return;
                case 16:
#if WIN64
                *(long*)dest = *(long*)src;
                *(long*)(dest + 8) = *(long*)(src + 8);
#else
                    *(int*)dest = *(int*)src;
                    *(int*)(dest + 4) = *(int*)(src + 4);
                    *(int*)(dest + 8) = *(int*)(src + 8);
                    *(int*)(dest + 12) = *(int*)(src + 12);
#endif
                    return;
                default:
                    break;
            }

            // P/Invoke into the native version for large lengths
            if (len >= 512) goto PInvoke;

            if (((int)dest & 3) != 0)
            {
                if (((int)dest & 1) != 0)
                {
                    *dest = *src;
                    src++;
                    dest++;
                    len--;
                    if (((int)dest & 2) == 0)
                        goto Aligned;
                }
                *(short*)dest = *(short*)src;
                src += 2;
                dest += 2;
                len -= 2;
                Aligned:;
            }

#if WIN64
            if (((int)dest & 4) != 0)
            {
                *(int *)dest = *(int *)src;
                src += 4;
                dest += 4;
                len -= 4;
            }
#endif

#if WIN64
            ulong count = len / 16;
#else
            uint count = len / 16;
#endif
            while (count > 0)
            {
#if WIN64
                ((long*)dest)[0] = ((long*)src)[0];
                ((long*)dest)[1] = ((long*)src)[1];
#else
                ((int*)dest)[0] = ((int*)src)[0];
                ((int*)dest)[1] = ((int*)src)[1];
                ((int*)dest)[2] = ((int*)src)[2];
                ((int*)dest)[3] = ((int*)src)[3];
#endif
                dest += 16;
                src += 16;
                count--;
            }

            if ((len & 8) != 0)
            {
#if WIN64
                ((long*)dest)[0] = ((long*)src)[0];
#else
                ((int*)dest)[0] = ((int*)src)[0];
                ((int*)dest)[1] = ((int*)src)[1];
#endif
                dest += 8;
                src += 8;
            }
            if ((len & 4) != 0)
            {
                ((int*)dest)[0] = ((int*)src)[0];
                dest += 4;
                src += 4;
            }
            if ((len & 2) != 0)
            {
                ((short*)dest)[0] = ((short*)src)[0];
                dest += 2;
                src += 2;
            }
            if ((len & 1) != 0)
                *dest = *src;

            return;

            PInvoke:
            _Memmove(dest, src, len);

        }

        // dest and src are pinned by caller
        private static unsafe void _Memmove(byte* dest, byte* src, uint len)
        {
            UnsafeNclNativeMethods.Memmove(new System.IntPtr(dest), new System.IntPtr(src), new System.UIntPtr(len));
        }
    }
}

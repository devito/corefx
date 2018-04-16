//---------------------------------------------------------------------------------------------------------------------
// <copyright file="IInternetSecurityManager.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------

namespace System
{
    using System.Runtime.InteropServices;

    [ComImport, ComVisible(false), Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b"), System.Runtime.InteropServices.InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IInternetSecurityManager
    {
        unsafe void SetSecuritySite(void* pSite);
        unsafe void GetSecuritySite( /* [out] */ void** ppSite);

        [System.Security.SuppressUnmanagedCodeSecurity]
        void MapUrlToZone(
                            [In, MarshalAs(UnmanagedType.BStr)]
                                    string pwszUrl,
                            [Out]   out int pdwZone,
                            [In]    int dwFlags);

        unsafe void GetSecurityId(  /* [in] */ string pwszUrl,
                            /* [size_is][out] */ byte* pbSecurityId,
                            /* [out][in] */ int* pcbSecurityId,
                            /* [in] */ int dwReserved);

        unsafe void ProcessUrlAction(
                            /* [in] */ string pwszUrl,
                            /* [in] */ int dwAction,
                            /* [size_is][out] */ byte* pPolicy,
                            /* [in] */ int cbPolicy,
                            /* [in] */ byte* pContext,
                            /* [in] */ int cbContext,
                            /* [in] */ int dwFlags,
                            /* [in] */ int dwReserved);

        unsafe void QueryCustomPolicy(
                            /* [in] */ string pwszUrl,
                            /* [in] */ /*REFGUID*/ void* guidKey,
                            /* [size_is][size_is][out] */ byte** ppPolicy,
                            /* [out] */ int* pcbPolicy,
                            /* [in] */ byte* pContext,
                            /* [in] */ int cbContext,
                            /* [in] */ int dwReserved);

        unsafe void SetZoneMapping( /* [in] */ int dwZone, /* [in] */ string lpszPattern, /* [in] */ int dwFlags);

        unsafe void GetZoneMappings( /* [in] */ int dwZone, /* [out] */ /*IEnumString*/ void** ppenumString, /* [in] */ int dwFlags);
    }
}

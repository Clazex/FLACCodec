using System;
using System.Runtime.InteropServices;

[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]

namespace FLACCodec;

// Contains low-level methods directly imported from Rust side
internal static class Native {
	private const string LIB_NAME = "flac_codec_native";

	// Probe the FLAC for metadata
	[DllImport(LIB_NAME, EntryPoint = "probe", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
	public static extern ProbeResult Probe(IntPtr data, int dataSize);

	// Decode the FLAC into given float[]
	[DllImport(LIB_NAME, EntryPoint = "decode", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
	[return: MarshalAs(UnmanagedType.U1)]
	public static extern bool Decode(IntPtr reader, IntPtr buffer, int bufferSize);
}

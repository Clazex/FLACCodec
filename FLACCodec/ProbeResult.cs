using System;
using System.Runtime.InteropServices;

namespace FLACCodec;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct ProbeResult {
	[MarshalAs(UnmanagedType.U1)]
	public readonly bool success;
	public readonly int lengthSamples;
	public readonly int channels;
	public readonly int frequency;
	public readonly IntPtr reader;
}

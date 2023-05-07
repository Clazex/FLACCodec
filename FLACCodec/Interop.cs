using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FLACCodec;

// Contains mid-level code that translates low-level error into C# exceptions,
// and transmits data between C# side and Rust side.
internal static class Interop {
	internal static ProbeResult Probe(IntPtr data, int dataSize) {
		ProbeResult result;

		try {
			result = Native.Probe(data, dataSize);
		} catch (Exception e) when (e is not DllNotFoundException or EntryPointNotFoundException) {
			throw new InvalidDataException("Invalid FLAC audio", e);
		}

		if (!result.success) {
			throw new InvalidDataException("Invalid FLAC audio");
		}

		return result;
	}

	internal static void Decode(ProbeResult probeResult, float[] buffer) {
		var hBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
		IntPtr pBuffer = hBuffer.AddrOfPinnedObject();
		bool success = false;

		try {
			success = Native.Decode(probeResult.reader, pBuffer, buffer.Length);
		} catch (Exception e) when (e is not DllNotFoundException or EntryPointNotFoundException) {
			throw new InvalidDataException("Invalid FLAC audio", e);
		} finally {
			hBuffer.Free();
		}

		if (!success) {
			throw new InvalidDataException("Invalid FLAC audio");
		}
	}
}

using UnityEngine;

using FLACCodec;
using System.Runtime.InteropServices;
using System;
using System.IO;

/// <summary>
/// Contains methods for decoding FLAC audios.
/// </summary>
public static class FLACDecoder {
	/// <summary>
	/// Create a <see cref="AudioClip"/> from the FLAC audio in
	/// <paramref name="data"/>.
	/// </summary>
	/// <param name="data">Array that contains audio data</param>
	/// <param name="name">Name to be given to the clip</param>
	/// <returns>Created <see cref="AudioClip"/></returns>
	/// <exception cref="InvalidDataException">
	/// Throws when fails to decode audio data
	/// </exception>
	/// <exception cref="DllNotFoundException">
	/// Throws when native DLL is not found
	/// </exception>
	/// <exception cref="EntryPointNotFoundException">
	/// Throws when using incompatible version of native DLL
	/// </exception>
	public static AudioClip ToAudioClip(byte[] data, string name = "FLAC Audio") {
		var hData = GCHandle.Alloc(data, GCHandleType.Pinned);
		IntPtr pData = hData.AddrOfPinnedObject();

		ProbeResult probeResult;
		float[] buffer;

		try {
			probeResult = Interop.Probe(pData, data.Length);
			buffer = new float[probeResult.lengthSamples * probeResult.channels];
			Interop.Decode(probeResult, buffer);
		} finally {
			hData.Free();
		}

		var clip = AudioClip.Create(name, probeResult.lengthSamples, probeResult.channels, probeResult.frequency, false);
		_ = clip.SetData(buffer, 0);

		return clip;
	}
}

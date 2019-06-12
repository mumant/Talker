using System;
using System.Collections.Generic;

namespace Wave
{

	static class WavePCMParser
	{
		private enum CompressionType : short
		{
			PCM = 1 // Pulse Code Modulation
		}

		private enum SectionType : uint
		{
			End = 0,
			RIFF = 0x46464952, // "RIFF"
			WAVE = 0x45564157, // "WAVE"
			Fmt = 0x20746d66,  // "fmt "
			Data = 0x61746164, // "data"
		}

		public struct Section
		{
			public uint size;
			public int indexDataStart;
		}

		public struct WaveInfo
		{
			public ushort channels; // Количество каналов. Моно = 1, Стерео = 2 и т.д.
			public int sampleRate; // Частота дискретизации. 8000 Гц, 44100 Гц и т.д.
			public ushort bytesPerSample;
			public uint samplesCount; // Количество сэмплов на каждом канале
			public Section sectionData;
		}

		private const int _sectionRIFFIndex = 0;
		private const int _sectionWAVEIndex = 8;
		private const int _sectionFieldNameSize = 4;
		private const int _sectionFieldSizeDataSize = 4;

		private const int _fmtFieldPCMOffset = 0;
		private const int _fmtFieldChannelsOffset = 2;
		private const int _fmtFieldSampleRateOffset = 4;
		private const int _fmtFieldBitsPerSampleOffset = 14;

		public static bool GetWaveInfo(byte[] waveData, out WaveInfo waveInfo)
		{
			/*
			Пример структуры WAV файла:

			Местоположение 		Поле 			Описание
			0..3 (4 байта) 		chunkRIFF 		Секция RIFF. Содержит символы "RIFF" в ASCII кодировке (0x52494646 в big-endian представлении)
			4..7 (4 байта) 		chunkRIFFSize 	Размер данных секции RIFF со следующего поля (размер файла – 8)
				8..11 (4 байта) format 			Секция WAVE. Содержит символы "WAVE" (0x57415645 в big-endian представлении)

					Подсекции WAVE могут находится в разном порядке:

					12..15 (4 байта) 	subchunkFmt 		Секция fmt. Содержит символы "fmt " (0x666d7420 в big-endian представлении)
					16..19 (4 байта) 	subchunkFmtSize		Размер секции fmt со следующего поля
					20..21 (2 байта) 	audioFormat 		Аудио формат, полный список можно получить здесь. Для PCM = 1 (то есть, Линейное квантование). Значения, отличающиеся от 1, обозначают некоторый формат сжатия
					22..23 (2 байта) 	numChannels 		Количество каналов. Моно = 1, Стерео = 2 и т.д.
					24..27 (4 байта) 	sampleRate 			Частота дискретизации. 8000 Гц, 44100 Гц и т.д.
					28..31 (4 байта) 	byteRate 			Количество байт, переданных за секунду воспроизведения
					32..33 (2 байта) 	blockAlign 			Количество байт для одного сэмпла, включая все каналы
					34..35 (2 байта) 	bitsPerSample 		Количество бит в сэмпле. Так называемая "глубина" или точность звучания. 8 бит, 16 бит и т.д.
					36..37 (2 байта)	extraFormatBytes	Дополнительные данные формата

					38..41 (4 байта) 	subchunkData 		Секция data. Содержит символы "data" (0x64617461 в big-endian представлении)
					42..45 (4 байта) 	subchunkDataSize 	Размер секции data со следующего поля
					46.. 				data 				Непосредственно WAV-данные
			*/

			waveInfo = new WaveInfo();

			if (waveData.Length < _sectionFieldNameSize + _sectionFieldSizeDataSize + _sectionFieldNameSize + _sectionFieldSizeDataSize)
			{
				return false;
			}

			// Проверка секции RIFF
			if (BitConverter.ToUInt32(waveData, _sectionRIFFIndex) != (uint)SectionType.RIFF)
			{
				return false;
			}

			// Проверка секции WAVE
			if (BitConverter.ToUInt32(waveData, _sectionWAVEIndex) != (uint)SectionType.WAVE)
			{
				return false;
			}

			var subsections = new Dictionary<SectionType, Section>();

			{ // Поиск вложенных секций
				int index = _sectionWAVEIndex + _sectionFieldNameSize;

				while (index < waveData.Length)
				{
					SectionType sectionType = (SectionType)BitConverter.ToUInt32(waveData, index);

					if (sectionType == SectionType.End)
					{
						break;
					}

					Section section;
					section.size = BitConverter.ToUInt32(waveData, index + _sectionFieldNameSize);
					section.indexDataStart = index + _sectionFieldNameSize + _sectionFieldSizeDataSize;

					subsections.Add(sectionType, section);

					index = section.indexDataStart + (int)section.size;
				}
			}

			{ // Проверка секции fmt
				Section sectionFmt;
				if (!subsections.TryGetValue(SectionType.Fmt, out sectionFmt))
				{
					return false;
				}

				// Проверка на тип PCM
				if ((CompressionType)BitConverter.ToUInt16(waveData, sectionFmt.indexDataStart + _fmtFieldPCMOffset) != CompressionType.PCM)
				{
					return false;
				}

				// Количество байт на сэмпл
				waveInfo.bytesPerSample = (ushort)(BitConverter.ToUInt16(waveData, sectionFmt.indexDataStart + _fmtFieldBitsPerSampleOffset) / 8);

				// Количество каналов
				waveInfo.channels = BitConverter.ToUInt16(waveData, sectionFmt.indexDataStart + _fmtFieldChannelsOffset);

				// Частота дискретизации
				waveInfo.sampleRate = BitConverter.ToInt32(waveData, sectionFmt.indexDataStart + _fmtFieldSampleRateOffset);
			}

			{ // Проверка секции data
				if (!subsections.TryGetValue(SectionType.Data, out waveInfo.sectionData))
				{
					return false;
				}

				if (waveInfo.channels > 0)
				{
					waveInfo.samplesCount = waveInfo.sectionData.size / waveInfo.bytesPerSample / waveInfo.channels;
				}
			}

			return true;
		}

		private static int FindData(byte[] srcData, int srcStartIndex, int srcRegionSize, byte[] soughtData)
		{
			// endIndex - последний включаемый
			// srcRegionSize = -1 - поиск до конца srcData

			if (srcData.Length < 1 || soughtData.Length < 1)
			{
				return -1;
			}

			if (srcRegionSize == -1)
			{
				srcRegionSize = srcData.Length - srcStartIndex;
			}

			if (srcRegionSize < 1)
			{
				return -1;
			}

			int endSearchIndex = Math.Min(srcStartIndex + srcRegionSize, srcData.Length);

			if (srcStartIndex >= endSearchIndex)
			{
				return -1;
			}

			if (endSearchIndex - srcStartIndex < soughtData.Length)
			{
				return -1;
			}

			endSearchIndex -= soughtData.Length - 1;

			for (int i = srcStartIndex; i < endSearchIndex; ++i)
			{
				bool found = true;

				for (int a = 0; a < soughtData.Length; ++a)
				{
					if (srcData[i + a] != soughtData[a])
					{
						found = false;
						break;
					}
				}

				if (found)
				{
					return i;
				}
			}

			return -1;
		}
	}

}
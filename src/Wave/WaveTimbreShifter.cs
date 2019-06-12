using System;

namespace Wave
{

	static class WaveTimbreShifter
	{
		private const float _normalizeCoeff = 32768f;

		static public bool Shift(byte[] waveData, float pitchShiftCoeff)
		{
			// pitchShiftCoeff: 0.5f - 2f
			// 0.5f - �� ���� ������ ����
			// 2f - �� ���� ������ ����

			WavePCMParser.WaveInfo waveInfo;

			if (!WavePCMParser.GetWaveInfo(waveData, out waveInfo))
			{
				return false;
			}

			// �������������� ������ ��� 16 ���
			if (waveInfo.bytesPerSample != 2)
			{
				return false;
			}

			float[][] channelsData = ConvertWaveDataToFloatArrays(waveData, ref waveInfo);

			foreach (var channelData in channelsData)
			{
				PitchShifter.PitchShift(pitchShiftCoeff, waveInfo.samplesCount, 1024, 10, waveInfo.sampleRate, channelData);
			}

			ConvertFloatArraysToWaveData(waveData, ref waveInfo, channelsData);

			return true;
		}

		private static float[][] ConvertWaveDataToFloatArrays(byte[] waveData, ref WavePCMParser.WaveInfo waveInfo)
		{
			float[][] channelsData = new float[waveInfo.channels][];

			for (int i = 0; i < waveInfo.channels; ++i)
			{
				channelsData[i] = new float[waveInfo.samplesCount];
			}

			int index = waveInfo.sectionData.indexDataStart;
			int sampleIndex = 0;

			while (index < waveInfo.sectionData.size)
			{
				foreach(var channelData in channelsData)
				{
					// ��������������� � ��������� �� 1f �� -1f
					channelData[sampleIndex] = BitConverter.ToInt16(waveData, index) / _normalizeCoeff;
					index += waveInfo.bytesPerSample;
				}

				sampleIndex++;
			}

			return channelsData;
		}

		public static void ConvertFloatArraysToWaveData(byte[] waveData, ref WavePCMParser.WaveInfo waveInfo, float[][] channelsData)
		{
			int index = waveInfo.sectionData.indexDataStart;

			for (int i = 0; i < waveInfo.samplesCount; ++i)
			{
				foreach (var channelData in channelsData)
				{
					short s = (short)(channelData[i] * _normalizeCoeff);
					waveData[index++] = (byte)(s & 0xFF);
					waveData[index++] = (byte)(s >> 8);
				}
			}
		}
	}

}
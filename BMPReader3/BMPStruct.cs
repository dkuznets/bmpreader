using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace newBMPLib
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct OLSFileHeader
	{
		public UInt16 Type;//	2
		public UInt32 Size;//	4
		public UInt64 TimeStamp;///Резерв;//	8
		public UInt32 TimeExposure;//	4
		public UInt16 preFormat;//	2
		public UInt32 OffBits;//	4
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct OLSInfoHeader
	{
		public UInt32 Size;//	4
		public UInt32 Width;//	4
		public UInt32 Height;//	4
		public UInt16 Planes;//	2
		public UInt16 BitCount;//	2
		public UInt32 Compression;//	4
		public UInt32 SizeImage;//	4
		public UInt32 XPelsPerMeter;//	4
		public UInt32 YPelsPerMeter;//	4
		public UInt32 ClrUsed;//	4
		public UInt32 ClrImportant;//	4

		public UInt64 N_Time;//	8
		public Single Angle_Y;//	4
		public Single Angle_Z;//	4
		public Single Vel_Y;//	4
		public Single Vel_Z;//	4

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
		public Single[] MGD_Matrix;// *	4

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
		public Single[] LV_Matrix;// ***	4

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public Single[] Vector1;// **	4

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public Single[] Vector2;// **	4

		//модифицированный заголовок Мошниным для ОАР [05/09/2012]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public Single[] Nav1;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public Single[] Nav2;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public Single[] Nav3;

		public UInt32 framesCounter;	//счетчик кадров
		public UInt16 tempSensorID;	//номер температурного датчика
		public UInt16 tempSensorVal;	//данные температурного датчика
		public UInt32 rezerv;// Резерв	4Резерв	4Резерв	4Резерв	36

		// для нового протокола:
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public UInt32[] Distortion;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct partOLSInfoHeader
	{
		public UInt32 Size;//	4
		public UInt32 Width;//	4
		public UInt32 Height;//	4
	}
}

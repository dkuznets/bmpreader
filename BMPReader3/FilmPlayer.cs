using System;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using BMPLib;
using MVVM.Core.Commands;
using System.Collections.Generic;
using CameraControl.Enums;

namespace CameraControl.Cameras
{
    public class FilmPlayer : FakeCamera
    {
        FileStream fs;
        BinaryReader br;

        byte[] fileHeaderBuff;
        GCHandle fileHeaderHandle;
        IntPtr fileHeader0;

        byte[] infoHeaderBuff;
        GCHandle infoHeaderHandle;
        IntPtr infoHeader0;

        BitmapFormats format;        

        int nLoopedFrames;

        private void SimpleHandler(IntPtr pdata, int dataSize, int cx, int cy) { }

        public FilmPlayer(
            string _fileName,
            Camera.FrameHandler _recData = null,
            BitmapFormats _format = BitmapFormats.Default,
            int _nLoopedFrames = 0)
            : base(_recData)
        {
            //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            //if (dlg.ShowDialog() == true)
            //    filePath = dlg.FileName;

            if (!File.Exists(_fileName)) throw new FileNotFoundException("Файла с указанным именем не существует.");

            fs = File.OpenRead(_fileName);
            br = new BinaryReader(fs);

            format = _format;
            nLoopedFrames = _nLoopedFrames;

            Type FileHeaderType;
            Type InfoHeaderType;

            switch (format)
            {
                case BitmapFormats.Default:
                    FileHeaderType = typeof(BitmapFileHeader);
                    InfoHeaderType = typeof(BitmapInfoHeader);
                    break;
                case BitmapFormats.OLS:
                    FileHeaderType = typeof(OLSFileHeader);
                    InfoHeaderType = typeof(OLSInfoHeader);
                    break;
                default:
                    throw new NotImplementedException();
            }

            fileHeaderBuff = new byte[Marshal.SizeOf(FileHeaderType)];
            fileHeaderHandle = GCHandle.Alloc(fileHeaderBuff, GCHandleType.Pinned);
            fileHeader0 = fileHeaderHandle.AddrOfPinnedObject();

            infoHeaderBuff = new byte[Marshal.SizeOf(InfoHeaderType)];
            infoHeaderHandle = GCHandle.Alloc(infoHeaderBuff, GCHandleType.Pinned);
            infoHeader0 = infoHeaderHandle.AddrOfPinnedObject();
        }

        private void ReadOlsBitmap()
        {
            fs.Read(fileHeaderBuff, 0, fileHeaderBuff.Length);

            var fileHeader = (OLSFileHeader)Marshal.PtrToStructure(fileHeader0, typeof(OLSFileHeader));

            if (fileHeader.Type != 0x4D42 /* BM in ASCII */)
                throw new IOException("Прочитан неверный заголовок файла");

            fs.Read(infoHeaderBuff, 0, infoHeaderBuff.Length);

            var infoHeader = (OLSInfoHeader)Marshal.PtrToStructure(infoHeader0, typeof(OLSInfoHeader));

            UInt32 cx = infoHeader.Width;
            UInt32 cy = infoHeader.Height;

            UInt32 nImageBytes = fileHeader.Size - fileHeader.OffBits;
            Int32 dataSize;
            if (nImageBytes <= Int32.MaxValue)
                dataSize = Convert.ToInt32(nImageBytes);
            else throw new NotImplementedException();

            byte[] data = br.ReadBytes(dataSize);
            GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr data0 = dataHandle.AddrOfPinnedObject();
            

            if (cx <= Int32.MaxValue && cy <= Int32.MaxValue)
                this.Receiver(data0, dataSize, Convert.ToInt32(cx), Convert.ToInt32(cy));
            else
                throw new NotImplementedException();

            dataHandle.Free();
        }

        private void ReadDefaultBitmap()
        {
            fs.Read(fileHeaderBuff, 0, fileHeaderBuff.Length);

            var fileHeader = (BitmapFileHeader)Marshal.PtrToStructure(fileHeader0, typeof(BitmapFileHeader));

            if (fileHeader.Type != 0x4D42 /* BM in ASCII */)
                throw new IOException("Прочитан неверный заголовок файла.");

            fs.Read(infoHeaderBuff, 0, infoHeaderBuff.Length);            

            var infoHeader = (BitmapInfoHeader)Marshal.PtrToStructure(infoHeader0, typeof(BitmapInfoHeader));

            int totalHeaderSize = infoHeaderBuff.Length + fileHeaderBuff.Length;
            if (fileHeader.OffBits < totalHeaderSize)
                throw new IOException("Прочитаны противоречивые данные.");

            UInt32 nBytesPass = fileHeader.OffBits - (UInt32)totalHeaderSize;
            fs.Seek(nBytesPass, SeekOrigin.Current);

            Int32 cx = infoHeader.Width;
            Int32 cy = infoHeader.Height;

            UInt32 nImageBytes = fileHeader.Size - fileHeader.OffBits;
            Int32 dataSize;
            if (nImageBytes <= Int32.MaxValue)
                dataSize = Convert.ToInt32(nImageBytes);
            else throw new NotImplementedException();

            byte[] data = br.ReadBytes(dataSize);
            GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr data0 = dataHandle.AddrOfPinnedObject();

            this.Receiver(data0, dataSize, cx, cy);

            dataHandle.Free();
        }

        protected override void Generation()
        {
            Action readFrame = () => { };

            switch (format)
            {
                case BitmapFormats.Default:
                    readFrame = ReadDefaultBitmap;
                    break;
                case BitmapFormats.OLS:
                    readFrame = ReadOlsBitmap;
                    break;
            }

            int kk = 0;

            while (this.isOn && br.BaseStream.Position < br.BaseStream.Length)
            {
                readFrame();
                kk++;

                Thread.Sleep(5);

                bool endOfStream = br.BaseStream.Position == br.BaseStream.Length;
                bool endOfLoop = nLoopedFrames > 0 && kk == nLoopedFrames;

                if (endOfStream || endOfLoop)
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    kk = 0;
                }
            }

            fs.Close();
        }
    }
}

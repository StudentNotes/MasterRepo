using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotesFileTransferManager.Base
{
    public abstract class File
    {
        private const int SEGMENT_SIZE = 1024000;    //  1 MB segment!

        public string Name { get; private set; }
        public double Size { get; private set; }
        public byte[] Content { get; private set; }
        public string Path { get; private set; }

        protected File(string path)
        {
            Path = path;
            Content = new byte[0];
            Size = Content.Length;
            

        }

        protected File(string name, string path, byte[] content)
        {
            Path = path;
            Content = content;
            Size = Content.Length;

            Name = name;
        }

        //public static void DumpData(byte[] sourceBytes, ref byte[] allBytes)
        //{
        //    int destinationArrayLength = sourceBytes.Length + allBytes.Length;
        //    byte[] destinationArray = new byte[destinationArrayLength];

        //    for (int i = 0; i < allBytes.Length; i++)
        //    {
        //        destinationArray[i] = allBytes[i];
        //    }

        //    //  Appending the sourceBytes to the allBytes array!
        //    for (int i = allBytes.Length, j = 0; i < destinationArray.Length; i++, j++)
        //    {
        //        destinationArray[i] = sourceBytes[j];
        //    }

        //    allBytes = destinationArray;
        //}

        public void DumpData(byte[] sourceBytes)
        {
            int bufferArrayLength = sourceBytes.Length + Content.Length;

            byte[] bufferArray = new byte[bufferArrayLength];

            for (int i = 0; i < Content.Length; i++)
            {
                bufferArray[i] = Content[i];
            }

            //  Appending the sourceBytes to the buffer array!
            for (int i = Content.Length, j = 0; i < bufferArray.Length; i++, j++)
            {
                bufferArray[i] = sourceBytes[j];
            }

            Content = bufferArray;
        }

        public void LoadContentFromDrive()
        {
            if (Path.Length <= 0 || !System.IO.File.Exists(Path)) return;

            using (FileStream fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader fileReader = new BinaryReader(fileStream))
                {
                    byte[] downloadedFilePart;

                    downloadedFilePart = fileReader.ReadBytes(SEGMENT_SIZE);

                    while (downloadedFilePart.Length > 0)
                    {
                        if (downloadedFilePart.Length < SEGMENT_SIZE)
                        {
                            DumpData(downloadedFilePart);
                            break;
                        }
                        DumpData(downloadedFilePart);
                        downloadedFilePart = fileReader.ReadBytes(SEGMENT_SIZE);
                    }
                    Size = Content.Length;
                }
            }
        }

        public void SafeContentOnDrive()
        {
            if (Path.Length <= 0) return;

            if (System.IO.File.Exists(Path))
            {
                //  Zmiana ścieżki zapisu (dopisanie '_1' do docelowego miejsca zapisu pliku)
                //Path += '_1'; - uwzględnić rozszerzenie i pozostałe pierdoły
            }

            using (FileStream fileStream = new FileStream(Path, FileMode.CreateNew, FileAccess.Write))
            {
                using (BinaryWriter fileWriter = new BinaryWriter(fileStream))
                {
                    fileWriter.Write(Content);
                }
            }
        }
    }
}

using System.Linq;

namespace StudentNotes.FileManager.Base
{
    public class File
    {
        public string Name { get; private set; }
        public double Size { get; private set; }
        public byte[] Content { get; private set; }

        public File(string name, byte[] content = null)
        {
            Name = name;
            Content = content ?? new byte[] {};
            Size = Content.Length;
        }

        public void AppendBytes(byte[] sourceBytes)
        {
            int bufferArrayLength = sourceBytes.Length + Content.Length;

            byte[] bufferArray = new byte[bufferArrayLength];

            for (int i = 0; i < Content.Length; i++)
            {
                bufferArray[i] = Content[i];
            }

            for (int i = Content.Length, j = 0; i < bufferArray.Length; i++, j++)
            {
                bufferArray[i] = sourceBytes[j];
            }

            Content = bufferArray;
            Size = Content.Length;
        }

        public void GetFileName(string path)
        {
            var directoryTree = path.Split('/').ToList();
            Name = directoryTree.Last();
        }
    }
}

using System.IO;

namespace ImageEdit.Core.Persistence.Mapping
{
    internal class MappingHelpers
    {
        public static Stream MapToStream(byte[] imgBytes)
        {
            var stream = new MemoryStream(imgBytes);
            return stream;
        }

        public static byte[] MapToByteArr(Stream stream)
        {
            using (MemoryStream mem = new MemoryStream(new byte[stream.Length]))
            {
                stream.CopyTo(mem);
                mem.Seek(0, SeekOrigin.Begin);
                var arr = mem.ToArray();
                return arr;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace HoneywellHackathonFileShare.Util
{
    static class CompressionUtil
    {
        public static byte[] CompressFile(byte[] inputFs, string fileName)
        {
            //Create a dir in temp of the user on server
            string temFileDir = Path.Combine(Path.GetTempPath(), fileName);
            string destFilePath = Path.Combine(temFileDir, fileName + ".zip");
            File.WriteAllBytes(temFileDir, inputFs);


            //Create Zip file
            ZipFile.CreateFromDirectory(temFileDir, destFilePath);

            //Read the zip file to byte array and return the byte array
            FileStream stream = File.OpenRead(destFilePath);
            byte[] fileBytes = new byte[stream.Length];

            stream.Read(fileBytes, 0, fileBytes.Length);
            stream.Close();

            return fileBytes;
        }
    }
}

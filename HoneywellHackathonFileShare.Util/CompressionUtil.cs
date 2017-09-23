using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace HoneywellHackathonFileShare.Util
{
    public static class CompressionUtil
    {
        public static byte[] CompressFile(byte[] inputFs, string fileName, string temFileDir, string destFilePath)
        {
            //Create a dir in temp of the user on server
            //string temFileDir = Path.Combine("~/Store/Temp", fileName);
            //string destFilePath = Path.Combine(temFileDir, fileName + ".zip");
            try
            {
                string path = Path.Combine(temFileDir, fileName);
                File.WriteAllBytes(path, inputFs);


                //Create Zip file
                string destinationPath = Path.Combine(destFilePath, fileName);
                ZipFile.CreateFromDirectory(temFileDir, destinationPath);

                //Read the zip file to byte array and return the byte array
                FileStream stream = File.OpenRead(destinationPath);
                byte[] fileBytes = new byte[stream.Length];

                stream.Read(fileBytes, 0, fileBytes.Length);
                stream.Close();

                return fileBytes;
            }
            catch (Exception ex) {
                throw;
            }
        }
    }
}

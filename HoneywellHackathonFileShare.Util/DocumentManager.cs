using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoneywellHackathonFileShare.Util
{
    public class DocumentManager
    {
        public string SaveFile(byte[] fileBytes, string fileName, string targetFolder, string tempFileDir, string destFilePath)
        {
            try
            {
                fileBytes = CompressionUtil.CompressFile(fileBytes, fileName, tempFileDir, destFilePath);
                fileBytes = EncryptionUtil.EncryptFile(fileBytes);

                string targetPath = Path.Combine(targetFolder, fileName);

                File.WriteAllBytes(targetPath, fileBytes);

                return fileName;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

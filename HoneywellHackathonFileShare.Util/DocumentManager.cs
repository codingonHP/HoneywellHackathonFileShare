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
        public void SaveFile(byte[] fileBytes, string fileName, string targetFolder)
        {
            try
            {
                fileBytes = CompressionUtil.CompressFile(fileBytes, fileName);
                fileBytes = EncryptionUtil.EncryptFile(fileBytes);

                string targetPath = Path.Combine(targetFolder, fileName);

                File.WriteAllBytes(targetPath, fileBytes);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

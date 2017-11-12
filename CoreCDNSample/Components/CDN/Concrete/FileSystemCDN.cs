using System.IO;

namespace CoreCDNSample.Components.CDN.Concrete
{
    public class FileSystemCDN : ICDN
    {
        string localRootPath;
        string relativeRootPath;

        public FileSystemCDN(string localRootPath, string relativeRootPath)
        {
            this.localRootPath = localRootPath;
            this.relativeRootPath = relativeRootPath;
        }

        public string GetPath(string relativePath)
        {
            return (relativeRootPath.EndsWith("/") ? relativeRootPath : relativeRootPath + "/") + relativePath;
        }

        public void Save(byte[] file, string name, string folder)
        {
            using (FileStream fs = new FileStream(Path.Combine(localRootPath, folder, name), FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(file, 0, file.Length);
            }
        }
    }
}

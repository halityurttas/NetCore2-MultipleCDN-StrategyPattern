namespace CoreCDNSample.Components.CDN
{
    public interface ICDN
    {
        void Save(byte[] file, string name, string folder);
        string GetPath(string relativePath);
    }
}

namespace CoreCDNSample.Components.CDN
{
    public class CDNContext : ICDN
    {
        ICDN cdn;

        public CDNContext(ICDN cdn)
        {
            this.cdn = cdn;
        }

        public string GetPath(string relativePath)
        {
            return cdn.GetPath(relativePath);
        }

        public void Save(byte[] file, string name, string folder)
        {
            cdn.Save(file, name, folder);
        }
    }
}

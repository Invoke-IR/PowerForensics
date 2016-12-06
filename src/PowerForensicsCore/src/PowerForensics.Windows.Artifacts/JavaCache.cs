using System;
using System.Text;
using PowerForensics.FileSystems.Ntfs;

namespace PowerForensics.Windows.Artifacts
{
    /// <summary>
    /// 
    /// </summary>
    public class JavaCache
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime LastModified;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ExpirationDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ValidationTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool Signed;

        private readonly ushort VersionLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Version;

        private readonly ushort UrlLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Url;

        private readonly ushort NamespaceLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Namespace;

        private readonly ushort CodebaseIpLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly string CodebaseIp;

        /// <summary>
        /// 
        /// </summary>
        public readonly string[] HttpHeaders;

        #endregion Properties

        #region Constructors
        
        private JavaCache(byte[] bytes)
        {
            JavaCacheHeader header = JavaCacheHeader.Get(bytes);

            //LastModified = header.LastModified;
            //ExpirationDate = header.ExpirationDate;
            //ValidationTime = header.ValidationTime;
            Signed = header.Signed;

            int offset = 0;

            switch (header.Version)
            {
                case 0x5A020000:
                    /*VersionLength = ;
                    Version = ;
                    offset += VersionLength;
                    UrlLength = ;
                    Url = ;
                    offset += UrlLength;
                    NamespaceLength = ;
                    Namespace = ;
                    offset += CodebaseIpLength;
                    CodebaseIpLength = ;
                    CodebaseIp = ;
                    offset += CodebaseIpLength;*/
                    break;

                case 0x5D020000:
                    offset = 0x80;
                    VersionLength = BitConverter.ToUInt16(bytes, offset);
                    Version = Encoding.ASCII.GetString(bytes, offset + 0x02, VersionLength);
                    offset += VersionLength + 0x02;
                    UrlLength = BitConverter.ToUInt16(bytes, offset);
                    Url = Encoding.ASCII.GetString(bytes, offset + 0x02, VersionLength);
                    offset += UrlLength + 0x02;
                    NamespaceLength = BitConverter.ToUInt16(bytes, offset);
                    Namespace = Encoding.ASCII.GetString(bytes, offset + 0x02, VersionLength);
                    offset += CodebaseIpLength + 0x02;
                    CodebaseIpLength = BitConverter.ToUInt16(bytes, offset);
                    CodebaseIp = Encoding.ASCII.GetString(bytes, offset + 0x02, VersionLength);
                    offset += CodebaseIpLength + 0x02;
                    break;

                default:
                    //Version = ;
                    //Url = ;
                    //Namespace = ;
                    //CodebaseIp = ;
                    break;
            }

            HttpHeaders = GetHttpHeaders(bytes, offset);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static JavaCache[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static JavaCache Get(string path)
        {
            byte[] bytes = FileRecord.Get(path, true).GetContent();
            return new JavaCache(bytes);
        }

        private static string[] GetHttpHeaders(byte[] bytes, int offset)
        {
            return null;
        }

        #endregion Static Methods
    }

    internal class JavaCacheHeader
    {
        #region Properties

        internal readonly bool IsBusy;
        internal readonly bool Incomplete;
        internal readonly uint Version;
        internal readonly bool IsShortcutImage;
        internal readonly uint ContentLength;
        //internal readonly DateTime LastModified;
        //internal readonly DateTime ExpirationDate;
        //internal readonly DateTime ValidationTime;
        internal readonly bool Signed;
        internal readonly uint Section2Length;

        #endregion Properties

        #region Constructors

        private JavaCacheHeader(byte[] bytes)
        {
            IsBusy = Convert.ToBoolean(bytes[0]);
            Incomplete = Convert.ToBoolean(bytes[1]);
            Version = BitConverter.ToUInt32(bytes, 0x02);
            switch (Version)
            {
                case 0x5A020000:
                    IsShortcutImage = Convert.ToBoolean(bytes[0x08]);
                    ContentLength = BitConverter.ToUInt32(bytes, 0x09);
                    //LastModified = Helper.FromUnixTime(BitConverter.ToUInt64(bytes, 0x0B));
                    //ExpirationDate = Helper.FromUnixTime(BitConverter.ToUInt64(bytes, 0x0B));
                    //ValidationTime = Helper.FromUnixTime(BitConverter.ToUInt64(bytes, 0x1D));
                    break;
                case 0x5D020000:
                    IsShortcutImage = Convert.ToBoolean(bytes[0x06]);
                    ContentLength = BitConverter.ToUInt32(bytes, 0x07);
                    //LastModified = Helper.FromUnixTime(BitConverter.ToUInt64(bytes, 0x0B));
                    //ExpirationDate = Helper.FromUnixTime(BitConverter.ToUInt64(bytes, 0x13));
                    //ValidationTime = Helper.FromUnixTime(BitConverter.ToUInt64(bytes, 0x1B));
                    Signed = Convert.ToBoolean(bytes[0x23]);
                    Section2Length = BitConverter.ToUInt32(bytes, 0x24);
                    break;
                default:
                    IsShortcutImage = Convert.ToBoolean(bytes[0x08]);
                    ContentLength = BitConverter.ToUInt32(bytes, 0x09);
                    //LastModified = Helper.FromUnixTime(BitConverter.ToUInt64(bytes, 0x0D));
                    //ExpirationDate = Helper.FromUnixTime(BitConverter.ToUInt64(bytes, 0x15));
                    //ValidationTime = Helper.FromUnixTime(BitConverter.ToUInt64(bytes, 0x1D));
                    Signed = Convert.ToBoolean(bytes[0x25]);
                    Section2Length = BitConverter.ToUInt32(bytes, 0x26);
                    break;
            }
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        internal static JavaCacheHeader Get(byte[] bytes)
        {
            return new JavaCacheHeader(bytes);
        }

        #endregion Static Methods
    }
}

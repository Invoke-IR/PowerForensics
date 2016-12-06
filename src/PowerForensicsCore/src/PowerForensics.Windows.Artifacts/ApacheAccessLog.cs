using PowerForensics.FileSystems.Ntfs;
using System.Collections.Generic;
using System.Text;

namespace PowerForensics.Windows.Artifacts
{
    /// <summary>
    /// 
    /// </summary>
    public class ApacheAccessLog
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string RemoteHostname;

        /// <summary>
        /// 
        /// </summary>
        public readonly string RemoteLogname;

        /// <summary>
        /// 
        /// </summary>
        public readonly string RemoteUsername;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Timestamp;

        /// <summary>
        /// 
        /// </summary>
        public readonly string HttpMethod;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Request;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Status;

        /// <summary>
        /// 
        /// </summary>
        public readonly string ResponseSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Referer;

        /// <summary>
        /// 
        /// </summary>
        public readonly string UserAgent;

        #endregion Properties

        #region Constructor

        private ApacheAccessLog(string entry)
        {
            string[] spacesplit = entry.Split(' ');
            string[] quotesplit = entry.Split('"');

            RemoteHostname = spacesplit[0];
            try
            {
                RemoteLogname = spacesplit[1];
            }
            catch
            {
                RemoteLogname = null;
            }
            try
            {
                RemoteUsername = spacesplit[2];
            }
            catch
            {
                RemoteUsername = null;
            }
            try
            {
                Timestamp = entry.Split('[')[1].Split(']')[0];
            }
            catch
            {
                Timestamp = null;
            }
            try
            {
                HttpMethod = quotesplit[1].Split(' ')[0];
            }
            catch
            {
                HttpMethod = null;
            }
            try
            {
                Request = quotesplit[1];
            }
            catch
            {
                Request = null;
            }
            try
            {
                Status = quotesplit[2].Split(' ')[1];
            }
            catch
            {
                Status = null;
            }
            try
            {
                ResponseSize = quotesplit[2].Split(' ')[2];
            }
            catch
            {
                ResponseSize = null;
            }
            try
            {
                Referer = quotesplit[3];
            }
            catch
            {
                Referer = null;
            }
            try
            {
                UserAgent = quotesplit[5];
            }
            catch
            {
                UserAgent = null;
            }
        }

        #endregion Constructor

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static ApacheAccessLog Get(string entry)
        {
            return new ApacheAccessLog(entry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static ApacheAccessLog[] GetInstances(string Path)
        {
            List<ApacheAccessLog> logList = new List<ApacheAccessLog>();

            foreach (string e in (Encoding.ASCII.GetString(FileRecord.Get(Path).GetContent()).Split('\n')))
            {
                logList.Add(Get(e));
            }

            return logList.ToArray();
        }

        #endregion Static Methods
    }
}

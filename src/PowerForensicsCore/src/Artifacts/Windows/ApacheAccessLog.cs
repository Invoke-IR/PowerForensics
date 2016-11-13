using PowerForensics.Ntfs;
using System.Collections.Generic;
using System.Text;

namespace PowerForensics.Artifacts
{
    public class ApacheAccessLog
    {
        #region Properties

        public readonly string RemoteHostname;
        public readonly string RemoteLogname;
        public readonly string RemoteUsername;
        public readonly string Timestamp;
        public readonly string HttpMethod;
        public readonly string Request;
        public readonly string Status;
        public readonly string ResponseSize;
        public readonly string Referer;
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

        #region StaticMethods

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

        #endregion StaticMethods
    }
}

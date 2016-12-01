using System;
using System.Collections.Generic;

namespace PowerForensics.Formats
{
    /// <summary>
    /// 
    /// </summary>
    public class Gource
    {
        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Get(ForensicTimeline input)
        {
            string type = null;

            if (input.ActivityType.Contains("B"))
            {
                type = "A";
            }
            else
            {
                type = "M";
            }

            return String.Format("{0}|{1}|{2}|{3}", Helper.ToUnixTime(input.Date), input.Source, type, input.FileName).Replace(@"\", "/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] GetInstances(ForensicTimeline[] input)
        {
            List<string> list = new List<string>();
            foreach (ForensicTimeline o in input)
            {
                list.Add(Get(o));
            }
            return list.ToArray();
        }

        #endregion Static Methods
    }
}

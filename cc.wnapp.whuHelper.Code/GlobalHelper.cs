using System;

namespace cc.wnapp.whuHelper.Code
{
    public class GlobalHelper
    {
        public static DateTime StrToDateTime(string dateTime)
        {
            int year = int.Parse(dateTime.Split('/')[0]);
            int month = int.Parse(dateTime.Split('/')[1]);
            int day = int.Parse(dateTime.Split('/')[2].Split(' ')[0]);
            string theRest = dateTime.Split('/')[2].Split(' ')[1];
            int hour = int.Parse(theRest.Split(':')[0]);
            int minute = int.Parse(theRest.Split(':')[1]);
            int second = 0;
            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}
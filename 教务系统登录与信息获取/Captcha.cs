using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CrOcr
{
    public static class Dc
    {
        
        [DllImport(@"dc.dll", EntryPoint = "GetUserInfo")]
        public static extern IntPtr pGetUserInfo(string username, string password);

        /// <summary>
        /// 查询剩余点数
        /// </summary>
        /// <param name="username">超人云账号</param>
        /// <param name="password">超人云密码</param>
        /// <returns>成功返回剩余点数;失败返回:-1=网络错误;-5=账户密码错误 -3=IP冻结</returns>
        public static string GetUserInfo(string username, string password)
        {
            IntPtr p = pGetUserInfo(username, password);
            return Marshal.PtrToStringAnsi(p);
        }

        [DllImport("dc.dll", EntryPoint = "RecByte_A")]
        public static extern IntPtr pRecByte_A(byte[] img, int len, string username, string password, string strSoftId);

        /// <summary>
        /// 通过上传图片字节数组到服务器进行识别
        /// </summary>
        /// <param name="img">图片字节集</param>
        /// <param name="len">图片字节集长度</param>
        /// <param name="username">超人云账号</param>
        /// <param name="password">超人云密码</param>
        /// <param name="strSoftId">软件ID,决定作者分成和字符最大位数,为空默认为识别最长4位字符</param>
        /// <returns>成功返回->识别结果|!|图片ID;失败返回->点数不够:Error:No Money!;账户密码错误:Error:No Reg!;上传失败，参数错误或者网络错误:Error:Put Fail!;识别超时:Error:TimeOut!;上传无效图片:Error:empty picture!;账户或IP被冻结-:Error:Account or Software Bind!;软件被冻结:Error:Software Frozen!;</returns>

        public static string RecByte_A(byte[] img, int len, string username, string password, string strSoftId)
        {
            IntPtr p = pRecByte_A(img,len, username, password, strSoftId);
            return Marshal.PtrToStringAnsi(p);
        }
        [DllImport("dc.dll", EntryPoint = "RecYZM_A")]
        public static extern IntPtr pRecYZM_A(string strYZMPath, string username, string password, string strSoftId);

        /// <summary>
        /// 通过发送本地图片到服务器识别
        /// </summary>
        /// <param name="strYZMPath">图片路径</param>
        /// <param name="username">超人云账号</param>
        /// <param name="password">超人云密码</param>
        /// <param name="strSoftId">软件ID,决定作者分成和字符最大位数,为空默认为识别最长4位字符</param>
        /// <returns>成功返回->识别结果|!|图片ID;失败返回->点数不够:Error:No Money!;账户密码错误:Error:No Reg!;上传失败，参数错误或者网络错误:Error:Put Fail!;识别超时:Error:TimeOut!;上传无效图片:Error:empty picture!;账户或IP被冻结-:Error:Account or Software Bind!;软件被冻结:Error:Software Frozen!;</returns>

        public static string RecYZM_A(string strYZMPath, string username, string password, string strSoftId)
        {
            IntPtr p = pRecYZM_A(strYZMPath, username, password, strSoftId);
            return Marshal.PtrToStringAnsi(p);
        }
        /// <summary>
        /// 对识别错误的返回值进行报错
        /// </summary>
        /// <param name="username">超人云用户</param>
        /// <param name="strImageId">图片ID</param>
        [DllImport("dc.dll")]
        public static extern void ReportError(string username, string strImageId);         
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
using COSXML.Model.Bucket;
using COSXML.CosException;
using System.IO;
using COSXML.Utils;
using Tools;

namespace CosOperation
{
    public static class CosOp
    {
        /// <summary>
        /// 上传指定文件到对象存储的CoursesExport目录中
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="path">本地文件的绝对路径</param>
        /// <returns>下载地址</returns>
        public static String UploadFile(string fileName, string path)
        {
            CosXmlConfig config = new CosXmlConfig.Builder()
            .SetConnectionTimeoutMs(60000)  //设置连接超时时间，单位毫秒，默认45000ms
            .SetReadWriteTimeoutMs(40000)  //设置读写超时时间，单位毫秒，默认45000ms
            .IsHttps(true)  //设置默认 HTTPS 请求
            .SetAppid("***REMOVED***") //设置腾讯云账户的账户标识 APPID
            .SetRegion("***REMOVED***") //设置一个默认的存储桶地域
            .Build();

            string AppDirectory = CQ.Api.AppDirectory;
            string secretId = ini.Read(AppDirectory + @"\CosSecret.ini", "CosSecret", "SecretId", "");   //云 API 密钥 SecretId
            string secretKey = ini.Read(AppDirectory + @"\CosSecret.ini", "CosSecret", "SecretKey", ""); //云 API 密钥 SecretKey
            long durationSecond = 600;          //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId,
              secretKey, durationSecond);

            CosXml cosXml = new CosXmlServer(config, qCloudCredentialProvider);

            try
            {
                string bucket = "chajian-***REMOVED***"; //存储桶，格式：BucketName-APPID
                string key = "***REMOVED***" + fileName; //对象在存储桶中的位置，即称对象键
                string srcPath = path;//本地文件绝对路径
                if (!File.Exists(srcPath))
                {
                    // 如果不存在目标文件，创建一个临时的测试文件
                    File.WriteAllBytes(srcPath, new byte[1024]);
                }

                PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                PutObjectResult result = cosXml.PutObject(request);
                //对象的 eTag
                string eTag = result.eTag;

                return "***REMOVED***CoursesExport/" + fileName;
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                //请求失败
                CQ.Log.Warning("CosClientException: " , clientEx);
                return "";
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                //请求失败
                CQ.Log.Warning("CosServerException: " , serverEx.GetInfo());
                return "";
            }
        }

    }
}

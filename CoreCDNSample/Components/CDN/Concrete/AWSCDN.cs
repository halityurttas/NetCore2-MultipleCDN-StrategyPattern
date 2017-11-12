using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Linq;

namespace CoreCDNSample.Components.CDN.Concrete
{
    public class AWSCDN : ICDN
    {
        string accessKey;
        string secretKey;
        string bucket;

        public AWSCDN(string accessKey, string secretKey, string bucket)
        {
            this.accessKey = accessKey;
            this.secretKey = secretKey;
            this.bucket = bucket;
        }

        public string GetPath(string relativePath)
        {
            try
            {
                var amazonS3 = new AmazonS3Client(accessKey, secretKey);
                var bucketResponse = amazonS3.ListBucketsAsync().Result;
                var awsBucket = bucketResponse.Buckets.FirstOrDefault(w => w.BucketName == bucket);
                if (awsBucket == null)
                {
                    return "";
                }
                else
                {
                    return amazonS3.GetPreSignedURL(new GetPreSignedUrlRequest
                    {
                        BucketName = bucket,
                        Key = relativePath,
                        Expires = DateTime.MaxValue
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Save(byte[] file, string name, string folder)
        {
            try
            {
                var amazonS3 = new AmazonS3Client(accessKey, secretKey);
                using (var ms = new MemoryStream(file))
                {
                    var transferUtility = new TransferUtility(amazonS3);
                    var transferUtilityRequest = new TransferUtilityUploadRequest()
                    {
                        BucketName = bucket,
                        StorageClass = S3StorageClass.ReducedRedundancy,
                        CannedACL = S3CannedACL.PublicRead,
                        Key = string.IsNullOrEmpty(folder) ? name : folder.EndsWith("/") ? folder + name : folder + "/" + name,
                        InputStream = ms
                    };
                    transferUtility.Upload(transferUtilityRequest);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

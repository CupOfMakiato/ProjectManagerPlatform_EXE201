using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Server.Application.Interfaces;
using Server.Contracts.Abstractions.CloudinaryService;
using Server.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinarySetting _cloudinarySetting;
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySetting> cloudinaryConfig)
        {
            var account = new Account(cloudinaryConfig.Value.CloudName,
                cloudinaryConfig.Value.ApiKey,
                cloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
            _cloudinarySetting = cloudinaryConfig.Value;
        }

        public async Task<DeletionResult> DeleteFileAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deletionParams);
        }

        public async Task<CloudinaryResponse> UploadImage(string fileName, IFormFile fileImage)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileImage.OpenReadStream()),
                Folder = _cloudinarySetting.Folder,
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult?.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            var imageUrl = uploadResult.Uri.AbsoluteUri;
            var imageId = uploadResult.PublicId;
            return new CloudinaryResponse
            {
                ImageUrl = imageUrl,
                PublicImageId = imageId
            };
        }

        public async Task<CloudinaryResponse> UploadVideo(string fileName, IFormFile fileVideo)
        {
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(fileName, fileVideo.OpenReadStream()),
                Folder = _cloudinarySetting.Folder,
            };
            var uploadResult = _cloudinary.Upload(uploadParams);
            if (uploadResult?.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            var videoId = uploadResult.PublicId;
            return new CloudinaryResponse
            {
                PublicVideoId = videoId
            };
        }
    }
}
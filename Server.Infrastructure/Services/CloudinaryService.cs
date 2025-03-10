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
using Server.Domain.Entities;

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

        public async Task<CloudinaryResponse> UploadCardImage(string fileName, IFormFile file, Card card)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, file.OpenReadStream()),
                PublicId = $"/{card.Id}/{Path.GetFileNameWithoutExtension(fileName)}",
                Overwrite = true,
                Folder = "cards"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                return null; // Handle upload failure
            }

            return new CloudinaryResponse
            {
                FileUrl = uploadResult.SecureUrl.ToString(),
                PublicFileId = uploadResult.PublicId
            };
        }

        public async Task<CloudinaryResponse> UploadBoardImage(string fileName, IFormFile file, Board board)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, file.OpenReadStream()),
                PublicId = $"/{board.Id}/{Path.GetFileNameWithoutExtension(fileName)}",
                Overwrite = true,
                Folder = "boards"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                return null; // Handle upload failure
            }

            return new CloudinaryResponse
            {
                
                FileUrl = uploadResult.SecureUrl.ToString(),
                PublicFileId = uploadResult.PublicId
            };
        }

        //public async Task<CloudinaryResponse> UploadVideo(string fileName, IFormFile fileVideo)
        //{
        //    var uploadParams = new VideoUploadParams
        //    {
        //        File = new FileDescription(fileName, fileVideo.OpenReadStream()),
        //        Folder = _cloudinarySetting.Folder,
        //    };
        //    var uploadResult = _cloudinary.Upload(uploadParams);
        //    if (uploadResult?.StatusCode != System.Net.HttpStatusCode.OK)
        //    {
        //        return null;
        //    }
        //    var videoId = uploadResult.PublicId;
        //    return new CloudinaryResponse
        //    {
        //        PublicVideoId = videoId
        //    };
        //}

        public async Task<CloudinaryResponse> UploadCardFile(string fileName, IFormFile file, Card card)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, file.OpenReadStream()),
                PublicId = $"/{card.Id}/{Path.GetFileNameWithoutExtension(fileName)}",
                Overwrite = true,
                Folder = "cards"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult?.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            return new CloudinaryResponse
            {
                PublicFileId = uploadResult.PublicId,
                FileUrl = uploadResult.Uri.AbsoluteUri
            };
        }
        // optional...
        public async Task<string> GetFileUrlAsync(string publicId)
        {
            return _cloudinary.Api.UrlImgUp.BuildUrl(publicId);
        }
    }
}
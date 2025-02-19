using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Server.Contracts.Abstractions.CloudinaryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface ICloudinaryService
    {
        Task<CloudinaryResponse> UploadVideo(string fileName, IFormFile fileVideo);
        Task<CloudinaryResponse> UploadImage(string fileName, IFormFile fileImage);
        Task<DeletionResult> DeleteFileAsync(string publicId);
    }
}

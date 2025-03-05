using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Server.Contracts.Abstractions.CloudinaryService;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Interfaces
{
    public interface ICloudinaryService
    {
        //Task<CloudinaryResponse> UploadVideo(string fileName, IFormFile fileVideo);
        Task<CloudinaryResponse> UploadCardImage(string fileName, IFormFile file, Card card);
        Task<CloudinaryResponse> UploadCardFile(string fileName, IFormFile file, Card card);
        Task<CloudinaryResponse> UploadBoardImage(string fileName, IFormFile file, Board board);
        Task<DeletionResult> DeleteFileAsync(string publicId);
    }
}

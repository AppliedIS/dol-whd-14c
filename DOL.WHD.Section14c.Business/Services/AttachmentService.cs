﻿using System;
using System.Data;
using System.IO;
using System.Linq;
using DOL.WHD.Section14c.DataAccess;
using DOL.WHD.Section14c.Domain.Models.Submission;
using DOL.WHD.Section14c.Domain.ViewModels;
using System.Collections.Generic;
using DOL.WHD.Section14c.PdfApi.Business;

namespace DOL.WHD.Section14c.Business.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IAttachmentRepository _attachmentRepository;

        public AttachmentService(IFileRepository fileRepository, IAttachmentRepository attachmentRepository)
        {
            _fileRepository = fileRepository;
            _attachmentRepository = attachmentRepository;
        }

        public Attachment UploadAttachment(string EIN, MemoryStream memoryStream, string fileName, string fileType)
        {
            var fileUpload = new Attachment()
            {
                FileSize = memoryStream.Length,
                MimeType = fileType,
                OriginalFileName = fileName,
                Deleted = false,
                EIN = EIN
            };

            fileUpload.RepositoryFilePath = $@"{EIN}\{fileUpload.Id}";

            _fileRepository.Upload(memoryStream, fileUpload.RepositoryFilePath);

            _attachmentRepository.Add(fileUpload);
            _attachmentRepository.SaveChanges();

            return fileUpload;
        }

        public AttachementDownload DownloadAttachment(MemoryStream memoryStream, string EIN, Guid fileId)
        {
            var attachment = _attachmentRepository.Get()
                .Where(x => x.EIN == EIN)
                .SingleOrDefault(x => x.Deleted == false && x.Id == fileId.ToString());

            if (attachment == null)
                throw new ObjectNotFoundException();

            var stream = _fileRepository.Download(memoryStream, attachment.RepositoryFilePath);

            return new AttachementDownload()
            {
                MemoryStream = stream,
                Attachment = attachment
            };
        }

        public List<ApplicationData> DownloadApplicationAttachments(string EIN, string htmlString)
        {
            var attachments = _attachmentRepository.Get()
                .Where(x => x.EIN == EIN && x.Deleted == false);

            if (attachments == null)
                throw new ObjectNotFoundException();

            var applicationData = new List<ApplicationData>();

            // Get application content html
            if (!string.IsNullOrEmpty(htmlString))
            {
                applicationData.Add(new ApplicationData()
                {
                    HtmlString = htmlString
                });
            }
            foreach (var attachment in attachments)
            {
                var memoryStream = new MemoryStream();
                var stream = _fileRepository.Download(memoryStream, attachment.RepositoryFilePath);

                applicationData.Add(new ApplicationData()
                {
                    Buffer = stream.ToArray(),
                    Type = attachment.MimeType
                });
            }
            return applicationData;
        }

        public void DeleteAttachement(string EIN, Guid fileId)
        {
            var attachment = _attachmentRepository.Get()
                .Where(x => x.EIN == EIN)
                .SingleOrDefault(x => x.Deleted == false && x.Id == fileId.ToString());

            if (attachment == null)
                throw new ObjectNotFoundException();

            attachment.Deleted = true;

            _attachmentRepository.SaveChanges();
        }

        public void Dispose()
        {
            _attachmentRepository.Dispose();
        }
    }
}

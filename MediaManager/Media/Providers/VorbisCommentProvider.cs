﻿using System.Threading.Tasks;
using MediaManager.Library;

namespace MediaManager.Media
{
    public class VorbisCommentProvider : IMediaItemMetadataProvider
    {
        public Task<IMediaItem> ProvideMetadata(IMediaItem mediaItem)
        {
            return null;
        }
    }
}

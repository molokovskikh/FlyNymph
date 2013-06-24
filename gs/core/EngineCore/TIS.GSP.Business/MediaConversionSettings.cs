﻿using System;
using System.Threading;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// Contains settings for controlling the conversion of media instances from one format
	/// to another.
	/// </summary>
	public class MediaConversionSettings
	{
		/// <summary>
		/// Gets or sets the full path to the source file. Example: "D:\Media\Vacation\party.avi"
		/// Specify <see cref="String.Empty" /> if not applicable.
		/// </summary>
		/// <value>A string.</value>
		public String FilePathSource { get; set; }

		/// <summary>
		/// Gets or sets the full path to the destination file. This file will be created during
		/// the conversion process. Example: "D:\Media\Vacation\party.avi"
		/// Specify <see cref="String.Empty" /> if not applicable.
		/// </summary>
		/// <value>A string.</value>
		public String FilePathDestination { get; set; }

		/// <summary>
		/// Gets or sets the encoder setting. May be null.
		/// </summary>
		/// <value>An instance of <see cref="IMediaEncoderSettings" />.</value>
		public IMediaEncoderSettings EncoderSetting { get; set; }

		/// <summary>
		/// Gets or sets the gallery ID.
		/// </summary>
		/// <value>An integer.</value>
		public int GalleryId { get; set; }

		/// <summary>
		/// Gets or sets the media queue ID. Specify <see cref="Int32.MinValue" /> if not applicable.
		/// </summary>
		/// <value>An integer.</value>
		public int MediaQueueId { get; set; }

		/// <summary>
		/// Gets or sets the timeout to apply to FFmpeg, in milliseconds.
		/// </summary>
		/// <value>An integer.</value>
		public int TimeoutMs { get; set; }

		/// <summary>
		/// Gets or sets the media object that is being converted.
		/// Specify <see cref="NullObjects.NullGalleryObject" /> if not applicable.
		/// </summary>
		/// <value>An instance of <see cref="IGalleryObject" />.</value>
		public int MediaObjectId { get; set; }

		/// <summary>
		/// Gets or sets the output FFmpeg generates during the conversion.
		/// </summary>
		/// <value>The F fmpeg output.</value>
		public String FFmpegOutput { get; set; }

		/// <summary>
		/// Gets or sets the arguments to provide to FFmpeg. Any replacement tokens 
		/// (e.g. {SourceFilePath}, {DestinationFilePath}, {GalleryResourcesPath}) should be replaced with their 
		/// actual values prior to assigning this property.
		/// /// </summary>
		/// <value>A string.</value>
		public String FFmpegArgs { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the destination file was created.
		/// </summary>
		/// <value><c>true</c> if the file was created; otherwise, <c>false</c>.</value>
		public bool FileCreated { get; set; }

		/// <summary>
		/// Gets or sets the cancellation token. Can be used to cancel the conversion process
		/// when it is running asynchronously.
		/// </summary>
		/// <value>An instance of <see cref="CancellationToken" />.</value>
		public CancellationToken CancellationToken { get; set; }
	}
}

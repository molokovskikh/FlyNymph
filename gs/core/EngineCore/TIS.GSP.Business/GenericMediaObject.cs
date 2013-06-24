using System;
using System.Globalization;
using System.IO;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Business.Metadata;
using GalleryServerPro.Business.Properties;

namespace GalleryServerPro.Business
{
	/// <summary>
	/// The <see cref="GenericMediaObject" /> class represents a media object within Gallery Server Pro that is not one of the other media types
	/// that have specific classes for handling their functionality. That is, any media object that is not an <see cref="Image" />, 
	/// <see cref="Video" />, <see cref="Audio" />, or <see cref="ExternalMediaObject" /> object is a <see cref="GenericMediaObject" />.
	/// </summary>
	public class GenericMediaObject : GalleryObject
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of a <see cref="GenericMediaObject" /> object.
		/// </summary>
		/// <param name="file">A <see cref="FileInfo"/> object containing the original file for this object. This is intended to be 
		/// specified when creating a new media object from a file. Specify null when instantiating an object for an existing database
		/// record.</param>
		/// <param name="parentAlbum">The album that contains this object. This is a required parameter.</param>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectException">Thrown when the  
		/// <paramref name="file"/> refers to a file that is not in the same directory as the parent album's directory.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parentAlbum" /> is null.</exception>
		/// <remarks>This constructor does not verify that <paramref name="file"/> refers to a file type that is enabled in the 
		/// configuration file.</remarks>
		internal GenericMediaObject(FileInfo file, IAlbum parentAlbum)
			: this(int.MinValue, parentAlbum, string.Empty, string.Empty, string.Empty,
						 int.MinValue, int.MinValue, int.MinValue, string.Empty, int.MinValue, int.MinValue, int.MinValue, int.MinValue,
						 String.Empty, DateTime.MinValue, String.Empty, DateTime.MinValue, parentAlbum != null ? parentAlbum.IsPrivate : false, false, file)
		{
		}

		/// <summary>
		/// Initializes a new instance of a <see cref="GenericMediaObject" /> object.
		/// </summary>
		/// <param name="id">The ID that uniquely identifies this object. Specify int.MinValue for a new object.</param>
		/// <param name="parentAlbum">The album that contains this object. This is a required parameter.</param>
		/// <param name="title">The title of this image.</param>
		/// <param name="hashKey">The hash key that uniquely identifies the original file.</param>
		/// <param name="thumbnailFilename">The filename of the thumbnail image.</param>
		/// <param name="thumbnailWidth">The width (px) of the thumbnail image.</param>
		/// <param name="thumbnailHeight">The height (px) of the thumbnail image.</param>
		/// <param name="thumbnailSizeKb">The size (KB) of the thumbnail image.</param>
		/// <param name="originalFilename">The filename of the original image.</param>
		/// <param name="originalWidth">The width (px) of the original image.</param>
		/// <param name="originalHeight">The height (px) of the original image.</param>
		/// <param name="originalSizeKb">The size (KB) of the original image.</param>
		/// <param name="sequence">An integer that represents the order in which this image should appear when displayed.</param>
		/// <param name="createdByUsername">The user name of the account that originally added this object to the data store.</param>
		/// <param name="dateAdded">The date this image was added to the data store.</param>
		/// <param name="lastModifiedByUsername">The user name of the account that last modified this object.</param>
		/// <param name="dateLastModified">The date this object was last modified.</param>
		/// <param name="isPrivate">Indicates whether this object should be hidden from un-authenticated (anonymous) users.</param>
		/// <param name="isInflated">A bool indicating whether this object is fully inflated.</param>
		/// <param name="file">A <see cref="FileInfo"/> object containing the original file for this object. This is intended to be 
		/// specified when creating a new media object from a file. Specify null when instantiating an object for an existing database
		/// record.</param>
		/// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.InvalidMediaObjectException">Thrown when 
		/// the <paramref name="file"/> parameter is specified (not null) and the file it refers to is not in the same directory
		/// as the parent album's directory.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="parentAlbum" /> is null.</exception>
		/// <remarks>This constructor does not verify that <paramref name="file"/> refers to a file type that is enabled in the 
		/// configuration file.</remarks>
		internal GenericMediaObject(int id, IAlbum parentAlbum, string title, string hashKey, string thumbnailFilename,
																int thumbnailWidth, int thumbnailHeight, int thumbnailSizeKb, string originalFilename,
																int originalWidth, int originalHeight, int originalSizeKb, int sequence,
																string createdByUsername, DateTime dateAdded, string lastModifiedByUsername, DateTime dateLastModified,
																bool isPrivate, bool isInflated, System.IO.FileInfo file)
		{
			if (parentAlbum == null)
				throw new ArgumentNullException("parentAlbum");

			this.Id = id;
			this.Parent = parentAlbum;
			this.GalleryId = this.Parent.GalleryId;
			this.Title = title;
			this.Sequence = sequence;
			this.Hashkey = hashKey;
			this.CreatedByUserName = createdByUsername;
			this.DateAdded = dateAdded;
			this.LastModifiedByUserName = lastModifiedByUsername;
			this.DateLastModified = dateLastModified;
			this.IsPrivate = isPrivate;

			string parentPhysicalPath = this.Parent.FullPhysicalPathOnDisk;

			IGallerySettings gallerySetting = Factory.LoadGallerySetting(GalleryId);

			// Thumbnail image
			this.Thumbnail = DisplayObject.CreateInstance(this, thumbnailFilename, thumbnailWidth, thumbnailHeight, DisplayObjectType.Thumbnail, new GenericThumbnailCreator(this));
			this.Thumbnail.FileSizeKB = thumbnailSizeKb;
			if (thumbnailFilename.Length > 0)
			{
				// The thumbnail is stored in either the album's physical path or an alternate location (if thumbnailPath config setting is specified) .
				string thumbnailPath = HelperFunctions.MapAlbumDirectoryStructureToAlternateDirectory(parentPhysicalPath, gallerySetting.FullThumbnailPath, gallerySetting.FullMediaObjectPath);
				this.Thumbnail.FileNamePhysicalPath = System.IO.Path.Combine(thumbnailPath, thumbnailFilename);
			}

			// GenericMediaObject instances do not have an optimized version.
			this.Optimized = new NullObjects.NullDisplayObject();

			// Original file
			this.Original = DisplayObject.CreateInstance(this, originalFilename, originalWidth, originalHeight, DisplayObjectType.Original, new NullObjects.NullDisplayObjectCreator());
			this.Original.ExternalHtmlSource = String.Empty;
			this.Original.ExternalType = MimeTypeCategory.NotSet;

			if (file != null)
			{
				this.Hashkey = HelperFunctions.GetHashKeyUnique(file);
				this.Original.FileInfo = file; // Will throw InvalidMediaObjectException if the file's directory is not the same as the album's directory.

				if (this.MimeType.TypeCategory == MimeTypeCategory.Other)
				{
					// Specify a default width and height for any object other than audio, video, and image. We leave those to their default
					// value of int.MinValue because we do not accurately know their real width and height. For example, a corrupt image file 
					// will be rejected by the Image class (an UnsupportedImageTypeException is thrown) and will be routed to this class instead.
					// In this case, we don't know it's real width and height. Similarly, audio and video files are normally handled by the
					// Audio and Video classes. If one of them ends up here, we need to treat it generically and not assign a width and height.
					this.Original.Width = gallerySetting.DefaultGenericObjectWidth;
					this.Original.Height = gallerySetting.DefaultGenericObjectHeight;
				}

				int fileSize = (int)(file.Length / 1024);
				this.Original.FileSizeKB = (fileSize < 1 ? 1 : fileSize); // Very small files should be 1, not 0.
				
				if (gallerySetting.ExtractMetadata)
				{
					// Get the metadata found in the original file.
					MediaObjectMetadataExtractor metadata = new MediaObjectMetadataExtractor(file.FullName, this.GalleryId);
					MetadataItems.AddRange(metadata.GetGalleryObjectMetadataItemCollection());
				}

				// Assign the title based on the template, resorting to the filename if necessary.
				if (String.IsNullOrEmpty(title))
				{
					SetTitle(gallerySetting.MediaObjectCaptionTemplate);

					if (String.IsNullOrEmpty(this.Title))
					{
						this.Title = file.Name;
					}
				}
			}
			else
			{
				this.Original.FileNamePhysicalPath = System.IO.Path.Combine(parentPhysicalPath, originalFilename);
				this.Original.FileSizeKB = originalSizeKb;
			}

			this.SaveBehavior = Factory.GetMediaObjectSaveBehavior(this);
			this.DeleteBehavior = Factory.GetMediaObjectDeleteBehavior(this);

			this.IsInflated = isInflated;

			// Setting the previous properties has caused HasChanges = true, but we don't want this while
			// we're instantiating a new object. Reset to false.
			this.HasChanges = false;

			// Set up our event handlers.
			//this.Saving += new EventHandler(Image_Saving); // Don't need
			this.Saved += Image_Saved;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Inflate the current object by loading all properties from the data store. If the object is already inflated 
		/// (<see cref="IGalleryObject.IsInflated"/>=true), no action is taken.
		/// </summary>
		public override void Inflate()
		{
			// If this is not a new object, and it has not been inflated
			// from the database, go to the database and retrieve the info for this object.
			if ((!this.IsNew) && (!this.IsInflated))
			{
				Factory.LoadGenericMediaObjectInstance(this);

				if ((!this.IsInflated) || (this.HasChanges))
					throw new System.InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.GenericMediaObject_Inflate_Ex_Msg, this.IsInflated, this.HasChanges));
			}
		}

		#endregion

		#region Event Handlers

		void Image_Saved(object sender, EventArgs e)
		{
			// This event is fired when the Save() method is called, after all data is saved.

			#region Assign DisplayObject.MediaObjectId

			// If the MediaObjectId has not yet been assigned, do so now. This will occur after a media object is first
			// saved, since that is when the ID is generated.
			if (this.Thumbnail.MediaObjectId == int.MinValue)
			{
				this.Thumbnail.MediaObjectId = this.Id;
			}

			if (this.Optimized.MediaObjectId == int.MinValue)
			{
				this.Optimized.MediaObjectId = this.Id;
			}

			if (this.Original.MediaObjectId == int.MinValue)
			{
				this.Original.MediaObjectId = this.Id;
			}

			#endregion
		}

		#endregion
	}
}

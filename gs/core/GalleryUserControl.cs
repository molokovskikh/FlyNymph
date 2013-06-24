﻿namespace GalleryServerPro.Web.Controls
{
	/// <summary>
	/// The base class user control used in Gallery Server Pro to represent control-like functionality. Controls in the 
	/// 'controls' directory inherit from this.
	/// </summary>
	public class GalleryUserControl : System.Web.UI.UserControl
	{
		#region Public Properties

		/// <summary>
		/// Gets the instance of the parent user control that contains this control.
		/// </summary>
		/// <value>The user control that contains this user control.</value>
		public Pages.GalleryPage GalleryPage
		{
			get
			{
				System.Web.UI.Control ctl = Parent;
				System.Web.UI.Control tempControl = this;
				while (ctl.GetType() != typeof(Gallery))
				{
					tempControl = ctl;
					ctl = ctl.Parent;
				}

				return (Pages.GalleryPage)tempControl;
			}
		}

		#endregion

	}
}

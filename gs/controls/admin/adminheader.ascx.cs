﻿using System;
using System.Web.UI.WebControls;

namespace GalleryServerPro.Web.Controls.Admin
{
	/// <summary>
	/// Specifies the header content at the beginning of each admin page.
	/// </summary>
	public partial class adminheader : GalleryUserControl
	{
		#region Public Propeties

		/// <summary>
		/// Gets / sets the page title text (e.g. Site Settings - General).
		/// </summary>
		public string AdminPageTitle
		{
			get
			{
				return lblAdminPageHeader.Text;
			}
			set
			{
				lblAdminPageHeader.Text = value;
			}
		}

		/// <summary>
		/// Gets / sets the text that appears on the top and bottom Ok buttons on the page. This is rendered as the value
		/// attribute of the input HTML tag.
		/// </summary>
		public string OkButtonText
		{
			get
			{
				return btnOkTop.Text;
			}
			set
			{
				// Ensure value is less than 25 characters.
				string btnText = String.IsNullOrEmpty(value) ? String.Empty : value.PadRight(25).Substring(0, 25).Trim();
				btnOkTop.Text = btnText;
			}
		}

		/// <summary>
		/// Gets / sets the ToolTip for the top and bottom Ok buttons on the page. The ToolTip is rendered as 
		/// the title attribute of the input HTML tag.
		/// </summary>
		public string OkButtonToolTip
		{
			get
			{
				return btnOkTop.ToolTip;
			}
			set
			{
				// Ensure value is less than 250 characters.
				string tooltipText = String.IsNullOrEmpty(value) ? String.Empty : value.PadRight(250).Substring(0, 250).Trim();
				btnOkTop.ToolTip = tooltipText;
			}
		}

		/// <summary>
		/// Gets / sets the text that appears on the top and bottom Cancel buttons on the page. This is rendered as the value
		/// attribute of the input HTML tag.
		/// </summary>
		public string CancelButtonText
		{
			// This is the text that appears on the top and bottom Cancel buttons.
			get
			{
				return btnCancelTop.Text;
			}
			set
			{
				// Ensure value is less than 25 characters.
				string btnText = String.IsNullOrEmpty(value) ? String.Empty : value.PadRight(25).Substring(0, 25).Trim();
				btnCancelTop.Text = btnText;
			}
		}

		/// <summary>
		/// Gets / sets the ToolTip for the top and bottom Cancel buttons on the page. The ToolTip is rendered as 
		/// the title attribute of the HTML tag.
		/// </summary>
		public string CancelButtonToolTip
		{
			get
			{
				return btnCancelTop.ToolTip;
			}
			set
			{
				// Ensure value is less than 250 characters.
				string tooltipText = String.IsNullOrEmpty(value) ? String.Empty : value.PadRight(250).Substring(0, 250).Trim();
				btnCancelTop.ToolTip = tooltipText;
			}
		}

		/// <summary>
		/// Gets / sets the visibility of the top and bottom Ok buttons on the page. When true, the buttons
		/// are visible. When false, they are not visible (not rendered in the page output.)
		/// </summary>
		public bool OkButtonIsVisible
		{
			get
			{
				return btnOkTop.Visible;
			}
			set
			{
				btnOkTop.Visible = value;
			}
		}

		/// <summary>
		/// Gets / sets the visibility of the top and bottom Cancel buttons on the page. When true, the buttons
		/// are visible. When false, they are not visible (not rendered in the page output.)
		/// </summary>
		public bool CancelButtonIsVisible
		{
			get
			{
				return btnCancelTop.Visible;
			}
			set
			{
				btnCancelTop.Visible = value;
			}
		}

		/// <summary>
		/// Gets a reference to the top button that initiates the completion of the task.
		/// </summary>
		public Button OkButtonTop
		{
			get
			{
				return btnOkTop;
			}
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Handles the Load event of the Page control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ConfigureControlsFirstTime();
			}

		}

		/// <summary>
		/// Handles the Click event of the btnCancel control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void btnCancel_Click(object sender, EventArgs e)
		{
			this.GalleryPage.RedirectToPreviousPage();
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsFirstTime()
		{
		}

		#endregion
	}
}
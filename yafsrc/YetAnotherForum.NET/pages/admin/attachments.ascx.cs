/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014 Ingo Herbote
 * http://www.yetanotherforum.net/
 * 
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
namespace YAF.Pages.Admin
{
    #region Using

    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI.WebControls;

    using YAF.Classes;
    using YAF.Classes.Data;
    using YAF.Controls;
    using YAF.Core;
    using YAF.Core.Model;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;
    using YAF.Types.Models;
    using YAF.Utils;
    using YAF.Utils.Helpers;

    #endregion

    /// <summary>
    ///     Summary description for attachments.
    /// </summary>
    public partial class attachments : AdminPage
    {
        #region Methods

        /// <summary>
        /// The delete_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The e. 
        /// </param>
        protected void Delete_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            ((ThemeButton)sender).Attributes["onclick"] =
                "return (confirm('{0}')".FormatWith(this.GetText("ADMIN_ATTACHMENTS", "CONFIRM_DELETE"));
        }

        /// <summary>
        /// The on init.
        /// </summary>
        /// <param name="e">
        /// The e. 
        /// </param>
        protected override void OnInit([NotNull] EventArgs e)
        {
            this.List.ItemCommand += this.List_ItemCommand;

            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            this.InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The e. 
        /// </param>
        protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            if (this.IsPostBack)
            {
                return;
            }

            this.PageLinks.AddLink(this.PageContext.BoardSettings.Name, YafBuildLink.GetLink(ForumPages.forum));
            this.PageLinks.AddLink(this.GetText("ADMIN_ADMIN", "Administration"), YafBuildLink.GetLink(ForumPages.admin_admin));
            this.PageLinks.AddLink(this.GetText("ADMIN_ATTACHMENTS", "TITLE"), string.Empty);

            this.Page.Header.Title = "{0} - {1}".FormatWith(this.GetText("ADMIN_ADMIN", "Administration"), this.GetText("ADMIN_ATTACHMENTS", "TITLE"));

            this.BindData();
        }

        /// <summary>
        /// The pager top_ page change.
        /// </summary>
        /// <param name="sender">
        /// The source of the event. 
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data. 
        /// </param>
        protected void PagerTop_PageChange([NotNull] object sender, [NotNull] EventArgs e)
        {
            // rebind
            this.BindData();
        }

        /// <summary>
        ///     The bind data.
        /// </summary>
        private void BindData()
        {
            this.PagerTop.PageSize = this.Get<YafBoardSettings>().MemberListPageSize;
            var dt = this.GetRepository<Attachment>().List(null, null, this.PageContext.PageBoardID, this.PagerTop.CurrentPageIndex, this.PagerTop.PageSize);
            this.List.DataSource = this.GetRepository<Attachment>().List(null, null, this.PageContext.PageBoardID, this.PagerTop.CurrentPageIndex, this.PagerTop.PageSize);
            this.PagerTop.Count = dt != null && dt.Rows.Count > 0 ? dt.AsEnumerable().First().Field<int>("TotalRows") : 0;
            this.DataBind();
        }

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// The list_ item command.
        /// </summary>
        /// <param name="source">
        /// The source. 
        /// </param>
        /// <param name="e">
        /// The e. 
        /// </param>
        private void List_ItemCommand([NotNull] object source, [NotNull] RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "delete":
                    this.GetRepository<Attachment>().Delete(e.CommandArgument.ToType<int>());
                    this.BindData();
                    break;
            }
        }

        #endregion
    }
}
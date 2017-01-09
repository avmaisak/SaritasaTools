﻿// Copyright (c) 2015-2017, Saritasa. All rights reserved.
// Licensed under the BSD license. See LICENSE file in the project root for full license information.

namespace Saritasa.Tools.Common.Pagination
{
    /// <summary>
    /// Pagination metadata class.
    /// </summary>
    public class PagedMetadata
    {
        /// <summary>
        /// Page size. Max number of items on page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Current page.
        /// </summary>
        public int CurrentPage { get; set; }
    }
}

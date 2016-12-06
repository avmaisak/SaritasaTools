﻿using System.Collections.Generic;
using Saritasa.Tools.Messages.Internal.Clauses;
using Saritasa.Tools.Messages.Internal.Enums;

namespace Saritasa.Tools.Messages.Internal
{
    /// <summary>
    /// The SELECT statement builder interface
    /// </summary>
    internal interface ISelectStringBuilder
    {
        /// <summary>
        /// Gets the selected columns.
        /// </summary>
        /// <value>
        /// The selected columns.
        /// </value>
        IList<string> SelectedColumns { get; }
        /// <summary>
        /// Gets a value indicating whether the SELECT statement has DISTINCT clause.
        /// </summary>
        /// <value>
        /// <c>true</c> if the SELECT statement has DISTINCT clause; otherwise, <c>false</c>.
        /// </value>
        bool IsDistinct { get; set; }
        /// <summary>
        /// Gets or sets the selected tables.
        /// </summary>
        /// <value>
        /// The selected tables.
        /// </value>
        IList<string> SelectedTables { get; }
        /// <summary>
        /// Gets the WHERE statement.
        /// </summary>
        /// <value>
        /// The WHERE statement.
        /// </value>
        IList<WhereClause> WhereStatement { get; }
        /// <summary>
        /// Gets the ORDER BY statement.
        /// </summary>
        /// <value>
        /// The ORDER BY statement.
        /// </value>
        IList<OrderByClause> OrderByStatement { get; }
        /// <summary>
        /// Gets or sets the skipped rows count.
        /// </summary>
        /// <value>
        /// The skipped rows count.
        /// </value>
        int? SkipRows { get; set; }
        /// <summary>
        /// Gets or sets the taken rows count.
        /// </summary>
        /// <value>
        /// The taken rows count.
        /// </value>
        int? TakeRows { get; set; }

        /// <summary>
        /// Selects all columns.
        /// </summary>
        /// <returns></returns>
        ISelectStringBuilder SelectAll();
        /// <summary>
        /// Selects the specified columns.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <returns></returns>
        ISelectStringBuilder Select(params string[] columnNames);
        /// <summary>
        /// Adds the specified columns to the SELECT statement.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        /// <returns></returns>
        ISelectStringBuilder AddSelect(params string[] columnNames);
        /// <summary>
        /// Distincts result records.
        /// </summary>
        /// <returns></returns>
        ISelectStringBuilder Distinct();
        /// <summary>
        /// Sets the specified table names.
        /// </summary>
        /// <param name="tableNames">The table names.</param>
        /// <returns></returns>
        ISelectStringBuilder From(params string[] tableNames);
        /// <summary>
        /// Sets WHERE statement for the specified column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        WhereClause Where(string columnName);
        /// <summary>
        /// Sets WHERE statement for the specified column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="operator">The operator.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        ISelectStringBuilder Where(string columnName, ComparisonOperator @operator, object value);
        /// <summary>
        /// Orders result.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        ISelectStringBuilder OrderBy(string columnName);
        /// <summary>
        /// Orders result.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="operator">The operator.</param>
        /// <returns></returns>
        ISelectStringBuilder OrderBy(string columnName, SortingOperator @operator);
        /// <summary>
        /// Skips the specified rows count.
        /// </summary>
        /// <param name="rows">The rows count.</param>
        /// <returns></returns>
        ISelectStringBuilder Skip(int rows);
        /// <summary>
        /// Takes the specified rows count.
        /// </summary>
        /// <param name="rows">The rows count.</param>
        /// <returns></returns>
        ISelectStringBuilder Take(int rows);
        /// <summary>
        /// Builds the SELECT statement.
        /// </summary>
        /// <returns></returns>
        string Build();
    }
}

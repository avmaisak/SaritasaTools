﻿using System;
using System.Linq;
using System.Text;
using Saritasa.Tools.Messages.Internal.Clauses;
using Saritasa.Tools.Messages.Internal.Enums;

namespace Saritasa.Tools.Messages.Internal
{
    /// <summary>
    /// The SELECT statement SQL server builder.
    /// </summary>
    /// <seealso cref="Saritasa.Tools.Messages.Internal.SelectStringBuilder" />
    internal class SqlServerSelectStringBuilder : SelectStringBuilder
    {
        /// <summary>
        /// Gets or sets a value indicating whether the TOP statement is PERCENT.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the TOP statement is PERCENT; otherwise, <c>false</c>.
        /// </value>
        public bool TopIsPercent { get; set; }

        /// <summary>
        /// Takes the specified rows count.
        /// </summary>
        /// <param name="rows">The rows count.</param>
        /// <param name="topIsPercent">if set to <c>true</c> the TOP statement is PERCENT.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">TopIsPercent - topIsPercent</exception>
        public ISelectStringBuilder Take(int rows, bool topIsPercent)
        {
            if (SkipRows.HasValue && topIsPercent)
            {
                throw new ArgumentException($"You mustn't set {nameof(TopIsPercent)} as true while there has been set {nameof(SkipRows)}", nameof(topIsPercent));
            }

            TopIsPercent = true;
            return this;
        }

        /// <inheritdoc />
        public override string Build()
        {
            var sb = new StringBuilder("SELECT ");

            // Output Distinct
            if (IsDistinct)
            {
                sb.Append("DISTINCT ");
            }

            if (!SkipRows.HasValue && TakeRows.HasValue)
            {
                sb.Append($"TOP {TakeRows} ");
                if (TopIsPercent)
                {
                    sb.Append("PERCENT ");
                }
            }

            // Output column names
            sb.Append(SelectedColumns.Any() ? string.Join(", ", SelectedColumns.Select(WrapVariable)) : "*");

            // Output table names
            if (SelectedTables.Any())
            {
                sb.Append($" FROM {string.Join(", ", SelectedTables.Select(WrapVariable))}");
            }

            // TODO Output joins
            //if (joins.Count > 0)
            //{
            //    foreach (var clause in joins)
            //    {
            //        switch (clause.JoinType)
            //        {
            //            case JoinType.InnerJoin:
            //                sb.Append(" INNER JOIN");
            //                break;
            //            case JoinType.OuterJoin:
            //                sb.Append(" OUTER JOIN");
            //                break;
            //            case JoinType.LeftJoin:
            //                sb.Append(" LEFT JOIN");
            //                break;
            //            case JoinType.RightJoin:
            //                sb.Append(" RIGHT JOIN");
            //                break;
            //        }
            //        sb.Append($" {clause.ToTable} ON ");
            //        sb.Append(Clauses.WhereStatement.CreateComparisonClause(
            //            $"{clause.FromTable}.{clause.FromColumn}",
            //            clause.ComparisonOperator,
            //            new SqlLiteral($"{clause.ToTable}.{clause.ToColumn}")));
            //        sb.Append(' ');
            //    }
            //}

            // Output where statement
            if (WhereStatement.Any())
            {
                sb.AppendLine();
                sb.Append($"WHERE {string.Join(" AND ", WhereStatement.Select(BuildWhereClauseString))}");
            }

            // TODO Output GroupBy statement
            //if (groupByColumns.Count > 0)
            //{
            //    sb.Append($" GROUP BY {string.Join(", ", groupByColumns)} ");
            //}

            // TODO Output having statement
            //if (Having.ClauseLevels > 0)
            //{
            //    // Check if a Group By Clause was set
            //    if (groupByColumns.Count == 0)
            //    {
            //        throw new Exception("Having statement was set without Group By");
            //    }
            //    if (buildCommand)
            //        sb.Append(" HAVING " + Having.BuildWhereStatement(() => command));
            //    else
            //        sb.Append(" HAVING " + Having.BuildWhereStatement());
            //}

            // Output OrderBy statement
            if (OrderByStatement.Any())
            {
                sb.AppendLine();
                sb.Append($"ORDER BY {string.Join(", ", OrderByStatement.Select(BuildOrderByClauseString))}");

                if (SkipRows.HasValue)
                {
                    sb.AppendLine();
                    sb.Append($"OFFSET {SkipRows} ROWS");
                    if (TakeRows.HasValue)
                    {
                        sb.AppendLine();
                        sb.Append($"FETCH NEXT {TakeRows} ROWS ONLY");
                    }
                }
            }

            // Return the built query
            return sb.ToString();
        }

        private static string BuildOrderByClauseString(OrderByClause clause)
        {
            switch (clause.SortOrder)
            {
                case SortingOperator.Ascending:
                    return $"[{clause.ColumnName}] ASC";
                case SortingOperator.Descending:
                    return $"[{clause.ColumnName}] DESC";
                default:
                    return $"[{clause.ColumnName}]";
            }
        }

        private static string BuildWhereClauseString(WhereClause clause)
        {
            var sb = new StringBuilder();

            sb.Append(CreateComparisonClause(clause.ColumnName, clause.Operator, clause.Value));

            return $"({sb})";
        }

        private static string CreateComparisonClause(string columnName, ComparisonOperator comparisonOperatorOperator, object value)
        {
            if (value == null || value == DBNull.Value)
            {
                switch (comparisonOperatorOperator)
                {
                    case ComparisonOperator.Equals:
                        return $"[{columnName}] IS NULL";
                    case ComparisonOperator.NotEquals:
                        return $"NOT [{columnName}] IS NULL";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(comparisonOperatorOperator),
                            $"Cannot use comparison operator {comparisonOperatorOperator} for NULL values.");
                }
            }

            switch (comparisonOperatorOperator)
            {
                case ComparisonOperator.Equals:
                    return $"[{columnName}] = {FormatSqlValue(value)}";
                case ComparisonOperator.NotEquals:
                    return $"[{columnName}] <> {FormatSqlValue(value)}";
                case ComparisonOperator.GreaterThan:
                    return $"[{columnName}] > {FormatSqlValue(value)}";
                case ComparisonOperator.GreaterOrEquals:
                    return $"[{columnName}] >= {FormatSqlValue(value)}";
                case ComparisonOperator.LessThan:
                    return $"[{columnName}] < {FormatSqlValue(value)}";
                case ComparisonOperator.LessOrEquals:
                    return $"[{columnName}] <= {FormatSqlValue(value)}";
                case ComparisonOperator.Like:
                    return $"[{columnName}] LIKE {FormatSqlValue(value)}";
                case ComparisonOperator.NotLike:
                    return $"NOT [{columnName}] LIKE {FormatSqlValue(value)}";
                case ComparisonOperator.In:
                    return $"[{columnName}] IN ({FormatSqlValue(value)})";
                default:
                    throw new ArgumentOutOfRangeException(nameof(comparisonOperatorOperator),
                        $"Cannot use comparison operator {comparisonOperatorOperator}.");
            }
        }

        private static string FormatSqlValue(object someValue)
        {
            if (someValue == null || someValue is DBNull)
            {
                return "NULL";
            }
            if (someValue is Guid)
            {
                return $"\'{(Guid)someValue}\'";
            }
            if (someValue is string)
            {
                return $"\'{((string)someValue).Replace("'", "''")}\'";
            }
            if (someValue is DateTime)
            {
                return $"\'{(DateTime)someValue:yyyy/MM/dd hh:mm:ss}\'";
            }
            if (someValue is bool)
            {
                return (bool)someValue ? "1" : "0";
            }
            if (someValue is SqlLiteral)
            {
                return WrapVariable(((SqlLiteral)someValue).Value);
            }

            return someValue.ToString();
        }

        private static string WrapVariable(string arg)
        {
            return $"[{arg}]";
        }
    }
}

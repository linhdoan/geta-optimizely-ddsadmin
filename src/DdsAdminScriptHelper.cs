using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data.Dynamic;

namespace Geta.DdsAdmin
{
    public static class DdsAdminScriptHelper
    {
        public static string GetColumns(List<PropertyMap> columns, int[] hiddenColumns)
        {
            // first column is Guid id so its always read only(e.g. -> null) or not present if hidden
            var result = !hiddenColumns.Contains(0) ? "null" : string.Empty;

            for (var i = 0; i < columns.Count(); i++)
            {
                if (hiddenColumns.Contains(i + 1))
                {
                    // skip invisible column
                    continue;
                }

                if (!(columns[i] is InlinePropertyMap))
                {
                    // setting to read-only
                    result += @", null";
                    continue;
                }

                if (columns[i].PropertyType == typeof(bool))
                {
                    result += @", {type: 'select', onblur: 'submit', data: ""{'True':'True', 'False':'False'}""}";
                    continue;
                }
                if (columns[i].PropertyType == typeof(int))
                {
                    result += @", {cssclass: 'number'}";
                    continue;
                }
                result += @", {}"; // empty definition for to be treated as string
            }

            result = result.TrimStart(',');

            return result;
        }

        public static string GetInvisibleColumns(int[] hiddenColumns)
        {
            var result = string.Empty;
            foreach (var column in hiddenColumns)
            {
                result += string.Format("dataTable.fnSetColumnVis({0}, false );", column);
            }
            return result;
        }
    }
}

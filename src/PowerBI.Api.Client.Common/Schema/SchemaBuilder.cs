using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace PowerBI.Api.Client.Schema
{
	/// <summary>
	/// Schema builder.
	/// </summary>
	public static class SchemaBuilder
	{
		/// <summary>
		/// Gets the dataset schema.
		/// </summary>
		/// <returns>The dataset.</returns>
		/// <param name="datasetName">Dataset name.</param>
		/// <param name="types">Types.</param>
		public static DatasetSchema GetDataset(string datasetName, ref Type[] types)
		{
			var datasetSchema = new DatasetSchema
			{
				Name = datasetName,
				Tables = new List<TableSchema>()
			};

			foreach(var type in types)
				datasetSchema.Tables.Add(GetTable(type));

			return datasetSchema;
		}

		/// <summary>
		/// Gets the table schema.
		/// </summary>
		/// <returns>The table.</returns>
		/// <param name="type">Type.</param>
		public static TableSchema GetTable(Type type)
		{
			var table = new TableSchema()
			{
				Name = type.Name,
				Columns = new List<ColumnSchema>()
			};

			var properties = type.GetProperties().Where(x => x.CanRead && x.CanWrite).ToList();

			foreach (var propertyInfo in properties)
				table.Columns.Add(GetColumn(propertyInfo));

			return table;
		}

		/// <summary>
		/// Gets the column schema.
		/// </summary>
		/// <returns>The column.</returns>
		/// <param name="propertyInfo">Property info.</param>
		static ColumnSchema GetColumn(PropertyInfo propertyInfo)
		{
			var column = new ColumnSchema
			{
				Name = typeof (Nullable<>).IsAssignableFrom(propertyInfo.PropertyType)
					? propertyInfo.PropertyType.GenericTypeArguments[0].Name
					: propertyInfo.Name,
				DataType = GetDataType(propertyInfo.PropertyType)
			};

			return column;
		}

		/// <summary>
		/// Gets the type of the data.
		/// </summary>
		/// <returns>The data type.</returns>
		/// <param name="type">Type.</param>
		static string GetDataType(Type type)
		{
			var dataType = string.Empty;

			switch (type.Name)
			{
				case "Int32":
				case "Int64":
					dataType = "Int64";
					break;
				case "Double":
					dataType = "Double";
					break;
				case "Boolean":
					dataType = "bool";
					break;
				case "DateTime":
					dataType = "DateTime";
					break;
				case "String":
					dataType = "string";
					break;
				default :
					throw new ArgumentException(string.Format("Invalid argument. Type {0} isn't support by the PowerBI WebApi", type.FullName));
			}

			return dataType;
		}
	}
}


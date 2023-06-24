using System;
using System.Collections.Generic;
using System.Linq;

namespace DiabloDiscordBot.DiscordStuff.DatabaseStuff.Attributes {
	[AttributeUsage(AttributeTargets.Class)]
	internal sealed class TableAttribute : Attribute {
		public string TableName { get; private set; }
		public TableAttribute(string tableName) {
			TableName = tableName;
		}

		public override string ToString() {
			return TableName;
		}

		public static string GetTableName<T>() where T : AbstractRecord {
			return GetTableName(typeof(T));
		}

		public static string GetTableName(AbstractRecord record) {
			return GetTableName(record.GetType());
		}

		public static string GetTableName(Type type) {
			if (type.BaseType != typeof(AbstractRecord))
				throw new Exception($"'{type.Name}' does not inherit from '{typeof(AbstractRecord).Name}'!");
			return ((TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true).First()).TableName;
		}

		public static IEnumerable<ColumnAttribute> GetColumns<T>() where T : AbstractRecord {
			return GetColumns(typeof(T));
		}

		public static IEnumerable<ColumnAttribute> GetColumns(AbstractRecord record) {
			return GetColumns(record.GetType());
		}

		public static IEnumerable<ColumnAttribute> GetColumns(Type type) {
			if (type.BaseType != typeof(AbstractRecord))
				throw new Exception($"'{type.Name}' does not inherit from '{typeof(AbstractRecord).Name}'!");
			return type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(ColumnAttribute))).Select(prop => prop.GetCustomAttributes(typeof(ColumnAttribute), true).First() as ColumnAttribute);
		}
	}
}

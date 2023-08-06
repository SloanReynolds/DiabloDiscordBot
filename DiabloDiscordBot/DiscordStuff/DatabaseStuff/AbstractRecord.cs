using System;
using System.Collections.Generic;
using System.Linq;
using DiabloBotShared;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff.Attributes;

namespace DiabloDiscordBot.DiscordStuff.DatabaseStuff {
	internal abstract record AbstractRecord {
		public abstract ulong PrimaryKey { get; }

		protected static Database _database => SingletonContainer.I.GetService<Database>();

		internal string GetValuesForInsert(IEnumerable<ColumnAttribute> columnAttributes = null) {
			if (columnAttributes == null) {
				columnAttributes = TableAttribute.GetColumns(this);
			}
			List<string> columns = new();
			List<string> values = new();

			foreach (var col in columnAttributes) {
				columns.Add(col.Name);
				var valueObj = this.GetType().GetProperty(col.Name).GetValue(this);
				switch (col.Type) {
					case ColumnType.Integer:
						if (valueObj.GetType() == typeof(ulong))
							values.Add(((ulong)valueObj).ToString());
						else
							values.Add((valueObj).ToString());
						break;
					case ColumnType.Real:
						values.Add(valueObj.ToString());
						break;
					case ColumnType.Text:
						values.Add("'" + valueObj.ToString().Replace("'","") + "'");
						break;
					case ColumnType.Blob:
						throw new Exception("HUH? Blob?");
				}
			}

			return $"({string.Join(',', columns)}) VALUES ({string.Join(',', values)})";
		}

		internal void SaveNew() {
			_database.Open();
			_database.SaveNew(this);
			_database.Close();
			ILogger.Singleton.WriteLine($"'{TableAttribute.GetTableName(this)}' Record Saved");
		}
	}
}

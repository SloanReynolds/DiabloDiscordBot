using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiabloDiscordBot.DiscordStuff.DatabaseStuff.Attributes {
	[AttributeUsage(AttributeTargets.Property)]
	internal class ColumnAttribute : Attribute {
		public string Name { get; }
		public ColumnType Type { get; }
		public bool IsUnique { get; }
		public bool IsNullable { get; }
		public bool IsPrimaryKey { get; }

		public ColumnAttribute(ColumnType type, bool isUnique = false, bool isNullable = true, bool isPrimaryKey = false, [CallerMemberName] string name = "") {
			Name = name;
			Type = type;
			IsUnique = isUnique;
			IsNullable = isNullable;
			IsPrimaryKey = isPrimaryKey;
		}

		public override string ToString() {
			List<string> ret = new() {
				Name
			};
			switch (Type) {
				case ColumnType.Integer:
					ret.Add("INTEGER");
					break;
				case ColumnType.Real:
					ret.Add("REAL");
					break;
				case ColumnType.Text:
					ret.Add("TEXT");
					break;
				case ColumnType.Blob:
					ret.Add("BLOB");
					break;
			}
			if (IsUnique)
				ret.Add("UNIQUE");
			if (!IsNullable)
				ret.Add("NOT NULL");
			if (IsPrimaryKey)
				ret.Add("PRIMARY KEY");

			return string.Join(' ', ret);
		}
	}

	public enum ColumnType {
		Integer,
		Real,
		Text,
		Blob
	}
}

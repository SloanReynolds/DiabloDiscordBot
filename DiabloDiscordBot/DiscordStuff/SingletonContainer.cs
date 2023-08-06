using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabloDiscordBot.DiscordStuff {
	public class SingletonContainer {
		private static SingletonContainer _instance = new SingletonContainer();
		private Dictionary<Type, object> _services = new();

		private static Dictionary<Type, object> _s => _instance._services;
		public static SingletonContainer I => _instance;

		public SingletonContainer RegisterService<T>(object service) {
			Type type = typeof(T);
			if (_s.ContainsKey(type))
				throw new Exception($"Singleton '{type.Name}' is already registered!");

			_s.Add(type, service);

			return _instance;
		}

		public T GetService<T>() {
			Type type = typeof(T);
			if (!_s.ContainsKey(type))
				throw new Exception($"Singleton '{type.Name}' not found in registered services!");

			return (T)_s[type];
		}
	}
}

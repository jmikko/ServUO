#region References
using System;

using Server.Mobiles;
#endregion

namespace Server
{
	public class OppositionGroup
	{
		// Faction opposition tables removed along with the legacy monster roster (no D&D equivalent) - always inert.
		private static readonly OppositionGroup m_TerathansAndOphidians = new OppositionGroup(new Type[0][]);
		private static readonly OppositionGroup m_SavagesAndOrcs = new OppositionGroup(new Type[0][]);
		private static readonly OppositionGroup m_FeyAndUndead = new OppositionGroup(new Type[0][]);

		private readonly Type[][] m_Types;

		public OppositionGroup(Type[][] types)
		{
			m_Types = types;
		}

		public static OppositionGroup TerathansAndOphidians { get { return m_TerathansAndOphidians; } }
		public static OppositionGroup SavagesAndOrcs { get { return m_SavagesAndOrcs; } }
		public static OppositionGroup FeyAndUndead { get { return m_FeyAndUndead; } }

		public bool IsEnemy(object from, object target)
		{
			var fromGroup = IndexOf(from);
			var targGroup = IndexOf(target);

			return (fromGroup != -1 && targGroup != -1 && fromGroup != targGroup);
		}

		public int IndexOf(object obj)
		{
			if (obj == null)
				return -1;

			var type = obj.GetType();

			for (var i = 0; i < m_Types.Length; ++i)
			{
				var group = m_Types[i];

				var contains = false;

				for (var j = 0; !contains && j < group.Length; ++j)
					contains = group[j].IsAssignableFrom(type);

				if (contains)
					return i;
			}

			return -1;
		}
	}
}
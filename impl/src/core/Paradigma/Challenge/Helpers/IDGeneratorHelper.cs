using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Works.Paradigma.Challenge.Helpers
{
    
	public sealed class IDGeneratorHelper
	{

		private static readonly Lazy<IDGeneratorHelper> lazy = new Lazy<IDGeneratorHelper>(() => new IDGeneratorHelper());

		public static IDGeneratorHelper Instance { get { return lazy.Value; } }

		private IDGeneratorHelper()
		{
			_id = 0;
		}

		private static long _id = 0;


		/// <summary>
		/// Returns and ID. e.g: 1
		/// </summary>
		public long Next() => Interlocked.Increment(ref _id);
		public void Reset() => _id = 0;


	}
}

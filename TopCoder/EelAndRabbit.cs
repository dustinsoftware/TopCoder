using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace TopCoder
{
	[TestFixture]
	public static class EelAndRabbit
	{
		/*
		 * Rabbit went to a river to catch eels. All eels are currently swimming down the stream at the same speed. Rabbit is standing by the river, downstream from all the eels. 

Each point on the river has a coordinate. The coordinates increase as we go down the stream. Initially, Rabbit is standing at the origin, and all eels have non-positive coordinates. 

You are given two int[]s: l and t. These describe the current configuration of eels. The speed of each eel is 1 (one). For each i, the length of eel number i is l[i]. 
		 * The head of eel number i will arrive at the coordinate 0 precisely at the time t[i]. Therefore, at any time T the eel number i has its head at the coordinate T-t[i], 
		 * and its tail at the coordinate T-t[i]-l[i]. 

Rabbit may only catch an eel when some part of the eel (between head and tail, inclusive) is at the same coordinate as the rabbit. 
		 * Rabbit can catch eels at most twice. Each time he decides to catch eels, he may catch as many of the currently available eels as he wants. 
		 * (That is, he can only catch eels that are in front of him at that instant, and he is allowed and able to catch multiple eels at once.) 

Return the maximal total number of eels Rabbit can catch.
		 */
		[Test]
		public static void Test()
		{
			Assert.AreEqual(6, GetMax(new[] { 2, 4, 3, 2, 2, 1, 10 }, new[] { 2, 6, 3, 7, 0, 2, 0 }));
			Assert.AreEqual(4, GetMax(new[] { 0, 0, 0, 0 }, new[] { 0, 0, 0, 0 }));
			Assert.AreEqual(7, GetMax(new[] { 8, 2, 1, 10, 8, 6, 3, 1, 2, 5 }, new[] { 17, 27, 26, 11, 1, 27, 23, 12, 11, 13 }));
		}

		public static int GetMax(int[] length, int[] time)
		{
			Assert.AreEqual(length.Length, time.Length);
			ReadOnlyCollection<Eel> eels = length.Select((t, i) => new Eel { Length = t, Time = time[i] }).ToReadOnlyCollection();

			int maxCatch = 0;
			foreach (var eelCatch in GetCatches(time.Max()))
			{
				int eelsCaught = 0;
				List<Eel> availableEels = eels.Select(x => new Eel { Length = x.Length, Time = x.Time }).ToList();
				eelsCaught += availableEels.RemoveAll(x => x.IsReachable(eelCatch.FirstTry));
				eelsCaught += availableEels.RemoveAll(x => x.IsReachable(eelCatch.SecondTry));
				maxCatch = Math.Max(maxCatch, eelsCaught);
			}

			return maxCatch;
		}

		private static IEnumerable<Catch> GetCatches(int max)
		{
			for (int i = 0; i <= max; i++)
			{
				for (int j = 0; j <= max; j++)
				{
					yield return new Catch { FirstTry = i, SecondTry = j };
				}
			}
		}

		private sealed class Catch
		{
			public int FirstTry { get; set; }
			public int SecondTry { get; set; }
		}

		private sealed class Eel
		{
			public bool IsReachable(int time)
			{
				return time >= Time && time <= Time + Length;
			}

			public int Length { get; set; }
			public int Time { get; set; }
		}
	}
}

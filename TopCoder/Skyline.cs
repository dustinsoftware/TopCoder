using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace TopCoder
{
	/*
	 * 
 The Skyline Problem 

Background

With the advent of high speed graphics workstations, CAD (computer-aided design) and other areas (CAM, VLSI design) have made increasingly effective use of computers. One of the problems with drawing images is the elimination of hidden lines -- lines obscured by other parts of a drawing.

The Problem

You are to design a program to assist an architect in drawing the skyline of a city given the locations of the buildings in the city. To make the problem tractable, all buildings are rectangular in shape and they share a common bottom (the city they are built in is very flat). The city is also viewed as two-dimensional. A building is specified by an ordered triple   where   and   are left and right coordinates, respectively, of building i and   is the height of the building. In the diagram below buildings are shown on the left with triples (1,11,5), (2,6,7), (3,13,9), (12,7,16), (14,3,25), (19,18,22), (23,13,29), (24,4,28)

the skyline, shown on the right, is represented by the sequence: (1, 11, 3, 13, 9, 0, 12, 7, 16, 3, 19, 18, 22, 3, 23, 13, 29, 0)



The Input

The input is a sequence of building triples. All coordinates of buildings are positive integers less than 10,000 and there will be at least one and at most 5,000 buildings in the input file. Each building triple is on a line by itself in the input file. All integers in a triple are separated by one or more spaces. The triples will be sorted by   , the left x-coordinate of the building, so the building with the smallest left x-coordinate is first in the input file.

The Output

The output should consist of the vector that describes the skyline as shown in the example above. In the skyline vector  , the   such that i is an even number represent a horizontal line (height). The   such that i is an odd number represent a vertical line (x-coordinate). The skyline vector should represent the ``path'' taken, for example, by a bug starting at the minimum x-coordinate and traveling horizontally and vertically over all the lines that define the skyline. Thus the last entry in the skyline vector will be a 0. The coordinates must be separated by a blank space.

Sample Input

1 11 5
2 6 7
3 13 9
12 7 16
14 3 25
19 18 22
23 13 29
24 4 28
Sample Output

1 11 3 13 9 0 12 7 16 3 19 18 22 3 23 13 29 0
	 * */
	[TestFixture]
	public static class Skyline
	{
		[Test]
		public static void Test()
		{
			Assert.AreEqual("1 11 3 13 9 0 12 7 16 3 19 18 22 3 23 13 29 0".Split(' ').Select(int.Parse), SweepLine(
				@"1 11 5
2 6 7
3 13 9
12 7 16
14 3 25
19 18 22
23 13 29
24 4 28
".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.Split(' '))
				.Select(x => x.Select(int.Parse).ToArray())
				.Select(x => new Building(x[0], x[1], x[2]))));

			const int max = 20000;
			const int buildingCount = 1000;
			Random random = new Random();
			ReadOnlyCollection<Building> buildings = Enumerable.Range(0, buildingCount).Select(x =>
			{
				int left = random.Next(max);
				return new Building(left, random.Next(max), left + random.Next(max));
			}).ToReadOnlyCollection();

			var sweepLineResults = SweepLine(buildings);
			var bruteforceResults = BruteForce(buildings);

			Assert.AreEqual(sweepLineResults, bruteforceResults);
		}

		public static IEnumerable<int> SweepLine(IEnumerable<Building> buildings)
		{
			ReadOnlyCollection<Building> buildingsList = buildings.OrderBy(x => x.Left).ToReadOnlyCollection();
			Dictionary<int, List<Event>> events = new Dictionary<int, List<Event>>();

			foreach (var building in buildingsList)
			{
				events.GetOrAddValue(building.Left).Add(new Event { Building = building, Mode = EventMode.Add });
				events.GetOrAddValue(building.Right).Add(new Event { Building = building, Mode = EventMode.Remove });
			}

			HashSet<Building> currentHeights = new HashSet<Building>();
			int lastHeight = 0;
			foreach (var position in events.OrderBy(x => x.Key))
			{
				foreach (var buildingEvent in position.Value)
				{
					if (buildingEvent.Mode == EventMode.Add)
						Assert.IsTrue(currentHeights.Add(buildingEvent.Building));
					if (buildingEvent.Mode == EventMode.Remove)
						Assert.IsTrue(currentHeights.Remove(buildingEvent.Building));
				}

				int max = currentHeights.Any() ? currentHeights.Max(x => x.Height) : 0;
				if (lastHeight == max) continue;

				yield return position.Key;
				yield return max;
				lastHeight = max;
			}
		}

		public static IEnumerable<int> BruteForce(IEnumerable<Building> buildings)
		{
			int[] heights = new int[buildings.Max(x => x.Right) + 1];
			foreach (var building in buildings)
			{
				for (int i = building.Left; i < building.Right; i++)
				{
					if (heights[i] < building.Height)
						heights[i] = building.Height;
				}
			}

			int lastHeight = 0;
			for (int i = 0; i < heights.Length; i++)
			{
				if (heights[i] == lastHeight) continue;
				yield return i;
				yield return heights[i];
				lastHeight = heights[i];
			}
		}

		public sealed class Building
		{
			public Building(int left, int height, int right)
			{
				Left = left;
				Height = height;
				Right = right;
			}

			public int Left { get; set; }
			public int Height { get; set; }
			public int Right { get; set; }
		}

		private sealed class Event
		{
			public EventMode Mode { get; set; }
			public Building Building { get; set; }
		}

		private enum EventMode
		{
			Add,
			Remove,
		}
	}
}

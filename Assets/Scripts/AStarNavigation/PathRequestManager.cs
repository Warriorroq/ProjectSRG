using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
namespace ProjectSRG.AStarNavigation
{
	public class PathRequestManager : Singleton<PathRequestManager>
	{

		private Queue<PathResult> _results = new Queue<PathResult>();

		private void Update()
		{
			if (_results.Count > 0)
			{
				int itemsInQueue = _results.Count;
				lock (_results)
				{
					for (int i = 0; i < itemsInQueue; i++)
					{
						PathResult result = _results.Dequeue();
						result.callback(result.path, result.success);
					}
				}
			}
		}

		public static void RequestPath(PathRequest request)
		{
			ThreadStart threadStart = delegate
			{
				Pathfinding.Instance.FindPath(request, Instance.FinishedProcessingPath);
			};
			threadStart.Invoke();
		}

		public void FinishedProcessingPath(PathResult result)
		{
			lock (_results)
			{
				_results.Enqueue(result);
			}
		}



	}

	public struct PathResult
	{
		public Node[] path;
		public bool success;
		public Action<Node[], bool> callback;

		public PathResult(Node[] path, bool success, Action<Node[], bool> callback)
		{
			this.path = path;
			this.success = success;
			this.callback = callback;
		}

	}

	public struct PathRequest
	{
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Node[], bool> callback;

		public PathRequest(Vector3 _start, Vector3 _end, Action<Node[], bool> _callback)
		{
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}

	}
}
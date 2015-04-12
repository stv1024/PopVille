using System.Collections.Generic;
using System.Linq;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI.PushLevel
{
    /// <summary>
    /// 岛上的路线，逻辑层。子物体应该全是路径点
    /// </summary>
    public class Route : MonoBehaviour
    {
        public readonly List<Transform> _points = new List<Transform>();
        public readonly List<Vector2> _segments = new List<Vector2>();

        void Awake()
        {
            FindAllPoints();
        }
        /// <summary>
        /// 将子物体列入_points并且在内存里排序。仅Editor用
        /// </summary>
        public void FindAllPoints()
        {
            _points.Clear();
            foreach (Transform point in transform)
            {
                _points.Add(point);
            }
            _points.Sort((x, y) =>
            {
                var ix = 0;
                int.TryParse(x.name, out ix);
                var iy = 0;
                int.TryParse(y.name, out iy);
                return ix - iy;
            });
            _segments.Clear();
            for (int i = 0; i < _points.Count - 1; i++)
            {
                _segments.Add(_points[i + 1].localPosition - _points[i].localPosition);
            }
        }

        /// <summary>
        /// 会自行FindAllPoints()的
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Vector2> GetPointsUniformly(int count)
        {
            FindAllPoints();
            return GetPointsOnRouteUniformly(_points, count);
        }


        public static List<Vector2> GetPointsOnRouteUniformly(List<Transform> points, int count)
        {
            var outputPoints = new List<Vector2>();
            if (points.Count <= 0)
            {
                for (int i = 0; i < count; i++)
                {
                    outputPoints.Add(Vector2.zero);
                }
                return outputPoints;
            }

            if (points.Count <= 1)
            {
                for (int i = 0; i < count; i++)
                {
                    outputPoints.Add(points[0].localPosition);
                }
                return outputPoints;
            }

            var pathLenghs = new List<float> {0};
            for (int i = 1; i < points.Count; i++)
            {
                var segment = points[i].localPosition - points[i-1].localPosition;
                pathLenghs.Add(pathLenghs[i - 1] + segment.ToVector2().magnitude);
            }
            var pointIndex = 1;
            for (int i = 0; i < count; i++)
            {
                var pathPos = pathLenghs[pathLenghs.Count - 1]*i/(count - 1);
                while (pointIndex < pathLenghs.Count - 1 && pathPos > pathLenghs[pointIndex])
                {
                    pointIndex++;
                }
                var lerp = Mathf.InverseLerp(pathLenghs[pointIndex - 1], pathLenghs[pointIndex], pathPos);
                outputPoints.Add(Vector2.Lerp(points[pointIndex - 1].localPosition, points[pointIndex].localPosition, lerp));
            }
            return outputPoints;
        }
    }
}
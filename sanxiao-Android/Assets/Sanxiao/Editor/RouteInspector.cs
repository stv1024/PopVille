using System.Linq;
using Assets.Sanxiao.UI.PushLevel;
using Fairwood.Math;
using UnityEditor;
using UnityEngine;

namespace Assets.Sanxiao.Editor
{
    [CustomEditor(typeof(Route))]
    public class RouteInspector : UnityEditor.Editor
    {
        private static bool _previewState;

        private static int _insertIndex;

        private static int _previewLevelCount;

        public override void OnInspectorGUI()
        {
            var route = target as Route;
            if (!route) return;

            DrawDefaultInspector();

            EditorGUILayout.LabelField("子物体都是路径点，名称必须\n为可解析成实数的字符串，\n如3,27,6.25", GUILayout.Height(40));
            if (GUILayout.Button("在末尾添加一个点"))
            {
                var locPos = new Vector3(10, 10);
                if (route._points.Count >= 2)
                {
                    locPos = route._points[route._points.Count - 1].localPosition*2 -
                             route._points[route._points.Count - 2].localPosition;
                }
                var go = new GameObject(route.transform.childCount.ToString());
                go.transform.ResetTransform(route.transform);
                go.transform.localPosition = locPos;
            }

            _insertIndex = EditorGUILayout.IntSlider("插入位置", _insertIndex, 0, route.transform.childCount - 1);
            if (GUILayout.Button("插入路径点"))
            {
                route.FindAllPoints();
                if (0 <= _insertIndex && _insertIndex < route._points.Count)
                {
                    var locPos = route._points[_insertIndex].localPosition + new Vector3(10, 10);
                    var go = new GameObject(string.Format("{0}.5", _insertIndex));
                    go.transform.ResetTransform(route.transform);
                    go.transform.localPosition = locPos;
                }
                TidyPointNames(route);
            }

            EditorGUILayout.LabelField("在中途插入点:\n如想在5和6之间插入，在Hierarchy里添加点，\n使名称为5.5，再点击整理路径点，\n即可使名称自动变成依次递增整数",
                                       GUILayout.Height(60));
            if (GUILayout.Button("整理路径点名称"))
            {
                TidyPointNames(route);
            }
            EditorGUILayout.LabelField("删除路径点:\n在Hierarchy里选中，删除。然后整理名称。", GUILayout.Height(30));

            var previewState = EditorGUILayout.Toggle("预览状态", _previewState);
            if (previewState != _previewState)
            {
                _previewState = previewState;
                SceneView.RepaintAll();
            }

            var previewLevelCount = EditorGUILayout.IntSlider("预览关数", _previewLevelCount, 5, 100);
            if (previewLevelCount != _previewLevelCount)
            {
                _previewLevelCount = previewLevelCount;
                SceneView.RepaintAll();
            }
        }

        void TidyPointNames(Route route)
        {
            route.FindAllPoints();
            for (int i = 0; i < route._points.Count; i++)
            {
                route._points[i].name = i.ToString();
            }
        }

        private void OnSceneGUI()
        {
            var route = target as Route;
            if (!route) return;

            route.FindAllPoints();

            if (!_previewState)
            {
                foreach (Transform point in route._points)
                {
                    var position = Handles.PositionHandle(point.position, Quaternion.identity);
                    if (position != point.position)
                    {
                        point.localPosition = position.SetV3Z(0);
                        EditorUtility.SetDirty(point);
                    }
                    Handles.Label(point.position + new Vector3(0, -3, 0),
                                  string.Format("{0}", point.name));
                }

                Handles.DrawPolyLine(route._points.Select(x => x.position).ToArray());
            }
            else
            {
                var previewPoss = Route.GetPointsOnRouteUniformly(route._points, _previewLevelCount);
                foreach (var lp in previewPoss)
                {
                    var pos = route.transform.TransformPoint(lp);
                    Handles.CircleCap(0, pos, Quaternion.identity, 5);
                }
            }
        }
    }
}
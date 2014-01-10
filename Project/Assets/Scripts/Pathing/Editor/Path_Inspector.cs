using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Path))]
public class Path_Inspector : Editor {

    Vector2 scrollPos;

	public override void OnInspectorGUI()
    {
        Path path = target as Path;

        scrollPos = GUILayout.BeginScrollView(scrollPos);

        GUILayout.BeginVertical("Box");

        for (int i = 0; i < path.nodes.Count; i++)
        {
            PathNode node = path.nodes[i];

            GUILayout.BeginHorizontal();

            GUILayout.Label(node.name);

            if(GUILayout.Button("+", GUILayout.Width(20)))
            {
                PathNode newNode = new GameObject().AddComponent<PathNode>();
                newNode.transform.parent = path.transform;
                newNode.transform.position = node.transform.position;

                path.nodes.Insert(i++, newNode);

                Selection.activeGameObject = newNode.gameObject;
                break;
            }

            if(GUILayout.Button("-", GUILayout.Width(20)))
            {             
                DestroyImmediate(node.gameObject);
                path.nodes.RemoveAt(i--);
                break;
            }

            if(GUILayout.Button("/\\", GUILayout.Width(20)))
            {                
                path.nodes.RemoveAt(i);
                path.nodes.Insert(i-1, node);
                break;
            }

            if(GUILayout.Button("\\/", GUILayout.Width(20)))
            {         
                path.nodes.RemoveAt(i);
                path.nodes.Insert(i+1, node);
                break;
            }

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Node"))
        {
            PathNode newNode = new GameObject().AddComponent<PathNode>();
            newNode.transform.parent = path.transform;
            if(path.nodes.Count > 0)
                newNode.transform.position = path.nodes[path.nodes.Count-1].transform.position;

            path.nodes.Add(newNode);

            Selection.activeGameObject = newNode.gameObject;
        }

        GUILayout.EndVertical();

        if (GUILayout.Button("Clear All"))
        {
            for(int i = 0; i < path.nodes.Count; i++)
            {
                Destroy(path.nodes[i].gameObject);
            }
            path.nodes.Clear();
        }

        GUILayout.EndScrollView();

        if (GUI.changed)
        {
            for (int i = 0; i < path.nodes.Count; i++)
            {
                path.nodes[i].name = "PathNode" + i;
                EditorUtility.SetDirty(path.nodes[i]);
            }
            EditorUtility.SetDirty(target);
        }
    }

    void OnSceneGUI()
    {
        Path path = target as Path;

        Handles.color = Color.white;

        for(int i = 0; i < path.nodes.Count-1; i++)
        {
            PathNode node = path.nodes[i];
            PathNode nextNode = path.nodes[i+1];
            Handles.DrawLine(node.transform.position, nextNode.transform.position);
        }
    }
}

using UnityEngine;
using UnityEditor;

public class TagSearcher : EditorWindow
{
    static TagSearcher  window;
    static string       tagValue    = "";
    static Vector2      scrollValue = Vector2.zero;
    static GameObject[] searchResult;

    [MenuItem("Find/TagSearcher")]
    static void OpenTagSearcher()
    {
        window = (TagSearcher) GetWindow(typeof(TagSearcher));
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Tag:", GUILayout.Width(30), GUILayout.Height(30));
        tagValue = EditorGUILayout.TagField(tagValue);
        if (GUILayout.Button("SEARCH!", GUILayout.Height(15)))
        {
            searchResult      = GameObject.FindGameObjectsWithTag(tagValue);
            Selection.objects = searchResult;
        }
        EditorGUILayout.EndHorizontal();

        scrollValue = EditorGUILayout.BeginScrollView(scrollValue);
        if (searchResult != null)
        {
            foreach (GameObject obj in searchResult)
            {
                if (obj != null)
                {
                    if (GUILayout.Button(obj.name)) //,GUIStyle.none))
                    {
                        Selection.activeObject = obj;
                        EditorGUIUtility.PingObject(obj);
                    }
                }
                else
                {
                    searchResult      = GameObject.FindGameObjectsWithTag(tagValue);
                    Selection.objects = searchResult;
                    break;
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
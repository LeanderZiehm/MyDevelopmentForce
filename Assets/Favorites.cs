using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class Favorites : EditorWindow
{
    private List<GameObject> favorites = new List<GameObject>();
    private ReorderableList favoritesList;

    [MenuItem("[Favorites]/Favorites")]
    public static void ShowWindow()
    {
        GetWindow<Favorites>("Favorites");
    }

    private void OnEnable()
    {
        favoritesList = new ReorderableList(favorites, typeof(GameObject), true, true, false, true);
        favoritesList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, $"Favorites ({favorites.Count}):");
        favoritesList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            GameObject favorite = favorites[index];
            if (favorite != null)
            {
                EditorGUI.LabelField(rect, favorite.name);
                Selection.activeGameObject = favorite;
            }

            else
            {
                favorites.RemoveAt(index);
            }
               
        };
        favoritesList.onReorderCallback = list => Repaint();
        favoritesList.onRemoveCallback = list =>
        {
            favorites.RemoveAt(list.index);
            Repaint();
        };
    }

    private void OnGUI()
    {
        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Link;
            currentEvent.Use();
        }
        else if (currentEvent.type == EventType.DragPerform)
        {
            DragAndDrop.AcceptDrag();

            foreach (Object draggedObject in DragAndDrop.objectReferences)
            {
                if (draggedObject is GameObject gameObject)
                    AddToFavorite(gameObject);
            }

            currentEvent.Use();
        }

        EditorGUILayout.Space();
        
        EditorGUILayout.BeginScrollView(Vector2.zero, GUILayout.ExpandHeight(true));
        favoritesList.DoLayoutList();
        EditorGUILayout.EndScrollView();
    }

    private void AddToFavorite(GameObject selectedGameObject)
    {
        if (!favorites.Contains(selectedGameObject))
        {
            favorites.Add(selectedGameObject);
            Debug.Log($"Added {selectedGameObject.name} to favorites.");
        }
    }
}

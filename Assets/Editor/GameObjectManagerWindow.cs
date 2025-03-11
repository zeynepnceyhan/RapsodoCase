using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class GameObjectManagerWindow : EditorWindow
{
    private List<GameObject> gameObjects = new List<GameObject>();
    private string searchQuery = "";
    private bool showOnlyWithMeshRenderer = false;
    private bool showOnlyWithCollider = false;
    private bool showOnlyWithRigidbody = false;
    private System.Type componentToAdd;
    private System.Type componentToRemove;

    [MenuItem("Tools/GameObject Manager")]
    public static void ShowWindow()
    {
        GetWindow<GameObjectManagerWindow>("GameObject Manager");
    }

    private void OnEnable()
    {
        LoadGameObjects();
        Selection.selectionChanged += OnSelectionChanged;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        LoadGameObjects();
        Repaint();
    }
    private void LoadGameObjects()
    {
        gameObjects = FindObjectsOfType<GameObject>().Where(go =>
            (!showOnlyWithMeshRenderer || go.GetComponent<MeshRenderer>() != null) &&
            (!showOnlyWithCollider || go.GetComponent<Collider>() != null) &&
            (!showOnlyWithRigidbody || go.GetComponent<Rigidbody>() != null) &&
            (string.IsNullOrEmpty(searchQuery) || go.name.ToLower().Contains(searchQuery.ToLower()))
        ).ToList();
        Repaint(); // Listeyi anında güncelle
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("GameObject Manager", EditorStyles.boldLabel);
        searchQuery = EditorGUILayout.TextField("Search:", searchQuery);

        showOnlyWithMeshRenderer = EditorGUILayout.Toggle("Only MeshRenderer", showOnlyWithMeshRenderer);
        showOnlyWithCollider = EditorGUILayout.Toggle("Only Collider", showOnlyWithCollider);
        showOnlyWithRigidbody = EditorGUILayout.Toggle("Only Rigidbody", showOnlyWithRigidbody);

        if (GUILayout.Button("Refresh"))
        {
            LoadGameObjects();
            Repaint();
        }

        foreach (var go in gameObjects)
        {
            if (!string.IsNullOrEmpty(searchQuery) && !go.name.ToLower().Contains(searchQuery.ToLower()))
                continue;
            if (showOnlyWithMeshRenderer && go.GetComponent<MeshRenderer>() == null)
                continue;
            if (showOnlyWithCollider && go.GetComponent<Collider>() == null)
                continue;
            if (showOnlyWithRigidbody && go.GetComponent<Rigidbody>() == null)
                continue;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(go.name, GUILayout.Width(200)))
            {
                Selection.activeGameObject = go;
            }
            bool newActiveState = EditorGUILayout.Toggle(go.activeSelf, GUILayout.Width(50));
            if (newActiveState != go.activeSelf)
            {
                Undo.RecordObject(go, "Toggle Active State");
                go.SetActive(newActiveState);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (Selection.activeGameObject != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Edit Selected GameObject", EditorStyles.boldLabel);
            GameObject selectedGO = Selection.activeGameObject;
            Undo.RecordObject(selectedGO.transform, "Transform Change");
            selectedGO.transform.position = EditorGUILayout.Vector3Field("Position", selectedGO.transform.position);
            selectedGO.transform.rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", selectedGO.transform.rotation.eulerAngles));
            selectedGO.transform.localScale = EditorGUILayout.Vector3Field("Scale", selectedGO.transform.localScale);
        }

        if (Selection.objects.Length > 1)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Batch Edit Selected Objects", EditorStyles.boldLabel);
            Vector3 newPosition = EditorGUILayout.Vector3Field("New Position", Vector3.zero);
            Vector3 newRotation = EditorGUILayout.Vector3Field("New Rotation", Vector3.zero);
            Vector3 newScale = EditorGUILayout.Vector3Field("New Scale", Vector3.one);

            if (GUILayout.Button("Apply to All"))
            {
                foreach (var obj in Selection.objects)
                {
                    if (obj is GameObject go)
                    {
                        Undo.RecordObject(go.transform, "Batch Edit");
                        go.transform.position = newPosition;
                        go.transform.rotation = Quaternion.Euler(newRotation);
                        go.transform.localScale = newScale;
                    }
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Batch Component Management", EditorStyles.boldLabel);

// Bileşen ekleme
        MonoScript addScript = EditorGUILayout.ObjectField("Component to Add", null, typeof(MonoScript), false) as MonoScript;
        if (addScript != null)
        {
            System.Type addType = addScript.GetClass();
            if (addType == null)
            {
                EditorGUILayout.HelpBox("Invalid script selected!", MessageType.Warning);
            }
            else if (GUILayout.Button("Add to Selected"))
            {
                foreach (var obj in Selection.objects)
                {
                    if (obj is GameObject go && !go.GetComponent(addType))
                    {
                        Undo.AddComponent(go, addType);
                    }
                }
            }
        }

// Bileşen kaldırma
        MonoScript removeScript = EditorGUILayout.ObjectField("Component to Remove", null, typeof(MonoScript), false) as MonoScript;
        if (removeScript != null)
        {
            System.Type removeType = removeScript.GetClass();
            if (removeType != null && GUILayout.Button("Remove from Selected"))
            {
                foreach (var obj in Selection.objects)
                {
                    if (obj is GameObject go)
                    {
                        Component comp = go.GetComponent(removeType);
                        if (comp != null)
                        {
                            Undo.DestroyObjectImmediate(comp);
                        }
                    }
                }
            }
        }

    }
}

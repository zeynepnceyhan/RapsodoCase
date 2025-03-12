using UnityEngine;
using UnityEditor;

public class SceneSetup : EditorWindow
{
    [MenuItem("Tools/Setup Scene")]
    public static void SetupScene()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "Cube_Mesh_" + i;
            cube.AddComponent<MeshRenderer>();
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "Cube_Collider_" + i;
            cube.AddComponent<BoxCollider>();
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject obj = new GameObject("RigidbodyObj_" + i);
            obj.AddComponent<Rigidbody>();
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "Sphere_" + i;
            sphere.AddComponent<SphereCollider>();
            sphere.AddComponent<MeshRenderer>();
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject obj = new GameObject("ComplexObj_" + i);
            obj.AddComponent<BoxCollider>();
            obj.AddComponent<MeshRenderer>();
            obj.AddComponent<Rigidbody>();
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject obj = new GameObject("InactiveObj_" + i);
            obj.SetActive(false);
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject obj = new GameObject("TransformOnly_" + i);
        }

        Debug.Log("Scene setup completed.");
    }
}
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    void Start()
    {
        CreateGameObjects();
    }

    void CreateGameObjects()
    {
        // 1. 5 Cube with MeshRenderer
        for (int i = 0; i < 5; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "Cube_MeshRenderer_" + i;
        }

        // 2. 5 Cube with BoxCollider
        for (int i = 0; i < 5; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "Cube_BoxCollider_" + i;
            DestroyImmediate(cube.GetComponent<MeshRenderer>());
        }

        // 3. 5 GameObjects with Rigidbody
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = new GameObject("GameObject_Rigidbody_" + i);
            obj.AddComponent<Rigidbody>();
        }

        // 4. 5 Sphere with MeshRenderer and SphereCollider
        for (int i = 0; i < 5; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "Sphere_MeshRenderer_Collider_" + i;
        }

        // 5. 5 GameObjects with Collider, Rigidbody, and MeshRenderer
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = "GameObject_Collider_Rigidbody_MeshRenderer_" + i;
            obj.AddComponent<Rigidbody>();
        }

        // 6. 5 Inactive GameObjects
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = new GameObject("InactiveGameObject_" + i);
            obj.SetActive(false);
        }

        // 7. 5 GameObjects with only Transform
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = new GameObject("EmptyGameObject_" + i);
        }

        Debug.Log("Scene setup completed!");
    }
}
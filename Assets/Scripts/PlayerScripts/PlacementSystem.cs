using UnityEngine;
using UnityEngine.InputSystem;  // Add this for new Input System

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject hitbox_prefab;
    public GameObject test;
    private static GameObject ghost_object;
    private static GameObject original_prefab;
    private static GameObject hitbox_instance;
    private static Camera main_camera;
    private static bool is_placing = false;

    public static PlacementSystem staticsystem { get; private set; }

    private void Awake()
    {
        staticsystem = this;

        if (main_camera == null)
        {
            main_camera = Camera.main;
        }
    }

    public static void start_placing(GameObject prefab)
    {
        if (staticsystem == null || staticsystem.hitbox_prefab == null)
        {
            Debug.LogError("placement_system is not initialized or hitbox_prefab is missing.");
            return;
        }

        if (ghost_object != null)
        {
            Destroy(ghost_object);
        }

        original_prefab = prefab;
        ghost_object = Instantiate(prefab);
        ghost_object.name = "ghost_" + prefab.name;

        foreach (MonoBehaviour script in ghost_object.GetComponentsInChildren<MonoBehaviour>(true))
        {
            script.enabled = false;
        }

        CapsuleCollider sourcecollider = prefab.GetComponentInChildren<CapsuleCollider>();
        if (sourcecollider != null)
        {
            hitbox_instance = Instantiate(staticsystem.hitbox_prefab);
            hitbox_instance.name = "placement_hitbox";
            hitbox_instance.transform.SetParent(ghost_object.transform);
            hitbox_instance.transform.position = sourcecollider.transform.position;
            hitbox_instance.transform.rotation = sourcecollider.transform.rotation;

            Vector3 scale = new Vector3(
                sourcecollider.radius,
               sourcecollider.radius,
             sourcecollider.radius
            );

            hitbox_instance.transform.localScale = scale;
        }

        is_placing = true;
    }

    private void Update()
    {
        if (is_placing && ghost_object != null)
        {
            update_placement();
        }
    }

    private void update_placement()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        // Define the distance from the camera to the ground plane (Y=0)
        float cameraHeight = main_camera.transform.position.y;

        // Create a Vector3 with mouse position and distance from camera to ground
        Vector3 screenPoint = new Vector3(mousePosition.x, mousePosition.y, cameraHeight);

        // Convert screen point to world point
        Vector3 worldPoint = main_camera.ScreenToWorldPoint(screenPoint);

        // Set Y to zero to clamp on ground plane
        Vector3 NewPosition = new Vector3(worldPoint.x, 0f, worldPoint.z);

        ghost_object.transform.position = NewPosition;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Instantiate(original_prefab, NewPosition, Quaternion.identity);
            Destroy(ghost_object);
            ghost_object = null;
            hitbox_instance = null;
            is_placing = false;
        }
    }

    private void Start()
    {
        start_placing(test);
    }
}

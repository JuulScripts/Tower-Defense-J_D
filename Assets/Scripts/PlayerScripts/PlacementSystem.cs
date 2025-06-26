using System;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject hitbox_prefab;
    private static GameObject ghost_object;
    private static GameObject original_prefab;

    private static Camera main_camera;
    private static bool is_placing = false;
    public static PlacementSystem staticsystem { get; private set; }
    public static Action<GameObject> OnUnitPlaced;

    private void Awake() // Initializes static reference and sets the main camera if not already assigned
    {
        staticsystem = this;

        if (main_camera == null)
        {
            main_camera = Camera.main;
        }
    }

    public static void start_placing(GameObject prefab) // Starts placement mode and creates a disabled "ghost" version of the prefab
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

        foreach (MonoBehaviour script in ghost_object.GetComponentsInChildren<MonoBehaviour>(true))
        {
            script.enabled = false;
        }

        foreach (Collider col in ghost_object.GetComponentsInChildren<Collider>(true))
        {
            col.enabled = false;
        }

        ghost_object.name = "ghost_" + prefab.name;

        CapsuleCollider sourcecollider = prefab.GetComponentInChildren<CapsuleCollider>();
        if (sourcecollider != null)
        {
      

            Vector3 scale = new Vector3(
                sourcecollider.radius,
                sourcecollider.radius,
                sourcecollider.radius
            );

        
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

    private void update_placement() // Handles ghost object positioning and final placement when valid
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = main_camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            ghost_object.transform.position = hit.point;

            if (hit.collider.CompareTag("placable"))
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    GameObject placedUnit = Instantiate(original_prefab, hit.point, Quaternion.identity);
                    OnUnitPlaced?.Invoke(placedUnit);
                    Destroy(ghost_object);
                    ghost_object = null;
                    is_placing = false;
                }
            }
        }
    }

    private void Start()
    {
    }
}
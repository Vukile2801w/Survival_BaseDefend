using UnityEngine;

public class PreviewHandler : MonoBehaviour
{

    [SerializeField] private Material previewMaterialPrefab;
    [SerializeField] private Color validColor;
    [SerializeField] private Color invalidColor;

    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private ObjectDataBase objectDataBase;
    [SerializeField] private PlacedObjectManager placedObjectManager;
    
    private GameObject previewObject;
    private BuildingObject ObjectData;
    private Material[] materials;
    private Material previewMaterial;
    

    private Log log;
    void Start()
    {
        log = new Log(true, "[PreviewHandler] ");

        inputHandler.onBuildStarted += StartPreview;

    }

    // Update is called once per frame
    void Update()
    {
        if (inputHandler.isBuilding)
        {
            PreviewUpdate();
        }

        else if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }

    void StartPreview(int objID)
    {
        previewMaterial = new Material(previewMaterialPrefab);

        ObjectData = objectDataBase.GetObjectByID(objID);

        if (previewObject != null)
            Destroy(previewObject);

        previewObject = Instantiate(ObjectData.prefab);

        MeshRenderer[] renderers = previewObject.GetComponentsInChildren<MeshRenderer>();
        materials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
            renderers[i].material = previewMaterial;
        }


        // Deaktiviraj sve ne-vizuelne komponente
        Component[] components = previewObject.GetComponentsInChildren<Component>(true);
        foreach (Component component in components)
        {
            if (component is Renderer || component is MeshFilter || component is MeshRenderer)
                continue;

            if (component is Collider collider)
            {
                collider.enabled = false; // ili true ako koristiš raycast
                continue;
            }

            if (component is Behaviour behaviour)
            {
                behaviour.enabled = false;
            }
        }

        log.log($"Preview started for {ObjectData.name}, ID: {objID}", this);
    }

    bool IsPreviewValid()
    {
        return placedObjectManager.IsObjectPlacedAt(inputHandler.mousePos);
    }

    /// <summary>
    /// Change the color of the preview material based on validity
    /// </summary>
    /// <param name="isValid">true: green, flase: red</param>
    void UpdateMaterial(bool isValid)
    {
        if (isValid)
        {
            SetMaterialColor(validColor);
            log.log($"Preview is valid for {ObjectData.name}, ID: {ObjectData.ID}", this);
        }
        else
        {
            SetMaterialColor(invalidColor);
            log.log($"Preview is invalid for {ObjectData.name}, ID: {ObjectData.ID}", this);
        }
    }

    void SetMaterialColor(Color color)
    {
        previewMaterial.color = color;
        /*
        foreach (MeshRenderer rend in previewObject.GetComponentsInChildren<MeshRenderer>())
        {
            
            
        }
        */
    }


    void PreviewUpdate()
    {
        if (previewObject == null) return;

        log.log($"Updating preview for {ObjectData.name}, ID: {ObjectData.ID}", this);
        previewObject.transform.position = inputHandler.mousePos;
        UpdateMaterial(IsPreviewValid());
    }
    
}

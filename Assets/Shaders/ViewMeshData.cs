using UnityEngine;
public class ViewMeshData : MonoBehaviour
{
    public static string currentLightMapName = null;
    public int lightmapIndex = 0;
    private void Start()
    {
        //UDebug.LogError("ViewMeshData Start.");        
        UpdateBakeLightMaping();
    }
    public void UpdateBakeLightMaping()
    {
        if (lightmapIndex >= 0)
        {
            if (TryGetComponent<MeshRenderer>(out MeshRenderer m))
            {
                m.lightmapIndex = lightmapIndex;
            }
        }
    }
    public bool TryGetComponent<T>(out T m) where T : Component
    {
        m = GetComponent<T>();
        return m != null;
    }
}

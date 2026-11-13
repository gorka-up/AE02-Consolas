using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesController : MonoBehaviour
{
    [SerializeField] AssetReferenceGameObject box;
    [SerializeField] Transform[] positions;

    AsyncOperationHandle<GameObject> handle;
    readonly List<GameObject> boxes = new List<GameObject>();
    bool loaded;

    void OnTriggerEnter(Collider otro)
    {
        if (!loaded) StartCoroutine(Create());
    }

    void OnTriggerExit(Collider otro)
    {
        if (loaded) ReleaseBoxes();
    }

    System.Collections.IEnumerator Create()
    {
        loaded = true;
        Debug.Log("LOAD CALLED");
        handle = Addressables.LoadAssetAsync<GameObject>(box);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var p in positions) boxes.Add(Instantiate(handle.Result, p.position, p.rotation));
            Debug.Log($"Create box number {boxes.Count}");
        }
        else
        {
            Debug.LogError("ERROR LOADING");
            Addressables.Release(handle);
        }

        if (boxes.Count == 0) ReleaseBoxes();
    }

    void ReleaseBoxes()
    {
        foreach (var c in boxes) Destroy(c);
        boxes.Clear();
        if (handle.IsValid())
        {
            Addressables.Release(handle);
            Debug.Log("Boxes deleted and handle release");
        }
        loaded = false;
    }
}

using System.Collections;
using UnityEngine;

public class ObjectsDisabler : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private int _retryDelayFrames = 10;

    public void Execute()
    {
        StartCoroutine(DisableObjectsCoroutine());
    }

    private IEnumerator DisableObjectsCoroutine()
    {
        foreach (var obj in _objects)
        {
            if (obj != null)
            {
                if (obj.activeSelf)
                {
                    obj.SetActive(false);

                    if (obj.activeSelf)
                    {
                        StartCoroutine(RetryDisableCoroutine(obj));
                    }
                }
                else
                {
                    Debug.Log($"Объект {obj.name} уже отключен.");
                    enabled = false;
                }
            }
            else
            {
                Debug.Log("Один из объектов в массиве _objects равен null!");
                enabled = false;
            }

            yield return null;
        }
    }

    private IEnumerator RetryDisableCoroutine(GameObject obj)
    {
        for (int i = 0; i < _retryDelayFrames; i++)
        {
            yield return null;
        }

        if (obj != null && obj.activeSelf)
        {
            obj.SetActive(false);

            if (obj.activeSelf)
            {
                Debug.LogError($"Не удалось отключить объект {obj.name} даже после повторной попытки.");
            }
            else
            {
                Debug.Log($"Повторно отключили объект {obj.name}.");
            }
        }
        else if (obj == null)
        {
            Debug.LogWarning("Объект был уничтожен до повторной попытки отключения.");
        }
    }
}

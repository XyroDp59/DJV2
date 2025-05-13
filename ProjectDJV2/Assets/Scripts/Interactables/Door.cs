using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool isOpened = false;
    [SerializeField] Transform door;
    [SerializeField] float speed;

    public void Open() { if (!isOpened) StartCoroutine(_Open()); }
    private IEnumerator _Open()
    {
        isOpened = true;
        float t = door.localScale.y;
        while(t > 0)
        {
            door.localScale = new Vector3(1, t, 1);
            t -= speed * Time.deltaTime;
            yield return null;
        }
    }
    public void Close() { if(isOpened) StartCoroutine(_Close()); }
    private IEnumerator _Close()
    {
        isOpened = false;
        float t = door.localScale.y;
        while (t < 2)
        {
            door.localScale = new Vector3(1, t, 1);
            t += speed * Time.deltaTime;
            yield return null;
        }    
    }

    public void OpenClose()
    {
        if (isOpened) { StartCoroutine(_Close()); }
        else StartCoroutine(_Open());
    }
}
